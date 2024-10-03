using DAL.Core;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.EmployeeServices;
using DAL.Core.IConfiguration;
using DAL.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HRManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSQLConnectionString"));
});

// Identity yapýlandýrmasý
builder.Services.AddIdentity<Employee, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<HRManagementDbContext>()
    .AddDefaultTokenProviders();

// EmployeeService ve diðer baðýmlýlýklarý ekleyin
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Admin kullanýcýyý oluþturma
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    var adminRole = new IdentityRole<Guid>("Admin");
    if (!await roleManager.RoleExistsAsync(adminRole.Name))
    {
        await roleManager.CreateAsync(adminRole);
    }

    var adminUser = new Employee
    {
        UserName = "admin",
        Email = "admin@example.com"
    };

    var user = await userManager.FindByNameAsync(adminUser.UserName);
    if (user == null)
    {
        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, adminRole.Name);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Authentication middleware'i ekleyin
app.UseAuthorization();

app.MapControllers();

app.Run();
