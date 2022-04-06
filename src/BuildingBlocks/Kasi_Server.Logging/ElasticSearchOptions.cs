namespace Kasi_Server.Logging
{
    public class ElasticSearchOptions
    {
        public ElasticSearchOptions()
        {
            ElasticsearchEnvironments = "Development,Production";
            ApplicationName = "unknownapp";
        }

        public bool Enabled { get; set; }
        public string ElasticsearchEnvironments { get; set; }
        public string ApplicationName { get; set; }
        public string ElasticsearchUrl { get; set; }
        public string ElasticsearchUser { get; set; }
        public string ElasticsearchPassword { get; set; }
    }
}