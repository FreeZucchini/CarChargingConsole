// See https://aka.ms/new-console-template for more information
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

string deviceConnectionString = "HostName=ChargingHub.azure-devices.net;DeviceId=Car1;SharedAccessKey=vjvm9hnVJNNGRosknxjFPXs0gCEIqdNhmRuTTFsDjp8=";

// A device is created in the IoT Hub on Azure web and thats where we get the the primary connection string from
DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);

bool isCharging = false;
double batteryPercentage = 100.0;


// We need to use SetMethodHandlerAsync to process immediate requests from the IoT hub and send back responses
await deviceClient.SetMethodHandlerAsync("StartCharging", (request, context) =>
{
    isCharging = true;
    Console.WriteLine("StartCharging");
    return Task.FromResult(new MethodResponse(200));
}, null);

await deviceClient.SetMethodHandlerAsync("StopCharging", (request, context) =>
{
    isCharging = false;
    Console.WriteLine("StopCharging");
    return Task.FromResult(new MethodResponse(200));
}, null);

await deviceClient.SetMethodHandlerAsync("GetBatteryLevel", (request, context) =>
{
    var level = new
    {
        batteryPercentage = Math.Round(batteryPercentage, 1),
        isCharging
    };

    string json = JsonConvert.SerializeObject(level);
    var response = new MethodResponse(Encoding.UTF8.GetBytes(json), 200);
    return Task.FromResult(response);
}, null);

await deviceClient.SetMethodHandlerAsync("SetSchedule", (request, context) =>
{
    string time = Encoding.UTF8.GetString(request.Data);

    Console.WriteLine($"Recieved start charging time: {time}");

    var response = new MethodResponse(Encoding.UTF8.GetBytes("OK"), 200);
    return Task.FromResult(response);
}, null);

await deviceClient.SetMethodHandlerAsync("CancelSchedule", (request, context) =>
{
    string time = Encoding.UTF8.GetString(request.Data);

    Console.WriteLine($"Cancelled charging schedule");

    var response = new MethodResponse(Encoding.UTF8.GetBytes("OK"), 200);
    return Task.FromResult(response);
}, null);

Console.WriteLine("Simulator running");

while (true) 
{
    if (isCharging && batteryPercentage < 100) 
    {
        batteryPercentage = Math.Min(100, batteryPercentage + 2);
    } 
    else if (!isCharging && batteryPercentage > 0)
    {
        batteryPercentage = Math.Max(0, batteryPercentage - 1);
    }

    var telemetry = new
    {
        deviceId = "Car1",
        batteryPercentage = Math.Round(batteryPercentage, 2),
        isCharging,
        timestamp = DateTime.UtcNow
    };

    string json = JsonConvert.SerializeObject(telemetry);
    var message = new Message(Encoding.UTF8.GetBytes(json))
    {
        ContentType = "application/json",
        ContentEncoding = "utf-8"
    };

    await deviceClient.SendEventAsync(message);
    Console.WriteLine($"Sent: {json}");

    await Task.Delay(5000);
}