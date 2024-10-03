using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using utgiftsoversikt.Data;
using utgiftsoversikt.Repos;
using utgiftsoversikt.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
/*builder.AddAzureCosmosClient("db"); OLD CODE

builder.Services.AddDbContext<CosmosContext>(c => c.UseCosmos(builder.Configuration.GetConnectionString("cosmos"), "db"));
*/

builder.AddAzureCosmosClient("cosmos");
builder.AddCosmosDbContext<CosmosContext>("cosmos", "db");

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IBudgetRepo, BudgetRepo>();
builder.Services.AddScoped<IBudgetService, BudgetService>();

builder.Services.AddScoped<IMonthRepo, MonthRepo>();
builder.Services.AddScoped<IMonthService, MonthService>();

builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();




builder.Services.AddDistributedMemoryCache();

// Add services to the container.



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
    builder =>
    {
        builder.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
               .SetIsOriginAllowed(origin => true); // Tillat alle opprinnelser
    });
});
builder.Services.AddControllers();

// Add auth spesific config
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login"; //Redirect if not logged in
        options.LogoutPath = "/auth/logout"; // Redirect when logged out
    });


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7062/",
        ValidAudience = "https://localhost:59294/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VeryStrongLoooooongAndSecretValue"))
    };
}
    
    );

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});




builder.Services.AddHttpContextAccessor();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Activates auth
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
