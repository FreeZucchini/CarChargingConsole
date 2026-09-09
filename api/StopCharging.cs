using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace api;

public class StopCharging
{
    private readonly ILogger<StopCharging> _logger;

    public StopCharging(ILogger<StopCharging> logger)
    {
        _logger = logger;
    }

    [Function("StopCharging")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "charge/stop")] HttpRequest req)
    {
        string serviceConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!;
        using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);

        var methodInvocation = new CloudToDeviceMethod("StopCharging")
        {
            ResponseTimeout = TimeSpan.FromSeconds(10)
        };

        try
        {
            CloudToDeviceMethodResult result = await serviceClient.InvokeDeviceMethodAsync("Car1", methodInvocation);

            if (result.Status == 200)
            {
                return new OkObjectResult(new {success = true, deviceStatus = result.Status});
            }

            _logger.LogWarning("Device failed to execute StopCharging with status {Status}", result.Status);
            return new OkObjectResult(new {success = false, deviceStatus = result.Status})
            {
                StatusCode = 502
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reach device to execute StopCharging");
            return new OkObjectResult(new {success = false, error = "Could not reach the Car1"})
            {
                StatusCode = 503
            };   
        }
    }
}
