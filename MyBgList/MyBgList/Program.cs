using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// resolve conflitos de rotas duplicadas automaticamente
builder.Services.AddSwaggerGen(opts => opts.ResolveConflictingActions(apiDesc => apiDesc.First()));

#region CORS
builder.Services.AddCors(options => 
{ 
    options.AddDefaultPolicy(cfg =>
    {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    }); 
    options.AddPolicy(name: "AnyOrigin", cfg =>
    {
        cfg.AllowAnyOrigin();
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    }); 
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers().RequireCors("AnyOrign");
// using minimal api approach to get the same result as controller based api
//app.MapGet("/error", () => Results.Problem()) ;
// emulating an exception
//app.MapGet("/error/test", () => { throw new Exception("test"); });

//
app.MapGet("/error"
    , [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () => Results.Problem());
app.MapGet("/error/test"
    , [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () => { throw new Exception("test"); });
//
app.Run();
