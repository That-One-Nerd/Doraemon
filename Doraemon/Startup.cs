﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Disqord.Bot.Hosting;
using Disqord.Gateway;
using Disqord.Gateway.Default;
using Doraemon.Common;
using Doraemon.Data;
using Doraemon.Data.Repositories;
using Doraemon.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Qmmands;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Filters;

namespace Doraemon
{
    internal class Internals
    {
        public static DoraemonConfiguration DoraemonConfig { get; } = new();
        public static ModerationConfiguration ModerationConfig { get; } = new();

        internal static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Filter.ByExcluding(Matching.FromSource("Disqord"))
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var builder = new HostBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("config.json", false, false)
                        .Build();

                    x.AddConfiguration(configuration);
                })
                .ConfigureDiscordBot((context, bot) =>
                {
                    bot.Token = DoraemonConfig.Token;
                    bot.Prefixes = new[]
                    {
                        DoraemonConfig.Prefix
                    };
                    bot.Intents = GatewayIntents.All;
                })
                .UseSerilog()
                .ConfigureServices(services =>
                {
                    services
                        .Configure<DefaultGatewayCacheProviderConfiguration>(x => x.MessagesPerChannel = 200)
                        .AddSingleton<HttpClient>()
                        .AddDbContext<DoraemonContext>(x =>
                            x.UseNpgsql(DoraemonConfig.DbConnection))
                        .AddDbContextFactory<DoraemonContext>(x => x.UseNpgsql(DoraemonConfig.DbConnection))
                        .AddDoraemonServices()
                        .AddDoraemonRepositories();
                })
                .UseConsoleLifetime();

            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}