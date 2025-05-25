using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using StudyTracker.Services;

namespace StudyTracker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // Loads configuration from appsettings.json
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Registers EF Core with MariaDB
            services.AddDbContext<StudyDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))
                ));

            // Registers dependency injection for service layer
            services.AddScoped<IStudyService, StudyService>();

            // Allow any origin for demo/testing purposes
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Enables Swagger for API documentation
            services.AddSwaggerGen();
        }

        // This method configures the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show detailed errors in development
                app.UseSwagger();                // Enable Swagger UI
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Maps controller endpoints
            });
        }
    }
}
