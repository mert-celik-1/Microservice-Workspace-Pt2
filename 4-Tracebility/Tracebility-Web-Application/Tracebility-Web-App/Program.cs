using NLog.Web;
using Tracebility_Web_App.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region NLog Setup
builder.Logging.ClearProviders();
builder.Host.UseNLog();
#endregion

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<OtherMiddleware>();

app.MapGet("/", (HttpContext context, ILogger<Program> logger) =>
{
    var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
    //ya da
    correlationId = context.Items["CorrelationId"].ToString();

    NLog.MappedDiagnosticsContext.Set("CorrelationId", correlationId);
    logger.LogDebug("Minimal API Log");
});

app.Run();