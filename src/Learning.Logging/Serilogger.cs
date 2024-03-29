﻿using Microsoft.Extensions.Hosting;
using Serilog;

namespace Learning.Logging;

public static class SeriLogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration)
            => configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .ReadFrom.Configuration(context.Configuration);
}
