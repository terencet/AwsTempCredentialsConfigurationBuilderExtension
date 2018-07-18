using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit;

namespace AwsCredentialsConfigurationBuilderExtension.Tests
{
    public class AwsCredentialsConfigurationBuilderExtensionTests
    {
        [Fact]
        public void ShouldThrowExceptionWhenTempCredentialJsonFileNotFound()
        {
            var configurationBuilder = new ConfigurationBuilder();
            Assert.Throws<FileNotFoundException>(() => configurationBuilder.AddTempAwsCredentialsToEnvironmentVariables("aws-credentials-not-found.json"));
        }

        [Fact]
        public void AwsRegionShouldBeUSEastWhenRegionIsNotProvided()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddTempAwsCredentialsToEnvironmentVariables("temp-aws-credentials.json");
            Assert.Equal("us-east-1", Environment.GetEnvironmentVariable("AWS_REGION"));
        }

        [Fact]
        public void ShouldSetAllEnvironmentVariables()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddTempAwsCredentialsToEnvironmentVariables("temp-aws-credentials.json");
            Assert.Equal("us-east-1", Environment.GetEnvironmentVariable("AWS_REGION"));
            Assert.Equal("A", Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));
            Assert.Equal("B", Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
            Assert.Equal("C", Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN"));
        }

        [Fact]
        public void ShouldBeTheRegionSpecified()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddTempAwsCredentialsToEnvironmentVariables("temp-aws-credentials.json", "ap-southeast-2");
            Assert.Equal("ap-southeast-2", Environment.GetEnvironmentVariable("AWS_REGION"));
            Assert.Equal("A", Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));
            Assert.Equal("B", Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
            Assert.Equal("C", Environment.GetEnvironmentVariable("AWS_SESSION_TOKEN"));
        }

        [Fact]
        public void ShouldThrowExceptionIfContentCannotBeParsed()
        {
            var configurationBuilder = new ConfigurationBuilder();
            Assert.Throws<JsonReaderException>(() => configurationBuilder.AddTempAwsCredentialsToEnvironmentVariables("bad-formatted.json"));
        }
    }
}