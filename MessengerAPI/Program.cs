using Microsoft.EntityFrameworkCore;
using MessengerAPI.Models;
using MessengerAPI.Services;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация DbContext с подключением к SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация Identity с типом ключа int
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options => 
        options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Регистрация UserService
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<MessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
 
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var db = service.GetRequiredService<ApplicationDbContext>();
    var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
    await db.Database.MigrateAsync();
    await SeedData.Initialize(service);
}

 app.Run();