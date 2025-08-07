using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.MongoDb;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ILeagueService, LeagueService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTeamService, UserTeamService>();
builder.Services.AddScoped<IAthleteService, AthleteService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IRosterService, RosterService>();

builder.Services.AddDbContext<FantasyFootballContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FantasyFootballContext>();


var app = builder.Build();

app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200"));
app.UseAuthorization();
app.MapGroup("api").MapIdentityApi<AppUser>();

app.UseHttpsRedirection();
app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    
    var db = services.GetRequiredService<FantasyFootballContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    await db.Database.MigrateAsync();
    await FantasyFootballContextSeed.SeedAsync(db, userManager, roleManager);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
