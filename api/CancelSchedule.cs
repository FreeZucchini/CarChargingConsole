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
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "schedule/cancel")] HttpRequest req)
    {
        string serviceConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!;
        using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);

        var methodInvocation = new CloudToDeviceMethod("CancelSchedule")
        {  
            ResponseTimeout = TimeSpan.FromSeconds(10)
        };

        string body = await new StreamReader(req.Body).ReadToEndAsync();

        methodInvocation.SetPayloadJson(body);  

        CloudToDeviceMethodResult result = await serviceClient.InvokeDeviceMethodAsync("Car1", methodInvocation);

        return new OkObjectResult($"Command sent. Device responded with status: {result.Status}");
    }
}
