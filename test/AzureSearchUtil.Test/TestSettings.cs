using System.Configuration; 

namespace AzureSearchUtil.Test
{
    public static class TestSettings
    {
        public static string MongoDbConnectionString
        {
            get { return ConfigurationManager.AppSettings["MongoDbConnectionString"]; }
        }

        public static string MongoDbDatabaseName
        {
            get { return ConfigurationManager.AppSettings["MongoDbDatabaseName"]; }
        }

        public static string AzureSearchApiKey
        {
            get { return ConfigurationManager.AppSettings["AzureSearchApiKey"]; }
        }

        public static string AzureSearchUrlPrefix
        {
            get { return ConfigurationManager.AppSettings["AzureSearchUrlPrefix"]; }
        }

        public static string AzureSearchApiVersion
        {
            get { return ConfigurationManager.AppSettings["AzureSearchApiVersion"]; }
        }

        public static int AzureSearchBatchUpdateLimit
        {
            get { return int.Parse(ConfigurationManager.AppSettings["AzureSearchBatchUpdateLimit"]); }
        }

    }
}
