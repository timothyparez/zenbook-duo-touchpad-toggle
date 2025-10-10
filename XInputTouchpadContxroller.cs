public class XInputTouchpadContxroller : ITouchpadController
{
    public bool TryToggleTouchpadState()
    {
        if (TryGetTouchpadDeviceId(out var deviceId))
        {
            if (TryGetTouchpadState(deviceId, out var currentTouchpadState))
            {
                return TrySetTouchpadState(deviceId, !currentTouchpadState);
            }
        }
        return false;
    }

    static bool TryGetTouchpadDeviceId(out int id)
    {
        try
        {
            var deviceIdOutput = ProcessHelper.CreateProcessAndReadOutput("xinput", "list");

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
            WriteLine(ex.Message);
            id = -1;
            return false;
        }
    }

    static bool TryGetTouchpadState(int deviceId, out bool enabled)
    {
        try
        {
            const string ENABLED = "enabled";
            var deviceStateOutput = ProcessHelper.CreateProcessAndReadOutput("xinput", $"list-props {deviceId}");
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
            Error.Write(ex.Message);
            enabled = false;
            return false;
        }
    }

    static bool TrySetTouchpadState(int deviceId, bool enabled)
    {
        try
        {
            ProcessHelper.CreateProcessAndReadOutput("xinput", $"{(enabled ? "enable" : "disable")} {deviceId}");
            if (TryGetTouchpadState(deviceId, out var newState))
            {
                if (newState == enabled)
                {
                    WriteLine($"The touchpad has been turned {(enabled ? "on" : "off")}");
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
            Error.WriteLine(ex.Message);
            return false;
        }
    }
}