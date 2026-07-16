using EcoMeal.API.Application.Constants;
using EcoMeal.Backend.Entities;
using EcoMeal.Backend.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenApi();
builder.Services.AddDbContext<EcoMealDbContext>
   (options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
   );

builder.Services.AddHostedService<ExpiredPackageCleanupService>();

builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole<int>>()
.AddEntityFrameworkStores<EcoMealDbContext>();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
options.AddPolicy("AllowBlazorSite",
    policy =>
    {
        policy.WithOrigins("https://localhost:5263")
        .AllowAnyHeader()
        .AllowAnyMethod();
    }
));
builder.Services.AddControllers();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json","EcoMeal API");
    });
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowBlazorSite");

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<User>();

app.MapControllers();

await SeedRolesAsync(app);



app.Run();

static async Task SeedRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var dbContext = services.GetRequiredService<EcoMealDbContext>();

    await dbContext.Database.MigrateAsync();

    var roles = new[] { UserRoles.Admin, UserRoles.User };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
        }
    }
}
