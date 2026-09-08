using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Devices;

namespace api;

public class SetSchedule
{
    private readonly ILogger<SetSchedule> _logger;

    public SetSchedule(ILogger<SetSchedule> logger)
    {
        _logger = logger;
    }

    [Function("SetSchedule")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "schedule/set")] HttpRequest req)
    {
        string serviceConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!;
        using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);

        var methodInvocation = new CloudToDeviceMethod("SetSchedule")
        {  
            ResponseTimeout = TimeSpan.FromSeconds(10)
        };

        string body = await new StreamReader(req.Body).ReadToEndAsync();

        methodInvocation.SetPayloadJson(body);  

        CloudToDeviceMethodResult result = await serviceClient.InvokeDeviceMethodAsync("Car1", methodInvocation);

        return new OkObjectResult($"Command sent. Device responded with status: {result.Status}");
    }
}
