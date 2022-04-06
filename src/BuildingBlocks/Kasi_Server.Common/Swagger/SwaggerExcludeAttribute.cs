using System;

namespace Kasi_Server.Common.Common.Swagger
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}