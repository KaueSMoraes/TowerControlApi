using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AssemblyMaster.Services;
using Microsoft.OpenApi.Models;
using AssemblyMaster.Utilities;
using Microsoft.AspNetCore.Authentication;

namespace AssemblyMaster
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IServerService, ServerService>();
            services.AddScoped<IActionsService, ActionsService>();
            services.AddControllers(options => options.Filters.Add<ApiExceptionFilter>())
                .AddNewtonsoftJson();
            services.AddEndpointsApiExplorer();
            services.AddScoped<ServerService>();
            services.AddScoped<ActionsService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AAServicePainel", Version = "v1" });
                c.SchemaFilter<EnumSchemaFilter>();
                c.EnableAnnotations();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication(); // Autenticação deve vir antes da Autorização
            app.UseAuthorization(); // Autorização deve vir antes dos Endpoints

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AAServicePainel V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}