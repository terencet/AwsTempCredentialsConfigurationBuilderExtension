using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AwsCredentialsConfigurationBuilderExtension
{
    public static class AwsCredentialsConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddTempAwsCredentialsToEnvironmentVariables(this IConfigurationBuilder configurationBuilder, string tempAwsCredentialFilePath, string awsRegion = "us-east-1")
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), tempAwsCredentialFilePath);
            if (File.Exists(filePath))
            {
                var jsonContent = File.ReadAllText(filePath);
                var tempAwsCredentials = JsonConvert.DeserializeObject<TempAwsCredentials>(jsonContent);
                Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", tempAwsCredentials.AWS_ACCESS_KEY_ID);
                Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", tempAwsCredentials.AWS_SECRET_ACCESS_KEY);
                Environment.SetEnvironmentVariable("AWS_SESSION_TOKEN", tempAwsCredentials.AWS_SESSION_TOKEN);
                Environment.SetEnvironmentVariable("AWS_REGION", awsRegion);
            }
            else
            {
                throw new FileNotFoundException($"{tempAwsCredentialFilePath}");
            }

            return configurationBuilder;
        }

        [Serializable]
        private class TempAwsCredentials
        {
            public string AWS_ACCESS_KEY_ID { get; set; }
            public string AWS_SECRET_ACCESS_KEY { get; set; }
            public string AWS_SESSION_TOKEN { get; set; }
        }
    }
}
