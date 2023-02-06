using TaskRegister.IDP;
using NLog.Web;
using NLog;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.Run();
}
// https://github.com/dotnet/runtime/issues/60600
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}