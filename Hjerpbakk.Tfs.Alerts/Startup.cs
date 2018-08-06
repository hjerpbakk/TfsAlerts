using System.ServiceModel.Channels;
using Hjerpbakk.Tfs.Alerts.Configuration;
using Hjerpbakk.Tfs.Alerts.Services;
using Hjerpbakk.Tfs.Alerts.Slack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlackConnector;
using SoapCore;

namespace Hjerpbakk.Tfs.Alerts {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .AddJsonFile($"config.json", true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            var config = Configuration.Get<AppConfig>();
            services.AddSingleton<IAppConfig>(config);
            services.AddSingleton<ISlackConnector, SlackConnector.SlackConnector>();
            services.AddSingleton<ISlackIntegration, SlackIntegration>();

            services.AddSingleton<INotifyService, NotifyService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var textBindingElement = new TextMessageEncodingBindingElement(MessageVersion.Soap12WSAddressing10, System.Text.Encoding.UTF8);
            var httpBindingElement = new HttpTransportBindingElement {
                AllowCookies = true,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue
            };
            var soap12Binding = new CustomBinding(textBindingElement, httpBindingElement);

            app.UseSoapEndpoint<INotifyService>("/NotifyService.svc", soap12Binding);
            app.UseStaticFiles();
        }
    }
}
