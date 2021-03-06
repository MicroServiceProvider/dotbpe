﻿
using System;
using DotBPE.Protocol.Amp;
using System.Threading.Tasks;
using DotBPE.Rpc.Extensions;
using DotBPE.Rpc.Hosting;
using HelloRpc.Common;
using DotBPE.Plugin.Logging;
using DotBPE.Rpc;
using Microsoft.Extensions.DependencyInjection;
using DotBPE.Rpc.Logging;

namespace HelloRpc.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            NLoggerWrapper.InitConfig();
            DotBPE.Rpc.Environment.SetLogger(new NLoggerWrapper(typeof(Program)));

            var host = new RpcHostBuilder()
                .UseStartup<Startup>()
                .Build();

            host.StartAsync().Wait();

            Console.WriteLine("Press any key to quit!");
            Console.ReadKey();

            host.ShutdownAsync().Wait();

        }
    }
    public class Startup : IStartup
    {
        static ILogger Logger = DotBPE.Rpc.Environment.Logger.ForType<Startup>();
        public void Configure(IAppBuilder app, IHostingEnvironment env)
        {
            Logger.Debug("Startup Configure");
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Logger.Debug("Startup ConfigureServices");

            services.AddDotBPE();

            services.AddServiceActors<AmpMessage>(actors =>{
                actors.Add<GreeterImpl>();
            });

            return services.BuildServiceProvider();
        }
    }
    public class GreeterImpl : GreeterBase
    {
        public override Task<HelloResponse> HelloAsync(HelloRequest request)
        {
            return Task.FromResult(new HelloResponse() { Message = "Hello " + request.Name });
        }
    }

}
