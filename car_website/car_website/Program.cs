using car_website.Data;
using car_website.Interfaces;
using car_website.Repository;
using car_website.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDB")));
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetService<IMongoClient>();
    if (client == null)
    {
        throw new Exception("IMongoClient is not registered.");
    }
    return client.GetDatabase(builder.Configuration.GetConnectionString("DBName"));
});
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddSingleton<CurrencyUpdater>();
builder.Services.AddHostedService<CurrencyUpdateService>();
builder.Services.AddScoped<ApplicationDbContext>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();