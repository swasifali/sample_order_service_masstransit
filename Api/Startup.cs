using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OrderSample.Contracts;
using Microsoft.OpenApi.Models;

namespace OrderSample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = check => check.Tags.Contains("ready");
            });

            services
                .AddHealthChecksUI()
                .AddInMemoryStorage();

            services.AddMassTransit(x => {

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });


                //comment the RabbitMq above and uncomment the below to use Azure Service Bus

                //bool useManagedIdentity = Convert.ToBoolean(Configuration["useManagedIdentity"]);
                //x.UsingAzureServiceBus((context, cfg) =>
                //{
                //    cfg.UseHealthCheck(context);

                //    if (useManagedIdentity)
                //    {
                //        cfg.Host(new Uri("sb://[your_sb_namespace].servicebus.windows.net/"), hostCfg =>
                //        {
                //            var tokenProvider = TokenProvider.CreateManagedIdentityTokenProvider();
                //            hostCfg.TokenProvider = tokenProvider;
                //        });
                //    }
                //    else
                //    {
                //        string endpoint = ""; //enter your sb connection string
                //        cfg.Host(endpoint);
                //    }

                //    cfg.ConfigureEndpoints(context);
                //});


                //register consumers, request clients or sagas

                //if using request / response paradigm
                x.AddRequestClient<SubmitOrderRequest>();
            });

            services.AddMassTransitHostedService();

            services.AddControllers();

            services.AddSwaggerGen();
            //services.ConfigureSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("Order Api", new OpenApiInfo
            //    {
            //        Version = "v1",
            //        Title = "Order Api",
            //        Description = "Sample Order Api using MassTransit"
            //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());

                endpoints.MapHealthChecksUI();

                endpoints.MapControllers();
            });
        }
    }
}
