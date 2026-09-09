// See https://aka.ms/new-console-template for more information
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

string deviceConnectionString = "HostName=ChargingHub.azure-devices.net;DeviceId=Car1;SharedAccessKey=vjvm9hnVJNNGRosknxjFPXs0gCEIqdNhmRuTTFsDjp8=";

// A device is created in the IoT Hub on Azure web and thats where we get the the primary connection string from
DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);

bool isCharging = false;
double batteryPercentage = 100.0;
string startChargeTimeString = "";
TimeOnly startChargeTime = new TimeOnly(0, 0, 0);
bool scheduleSet = false; 

void StartCharging()
{
    isCharging = true;
    Console.WriteLine("StartCharging");
}

void StopCharging()
{
    isCharging = false;
    Console.WriteLine("StopCharging");
}

// We need to use SetMethodHandlerAsync to process immediate requests from the IoT hub and send back responses
await deviceClient.SetMethodHandlerAsync("StartCharging", (request, context) =>
{
    StartCharging();
    return Task.FromResult(new MethodResponse(200));
}, null);

await deviceClient.SetMethodHandlerAsync("StopCharging", (request, context) =>
{
    StopCharging();
    return Task.FromResult(new MethodResponse(200));
}, null);

await deviceClient.SetMethodHandlerAsync("GetBatteryLevel", (request, context) =>
{
    var level = new
    {
        batteryPercentage = Math.Round(batteryPercentage, 1),
        isCharging,
        scheduleSet,
        scheduleTime = startChargeTimeString
    };

    string json = JsonConvert.SerializeObject(level);
    Console.WriteLine($"Sent: {json}");
    var response = new MethodResponse(Encoding.UTF8.GetBytes(json), 200);
    return Task.FromResult(response);
}, null);

await deviceClient.SetMethodHandlerAsync("SetSchedule", (request, context) =>
{
    startChargeTimeString = Encoding.UTF8.GetString(request.Data);

    Schedule schedule = JsonConvert.DeserializeObject<Schedule>(startChargeTimeString);

    startChargeTime = TimeOnly.Parse(schedule.Time);
    scheduleSet = true;

    Console.WriteLine($"Recieved start charging time: {startChargeTimeString}");

    var response = new MethodResponse(Encoding.UTF8.GetBytes("OK"), 200);
    return Task.FromResult(response);
}, null);

await deviceClient.SetMethodHandlerAsync("CancelSchedule", (request, context) =>
{
    string data = Encoding.UTF8.GetString(request.Data);

    scheduleSet = false;

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

    TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
    if (scheduleSet &&
        currentTime.Hour == startChargeTime.Hour &&
        currentTime.Minute == startChargeTime.Minute)
    {
        StartCharging();
        scheduleSet = false;
    }

    await Task.Delay(3000);
}

public class Schedule
{
    public string Time { get; set; } = "";
}