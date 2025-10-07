using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;


if (TryGetTouchpadDeviceId(out var deviceId))
{
    if (TryGetDeviceState(deviceId, out var enabled))
    {
        if (TrySetDeviceState(deviceId, !enabled))
        {
            Console.WriteLine($"The touch pad state has been toggled to: {(!enabled ? "enabled" : "disabled")}");
        }
        else
        {
            Console.Error.WriteLine("Failed to set the touchpad state");
        }
    }
    else
    {
        Console.Error.WriteLine("Failed to set the get the touchpad state");
    }
}
else
{
    Console.Error.WriteLine("Failed to toggle the device");
}

static string CreateProcessAndReadOutput(string filename, string arguments)
{
    var processStartInfo = new ProcessStartInfo(filename, arguments)
    {
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true
    };

    using var process = new Process() { StartInfo = processStartInfo };
    process.Start();

    using var reader = process.StandardOutput;

    var output = reader.ReadToEnd();
    process.WaitForExit();

    return output;
}

static bool TryGetTouchpadDeviceId(out int id)
{
    try
    {
        var deviceIdOutput = CreateProcessAndReadOutput("xinput", "list");

        const string ID = "id";

        var deviceIdRegex = new Regex(@"Primax Electronics Ltd. ASUS Zenbook Duo Keyboard Touchpad.*id=(?<id>\d*)");

        var deviceIdMatch = deviceIdRegex.Match(deviceIdOutput);
        if (deviceIdMatch.Success)
        {
            if (deviceIdMatch.Groups.Keys.Contains(ID))
            {
                return Int32.TryParse(deviceIdMatch.Groups[ID].Value, out id);
            }
            else
            {
                throw new Exception("Failed to parse the device id");
            }
        }
        else
        {
            throw new Exception("Failed to find the device id");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        id = -1;
        return false;
    }
}

static bool TryGetDeviceState(int deviceId, out bool enabled)
{
    try
    {
        const string ENABLED = "enabled";
        var deviceStateOutput = CreateProcessAndReadOutput("xinput", $"list-props {deviceId}");
        var deviceStateRegex = new Regex(@"Device Enabled\s*\(\d*\):\s*(?<enabled>\d{1})");
        var deviceStateMatch = deviceStateRegex.Match(deviceStateOutput);
        if (deviceStateMatch.Success)
        {
            if (deviceStateMatch.Groups.Keys.Contains(ENABLED))
            {
                enabled = deviceStateMatch.Groups[ENABLED].Value switch
                {
                    "1" => true,
                    "0" => false,
                    _ => throw new Exception("Unable to parse the device state")
                };

                return true;
            }
            else
            {
                throw new Exception("Unable to parse the device status");
            }
        }
        else
        {
            throw new Exception("Unable to find the device status");
        }

    }
    catch (Exception ex)
    {
        Console.Error.Write(ex.Message);
        enabled = false;
        return false;
    }
}

static bool TrySetDeviceState(int deviceId, bool enabled)
{
    try
    {
        CreateProcessAndReadOutput("xinput", $"{(enabled ? "enable" : "disable")} {deviceId}");
        if (TryGetDeviceState(deviceId, out var newState))
        {
            if (newState == enabled)
            {
                Console.WriteLine($"The touchpad has been turned {(enabled ? "on" : "off")}");
                return true;
            }
            else
            {
                throw new Exception("Failed to toggle the touchpad state");
            }
        }
        else
        {
            throw new Exception("Failed to get the touchpad state");
        }
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        return false;
    }
}