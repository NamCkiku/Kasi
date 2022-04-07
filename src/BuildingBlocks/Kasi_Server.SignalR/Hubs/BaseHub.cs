using Kasi_Server.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Kasi_Server.SignalR.Hubs
{
    //[JwtAuth]
    public class BaseHub<T> : Hub<T> where T : class
    {
        private readonly IConnectionMapping<string> connections;

        private readonly ILogger<BaseHub<T>> _logger;

        public BaseHub(ILogger<BaseHub<T>> logger, IConnectionMapping<string> connections)
        {
            _logger = logger;
            this.connections = connections;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var http = Context.GetHttpContext();
                var fk_UserID = http.Request.Query["UserID"].ToString();
                if (!connections.GetConnections(fk_UserID.ToUpper()).Contains(Context.ConnectionId))//Check xem user này đã có ConnectionId chưa
                {
                    connections.Add(fk_UserID.ToUpper(), Context.ConnectionId);
                }
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                await base.OnDisconnectedAsync(exception);
                var http = Context.GetHttpContext();
                var fk_UserID = http.Request.Query["UserID"].ToString();
                if (connections.GetConnections(fk_UserID.ToUpper()).Contains(Context.ConnectionId))//Check xem user này đã có ConnectionId chưa
                {
                    connections.Remove(fk_UserID.ToUpper(), Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public int GetCountUserOnline()
        {
            return connections.Count;
        }
    }
}