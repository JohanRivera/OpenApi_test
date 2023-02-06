using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskRegister.IDP.Entities;
using TaskRegister.IDP.DbContexts;
using TaskRegister.IDP.Services;

namespace TaskRegister.IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        #region Definición de scopes

        builder.Services.AddScoped<ILocalUserService, LocalUserService>();
        builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        #endregion

        #region Cadena conexión SQLite

        builder.Services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDb"));
        });

        #endregion

        #region Configuración Identity Server

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddProfileService<LocalUserProfileService>()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryClients(Config.Clients(builder.Configuration.GetSection("StsConfig")));

        #endregion

        builder.Services.AddControllers();

        #region Configuración OpenAPI

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion

        #region Habilitar cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowedHosts",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200",
                                      "https://localhost:44360")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        #endregion

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowedHosts");

        app.MapControllers();

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
