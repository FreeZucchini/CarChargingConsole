using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api;

public class StartCharging
{
    private readonly ILogger<StartCharging> _logger;

    private static readonly ServiceClient serviceClient =
        ServiceClient.CreateFromConnectionString(
            Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!);

    public StartCharging(ILogger<StartCharging> logger)
    {
        _logger = logger;
    }

    [Function("StartCharging")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "charge/start")] HttpRequest req)
    {

        var methodInvocation = new CloudToDeviceMethod("StartCharging")
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

            _logger.LogWarning("Device failed to execute StartCharging with status {Status}", result.Status);
            return new OkObjectResult(new {success = false, deviceStatus = result.Status});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reach device to execute StartCharging");
            return new OkObjectResult(new {success = false, error = "Could not reach the Car1"})
            {
                StatusCode = 503
            };   
        }
    }
}
