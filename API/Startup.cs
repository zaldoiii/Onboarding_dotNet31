using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DAL;
using DAL.Model;
using DAL.Repositories;
using BLL;
using BLL.Messaging;
using BLL.Redis;
using External;
using Scheduler;
using Scheduler.Job;
using Quartz.Spi;


namespace API
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Turoial", Version = "v1" });
            });

            services.AddDbContext<SoccerDbContext>(option =>
                option
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessageSenderFactory, MessageSenderFactory>();

            services.AddHostedService<MessageListener>();
            services.AddHostedService<SchedulerService>();

            services.AddSingleton<IJobFactory, QuartzJobFactory>();

            services.AddTransient<LogTimeJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tutorial Net Core v1");
            });
        }
    }
}
