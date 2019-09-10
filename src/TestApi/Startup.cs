using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace TestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection()
               .UseMvc()
               .UseSwagger()
               .UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "v1"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSwaggerGen(options => options.SwaggerDoc("v1", new Info { Version = "v1" }))
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}