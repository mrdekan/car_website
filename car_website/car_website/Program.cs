using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using car_website.Repository;
using car_website.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllers();
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWaitingCarsRepository, WaitingCarsRepository>();
builder.Services.AddScoped<IBuyRequestRepository, BuyRequestRepository>();
builder.Services.AddScoped<IPurchaseRequestRepository, PurchaseRequestRepository>();
builder.Services.AddScoped<IExpressSaleCarRepository, ExpressSaleCarRepository>();
builder.Services.AddScoped<IAppSettingsDbRepository, AppSettingsDbRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<CarDeleteService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IValidationService, ValidationService>();
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddSingleton<CurrencyUpdater>();
builder.Services.AddHostedService<CurrencyUpdateService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

    options.ApiVersionReader = new MediaTypeApiVersionReader();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddIdentity<User, Role>()
    .AddMongoDbStores<User, Role, ObjectId>(
        builder.Configuration.GetConnectionString("MongoDB"),
        builder.Configuration.GetConnectionString("DBName")
    );
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseSession();

app.UseAuthorization();
app.UseAuthentication();
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();