using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.Net;
using System.Text;

namespace api;

public class GetStatus
{
    private readonly ILogger<GetStatus> _logger;

    public GetStatus(ILogger<GetStatus> logger)
    {
        _logger = logger;
    }

    [Function("GetStatus")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "status")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");

        string serviceConnectionString = Environment.GetEnvironmentVariable("IoTHubServiceConnectionString")!;
        using ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(serviceConnectionString);

        var methodInvocation = new CloudToDeviceMethod("GetBatteryLevel")
        {
            ResponseTimeout = TimeSpan.FromSeconds(10)
        };

        try
        {
            CloudToDeviceMethodResult result = await serviceClient.InvokeDeviceMethodAsync("Car1", methodInvocation);
            string payloadJson = result.GetPayloadAsJson() ?? "{}";

            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync(payloadJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reach simulator for status");
            response = req.CreateResponse(HttpStatusCode.ServiceUnavailable);
            await response.WriteStringAsync("Could not reach the car");
        }

        return response;
    }
}
