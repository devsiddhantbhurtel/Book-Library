using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Data;
using BookLibrarySystem.Repositories;
using BookLibrarySystem.Services;  
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookLibrarySystem.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add logging to verify connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection string: {connectionString}");

// Generate a unique server session key on startup
var serverSessionKey = Guid.NewGuid().ToString();
builder.Services.AddSingleton(new BookLibrarySystem.ServerSessionKey { Value = serverSessionKey });

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

// Add DbContext configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookPublisher, BookPublisherService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>(); 

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder
            .WithOrigins("http://localhost:3000") // Change to your frontend's actual origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Add Cookie Authentication for MVC and API
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/Login";
        options.Events.OnRedirectToLogin = context =>
        {
            // If the request is for an API endpoint, return 401 Unauthorized instead of redirecting
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();

// Add SignalR services
builder.Services.AddSignalR();

// Register StaffNotificationService
builder.Services.AddScoped<StaffNotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowFrontend"); // Enable CORS for frontend with credentials

app.UseAuthentication();
app.UseMiddleware<BookLibrarySystem.Middleware.SessionKeyValidationMiddleware>();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();

// Enable support for MVC Areas (Admin panel at /admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{id?}");

// Map default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Explicitly map default controller route for MVC
app.MapDefaultControllerRoute();

// Map SignalR hub
app.MapHub<StaffNotificationHub>("/staffNotificationHub");

app.Run();
