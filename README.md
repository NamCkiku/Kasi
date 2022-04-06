# ASP.NET Core 3.1 project from BA GPS
========================================

## Công nghệ sử dụng

- ASP.NET Core 3.1
- Entity Framework Core 3.1
- Dapper ORM  2.0.30
- AutoMapper 9.0.0
- FluentValidation 8.6.2
- Serilog 2.8.0
- XUnit 2.4.0
- Newtonsoft.Json 12.0.3
- IdentityServer4.AspNetIdentity 3.1.2
- Swagger 5.3.1

## Cài đăt Tools

- .NET Core SDK 3.1
- Git client
- Visual Studio 2019
- SQL Server 2019


## Làm thế nào để config chạy project

- Clone code từ GITBUCKET: git clone http://192.168.1.65:7990/scm/ppm/ba_gps_servercore.git
- Mở solution BA_GPS_Server.sln với Visual Studio 2019
- Set startup project is BA_GPS_Server.Services.Api
- Thay đổi ConnectionString in Appsetting.json trong project BA_GPS_Server.Services.Api
- Chọn profile to run hoặc nhấn phím F5


## Một số link tài liệu tham khảo

- Dapper CRUD: https://github.com/ericdc1/Dapper.SimpleCRUD
- CQRS – Simple architecture : https://www.future-processing.pl/blog/cqrs-simple-architecture/

## Khung cấu trúc tổng quan của BA_GPS_Server

![](resources/images/Diagram.png)

### Cấu trúc dự án được thiết kế dựa trên một số project tham khảo trên github.

![](resources/images/gpsserverarchitecture.png)

### Kiến trúc được chia thành 4 thư mục chính
- 0 - Solution Items : Thư mục này chứa file README.md để viết hướng dẫn về dự án và tổng quan về kiến trúc dự án BA GPS API NET Core
- 1 - ApiGateways : Thư mục ApiGateways chứa các Project ApiGetway là cổng trung gian duy nhất tới hệ thống microservices GPS
- 2 - Services : Thư mục Services chứa các Project ASP.NET Core chủ yếu để phân nhỏ các Service ra lai với cấu trúc Microservice như: OnlineService, RouteService , ApiService,IdentityServer,...
- 3 - BuildingBlocks : Thư mục chứa toàn bộ các project dùng chung cho toàn bộ dự án
- 4 - UnitTest: Thư mục chưa các Unit Test của toàn bộ Project

## Chi tiết chức năng và nhiệm vụ của từng Project

### 1 - ApiGateways(ApiGateway)
- ApiGateway
- API Gateway có thể coi là một cổng trung gian, nó là cổng vào duy nhất tới hệ thống microservices của chúng ta, api gateway sẽ nhận các requests từ phía client, chỉnh sửa, xác thực và điều hướng chúng đến các API cụ thể trên các services phía sau. Khi này sơ đồ hệ thống của chúng ta sẽ trông như này

![](resources/images/apigateway.png)

- Clients sẽ tương tác với hệ thống của chúng ta thông qua api gateway chứ không gọi trực tiếp tới một services cụ thể, các endpoints của các services sẽ chỉ được gọi nội bộ, tức là gọi giữa các services với nhau hoặc được gọi từ API gateway, người dùng sẽ gọi các api này thông qua các public endpoints từ API Gateway

### 2 - Services(API Common Service,IndentityServer,OnlineService, ReportService,....)

- API Common Service là một service asp.net core api dùng để cấp các api cho Apigateway để cho client Mobile hoặc Web gọi ,Service bao gồm các Controller Common như lấy danh sách Menu cho App, lấy danh sách các cấu hình hệ thống, công ty,....

- IndentityServer là một service chuyên phục vụ cho mục đích Authentication chức thực và cấp quyền cho người dùng. Sau có thể làm đăng nhập Single sign on

- OnlineService là một service với mục đích lấy danh sách xe Online của người dùng từ Mem

- ReportService là một service với mục đích lấy những thông tin báo cáo

### 3 - BuildingBlocks(BuildingBlocks Shared)

- Application là một Class Library dùng để triển khai các logic nghiệp vụ của dự án có sử dụng một số nuget như AutoMapper, FluentValidation
- AutoMapper dùng để map các đối tượng entities và viewmodel với nhau
- FluentValidation là một khuôn khổ xác nhận rất mạnh mẽ cho phép nhiều tính năng mà khuôn khổ ComponentModel thường không cung cấp đó là lý do nhiều người đang sử dụng nó trong phần mềm chính thức.

- 3.1 - Domain(Domain.Core,Domain.Entities,...)

- Domain nơi chứa các đối tượng của một phần mềm, có một nhóm các đối tượng có định danh riêng, những định danh - tên gọi này của chúng được giữ nguyên xuyên suốt trạng thái hoạt động của phần mềm. Các đối tượng này thường được map với Database để định danh các cột , các bảng trong cơ sở dữ liệu

- 3.2 - Data : 

- BA_GPS_Server.Infrastructure nơi chứa các lớp trừu tượng interface dùng chung và cũng là nơi chưa DBcontex để kết nối với cơ sở dữ liệu
- BA_GPS_Server.Infrastructure.EFCore là nơi triển khai của các lớp trừu tượng interface của BA_GPS_Server.Infrastructure với các hàm CRUD. Ở đây mình có chia thành 2 thư mục là Dapper và EFCore. Thư mục Dapper là đùng để triển khai Repository Parten với ORM Dapper. Thư mục EFCore là dùng để triển khai Repository Parten với EntityFramewordCore
- Ở đây mình cũng có sử dụng kiến trúc CRQS là một pattern được viết tắt bởi Command Query Responsibility Segregation, dịch nôm na là phân tách vai trò Command (tượng trưng cho việc ghi dữ liệu) và Query (tượng trưng cho việc đọc dữ liệu). CQRS được mô tả lần đầu bởi tác giả Greg Young
- Bạn có thể đọc qua về CRQS tại đây : https://martinfowler.com/bliki/CQRS.html

- 3.4- Logging : 

- Project này sẽ phục vụ cho việc ghi log của toàn bộ dự án. Hiện tại mình đang dùng Serilog để ghi log một cách dễ dàng và tra lại cũng tiện nữa. Sau này muốn thay thế các kiểu ghi log khác thì có thể thêm vào đây

- 3.6 - Common : Bao gồm các class dùng chung 
- 3.7 - Utils : Bao gồm các class chúng ta dùng chung, các tiện ích (Extension, Helper,..). Cái này thì thường dự án nào cũng sẽ có



### 7 - UnitTest

-- Ở đây chúng ta sẽ triển khai viết UnitTest cho toàn bộ code được viết ra trong dự án . Nó để đảm báo code không lỗi và nghiếp vụ không sai và dễ dàng phục vụ cho đẩy code tự động