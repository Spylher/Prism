using FluentValidation;
using Prism.Application.DependencyInjection;
using Prism.Application.Validators;
using Prism.Infrastructure.DependencyInjection;
namespace Prism.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Swagger services by Swashbuckle.AspNetCore
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // validation by FluentValidation.DependencyInjectionExtensions
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterClientRequestValidator>();

        // Application / Infra services
        builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
        builder.Services.AddApplication();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();

            app.MapGet("/", () => Results.Redirect("/swagger"));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Prism API Documentation";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prism API V1");
            });
        }

        app.UseHttpsRedirection();

        // important for cookie auth, must be before UseAuthorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
