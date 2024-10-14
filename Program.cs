using System.Reflection;
using BookStoreApi.middleware;
using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddSingleton<BooksService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BookStore API",
        Description = "An ASP.NET Core Web API for managing book items",
        TermsOfService = new Uri("https://localhost:5096"),
        Contact = new OpenApiContact
        {
            Name = "tsxepo",
            Url = new Uri("https://localhost:5096"),
        },
        License  = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://localhost:5096")
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
    options.CustomOperationIds(ApiDescription => 
    {
        return ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.DisplayOperationId();
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
