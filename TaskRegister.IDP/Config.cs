using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace TaskRegister.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(), //Soporte para el alcance del perfil de los usuarios
            new IdentityResource("roles",
                    "Your role(s)",
                    new [] {"role"})
        };

    public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
                {
                    new ApiResource("taskregisterapi", "Task Register Api")
                    {
                        ApiSecrets =
                        {
                            new Secret("taskRegisterApiSecret".Sha256())
                        },
                        Scopes = { "taskregisterapi.fullaccess" },
                        UserClaims = { "role", "admin", "user", "taskRegisterApiSecret", "taskRegisterApiSecret.admin", "taskRegisterApiSecret.user" }
                    }
                };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("taskregisterapi.fullaccess")
            };

    public static IEnumerable<Client> Clients(IConfigurationSection stsConfig)
    {
        var clientUrl = stsConfig["ClientUrl"];

        return new Client[]
            {
                new Client
                {
                    ClientName = "Task Register App",
                    ClientId = "angularClientForTaskRegisterApi",
                    AccessTokenLifetime = 300,// 900 seconds, default 60 minutes
                    IdentityTokenLifetime = 60,
                    // RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        clientUrl
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{clientUrl}/login",
                        clientUrl
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        clientUrl
                    },
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "taskregisterapi.fullaccess"
                    },
                    
                    //AllowOfflineAccess = true,
                    //RefreshTokenUsage = TokenUsage.OneTimeOnly
                }, };
    }
}