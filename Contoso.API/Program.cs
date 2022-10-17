using Contoso.API.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

namespace Contoso.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient(Constants.SecurityAPIClient, httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://localhost:7123/");
            });

            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            // As always, handlers must be provided for the requirements of the authorization policies
            builder.Services.AddSingleton<IAuthorizationHandler, CanGetWeatherAuthorizationHandler>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.CanGetWeatherPolicyName, policy =>
                    policy.Requirements.Add(new CanGetWeatherRequirement()));
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}