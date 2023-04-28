using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region VERSIONING
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "MyBGList", Version = "v1.0" });
    opts.SwaggerDoc("v2", new OpenApiInfo { Title = "MyBgList", Version = "v2.0" });

    //opts.ResolveConflictingActions(apiDesc => apiDesc.First());
});
builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
#endregion

var app = builder.Build();
#region VERSIONING
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/v1/swagger.json", $"MyBGList v1");
    options.SwaggerEndpoint($"/swagger/v2/swagger.json", $"MyGBList v2");
});
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/v{version:ApiVersion}/error",
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
Results.Problem());

app.MapGet("/v{version:ApiVersion}/error/test",
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
{ throw new Exception("test"); });

app.MapGet("/v{version:ApiVersion}/cod/test",
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[EnableCors("AnyOrigin")]
[ResponseCache(NoStore = true)] () =>
Results.Text("<script>" +
    "window.alert('Your client supports JavaScript!" +
    "\\r\\n\\r\\n" +
    $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
    "\\r\\n" +
    "Client time (UTC): ' + new Date().toISOString());" +
    "</script>" +
    "<noscript>Your client does not support JavaScript</noscript>",
    "text/html"));

app.Run();
