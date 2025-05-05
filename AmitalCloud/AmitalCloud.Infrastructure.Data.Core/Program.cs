using AmitalCloud.Infrastructure.Data.Counters;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Data.Services;
using AmitalCloud.Infrastructure.Data.Validators;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json (this is the default behavior)
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// Register services
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// DI for HttpContext
builder.Services.AddHttpContextAccessor();
var httpContextAccessor = builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
HttpContextHelper.Initialize(httpContextAccessor);

// create one instance per http request
builder.Services.AddScoped<ILoggedContactUtil, LoggedContactUtil>();
// builder.Services.AddScoped<IRulesValidator, RulesValidator>();
builder.Services.AddScoped<IClassLevelValidator, ClassLevelValidator>();
builder.Services.AddScoped<LoggedContactResolver>();

// DI for Configuration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // maybe not required
builder.Services.AddScoped<GlobalDbHelper>();
builder.Services.AddScoped<GlobalDBRepository>();
ConfigurationHelper.Initialize(builder.Configuration);

// pass connection string to services
// var configuration = builder.Configuration;
string connectionStringName = AmitalCloudSettings.DatabaseManagementSystem == "oracle" ? "Oracle_Globalstr" : "Globalstr";
string connectionString = builder.Configuration.GetConnectionString(connectionStringName);
builder.Services.AddSingleton(new TenantCounter(connectionString));

var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.UseRouting();
// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();

app.Run();


