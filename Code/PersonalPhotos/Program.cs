using System.Runtime.CompilerServices;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalPhotos.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

RegisterServices(builder);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

ApplyStandardUseStatements(app);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Photos}/{action=Display}");

app.Run();

void RegisterServices(WebApplicationBuilder appBuilder)
{
    appBuilder.Services.AddSession();
    appBuilder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    appBuilder.Services.AddSingleton<IKeyGenerator, DefaultKeyGenerator>();
    appBuilder.Services.AddScoped<ILogins, SqlServerLogins>();
    appBuilder.Services.AddScoped<IPhotoMetaData, SqlPhotoMetaData>();
    appBuilder.Services.AddScoped<IFileStorage, LocalFileStorage>();
    appBuilder.Services.AddScoped<LoginAttribute>();
}

void ApplyStandardUseStatements(WebApplication theApp)
{
    theApp.UseHttpsRedirection();
    theApp.UseStaticFiles();
    theApp.UseRouting();
    theApp.UseAuthorization();
    theApp.UseSession();
}