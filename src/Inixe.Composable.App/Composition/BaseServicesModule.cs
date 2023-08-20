// -----------------------------------------------------------------------
// <copyright file="BaseServicesModule.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Autofac;
    using AWS.Logger;
    using AWS.Logger.SeriLog;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Extensions.Autofac.DependencyInjection;

    internal class BaseServicesModule : Module
    {
        private readonly string[] args;

        public BaseServicesModule()
            : this(Array.Empty<string>())
        {
        }

        public BaseServicesModule(string[] args)
        {
            this.args = args;
        }

        private IEnumerable<KeyValuePair<string, string>> DefaultConfigValues
        {
            get
            {
                var values = new Dictionary<string, string>();
                values.Add(Constants.LocalPluginsDirectory, Constants.DefaultPluginDirectory);

                return values;
            }
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (!builder.Properties.ContainsKey(nameof(BaseServicesModule)))
            {
                this.RegisterBaseServices(builder);
                builder.Properties.Add(nameof(BaseServicesModule), true);

                base.Load(builder);
            }
        }

        private static LoggerConfiguration CreateLoggingConfiguration(IConfiguration configuration)
        {
            var awsLoggerConfig = new AWSLoggerConfig();
            awsLoggerConfig.Region = configuration.GetValue("Logging:Cloudwatch:Region", "us-west-2");
            awsLoggerConfig.ServiceUrl = configuration.GetValue("Logging:Cloudwatch:Url", "http://logging.inixe.com.mx");

            var loggerConfig = new LoggerConfiguration();
            var formatter = new Serilog.Formatting.Json.JsonFormatter();

            loggerConfig.WriteTo.AWSSeriLog(awsLoggerConfig, CultureInfo.InvariantCulture, formatter);

            return loggerConfig;
        }

        private void RegisterBaseServices(ContainerBuilder builder)
        {
            var configuration = this.InitializeConfiguration();
            var loggerConfig = CreateLoggingConfiguration(configuration);

            builder.RegisterSerilog(loggerConfig);
            builder.RegisterInstance(configuration)
                .AsImplementedInterfaces();
        }

        private IConfigurationRoot InitializeConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection();
            configBuilder.AddCommandLine(this.args);

            return configBuilder.Build();
        }
    }
}
