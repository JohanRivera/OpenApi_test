using IdentityServer4.AccessTokenValidation;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using TaskRegister.API.DbContexts;
using TaskRegister.API.Services.ProjectsService;
using TaskRegister.API.Services.TaskRegister;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Host.UseNLog();

    #region Definición de scopes

    builder.Services.AddScoped<IProjectsService, ProjectsService>();
    builder.Services.AddScoped<ITaskRegisterService, TaskRegisterService>();

    #endregion

    builder.Services.AddControllers();

    #region Cadena conexión SQLite

    builder.Services.AddDbContext<TaskRegisterContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDb"));
    });

    #endregion

    #region Configuración OpenApi

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    #endregion

    #region Configuración Autenticación

    builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
        .AddIdentityServerAuthentication(options =>
        {
            //Configuration Security Token Server (STS)
            options.Authority = "https://localhost:5001";
            options.ApiName = "taskregisterapi";
            options.ApiSecret = "taskRegisterApiSecret";
        });

    #endregion

    #region Configuración cors

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowOrigins",
            builder =>
            {
                builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowOrigins");

    app.UseAuthentication(); // Para habilitar la autenticación configurada

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}catch(Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    Console.Write(ex.Message);
    logger.Error(ex);
}
finally
{
    Console.Write("Fin");
    LogManager.Shutdown();
}
