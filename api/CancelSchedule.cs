using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Devices;

namespace api;

public class CancelSchedule
{
    private readonly ILogger<CancelSchedule> _logger;

    public CancelSchedule(ILogger<CancelSchedule> logger)
    {
        _logger = logger;
    }

    [Function("CancelSchedule")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "schedule/cancel")] HttpRequest req)
    {
        string serviceConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!;
        using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);

        var methodInvocation = new CloudToDeviceMethod("CancelSchedule")
        {  
            ResponseTimeout = TimeSpan.FromSeconds(10)
        };

        string body = await new StreamReader(req.Body).ReadToEndAsync();

        methodInvocation.SetPayloadJson(body);  

        try
        {
            CloudToDeviceMethodResult result = await serviceClient.InvokeDeviceMethodAsync("Car1", methodInvocation);
            if (result.Status == 200)
            {
                return new OkObjectResult(new { success = true, deviceStatus = result.Status });
            }

            _logger.LogWarning("Device failed to execute CancelSchedule with status {Status}", result.Status);
            return new ObjectResult(new { success = false, deviceStatus = result.Status }) 
            {
                StatusCode = 502 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reach device to execute CancelSchedule");
            return new ObjectResult(new { success = false, error = "Could not reach the Car1" }) 
            { 
                StatusCode = 503 
            };
        }
    }
}
