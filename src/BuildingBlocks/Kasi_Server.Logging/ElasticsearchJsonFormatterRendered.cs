using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using System;
using System.IO;

namespace Kasi_Server.Logging
{
    public class ElasticsearchJsonFormatterRendered : ElasticsearchJsonFormatter
    {
        protected override void WriteLevel(LogEventLevel level, ref string delim, TextWriter output)
        {
            var intLevel =(int)level;
            WriteJsonProperty("level", intLevel, ref delim, output);
        }
    }
}