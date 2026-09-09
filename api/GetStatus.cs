using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Devices;

namespace api;

public class GetStatus
{
    private readonly ILogger<GetStatus> _logger;

    private static readonly ServiceClient _serviceClient =
        ServiceClient.CreateFromConnectionString(
            Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!);

    public GetStatus(ILogger<GetStatus> logger)
    {
        _logger = logger;
    }

    [Function("GetStatus")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "status")] HttpRequest req)
    {
        req.HttpContext.Response.Headers.Append("Cache-Control", "no-store");

        var methodInvocation = new CloudToDeviceMethod("GetBatteryLevel")
        {
            ResponseTimeout = TimeSpan.FromSeconds(10)
        };

        try
        {
            CloudToDeviceMethodResult result = await _serviceClient.InvokeDeviceMethodAsync("Car1", methodInvocation);
            string payloadJson = result.GetPayloadAsJson() ?? "{}";

            return new ContentResult
            {
                Content = payloadJson,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reach simulator for status");
            return new ObjectResult(new { success = false, error = "Could not reach Car1" })
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable
            };
        }
    }
}