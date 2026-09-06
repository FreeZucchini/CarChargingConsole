using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;

namespace api;

public class StartCharging
{
    private readonly ILogger<StartCharging> _logger;

    public StartCharging(ILogger<StartCharging> logger)
    {
        _logger = logger;
    }

    [Function("StartCharging")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "charge/start")] HttpRequest req)
    {
        string serviceConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!;
        using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);

        var methodInvocation = new CloudToDeviceMethod("StartCharging")
        {
            ResponseTimeout = TimeSpan.FromSeconds(10)
        };

        CloudToDeviceMethodResult result = await serviceClient.InvokeDeviceMethodAsync("Car1", methodInvocation);

        return new OkObjectResult($"Command sent. Device responded with status: {result.Status}");
    }
}
