using System.Configuration;
using AzureSearchUtil.Exceptions;

namespace AzureSearchUtil.Demo
{
    public static class TestSettings
    {
        public static string AzureSearchApiKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AzureSearchApiKey"]))
                {
                    throw new InvalidConfigurationException("Unable to read 'AzureSearchApiKey' from app.config file!");
                }
                else
                {
                    return ConfigurationManager.AppSettings["AzureSearchApiKey"];
                }
            }
        }

        public static string AzureSearchUrlPrefix
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AzureSearchUrlPrefix"]))
                {
                    throw new InvalidConfigurationException("Unable to read 'AzureSearchUrlPrefix' from app.config file!");
                }
                else
                {
                    return ConfigurationManager.AppSettings["AzureSearchUrlPrefix"];
                }
            }
        }

        public static string AzureSearchApiVersion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AzureSearchApiVersion"]))
                {
                    throw new InvalidConfigurationException("Unable to read 'AzureSearchApiVersion' from app.config file!");
                }
                else
                {
                    return ConfigurationManager.AppSettings["AzureSearchApiVersion"];
                }
            }
        }

        public static int AzureSearchBatchUpdateLimit
        {
            get
            {

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AzureSearchBatchUpdateLimit"]))
                {
                    throw new InvalidConfigurationException("Unable to read 'AzureSearchBatchUpdateLimit' from app.config file!");
                }
                else
                {
                    return int.Parse(ConfigurationManager.AppSettings["AzureSearchBatchUpdateLimit"]);
                }
            }
        }

    }
}
