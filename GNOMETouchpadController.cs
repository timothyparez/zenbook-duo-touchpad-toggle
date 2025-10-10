namespace ZenbookDuoTouchPadToggle;

[CommandLineMethodValue("gnome")]
public class GNOMETouchpadController : ITouchpadController
{
    private static string GNOME_GSETTINGS = "gsettings";
    private static string GNOME_GET_TOUCHPAD_STATE_ARGS = "get org.gnome.desktop.peripherals.touchpad send-events";
    private static string GNOME_SET_TOUCHPAD_STATE_ARGS = "set org.gnome.desktop.peripherals.touchpad send-events";

    public bool TryToggleTouchpadState()
    {
        try
        {
            var currentTouchpadState = ProcessHelper.CreateProcessAndReadOutput(GNOME_GSETTINGS, GNOME_GET_TOUCHPAD_STATE_ARGS);

            var targetState = currentTouchpadState.Trim() switch
            {
                "'enabled'" => "disabled",
                "'disabled'" => "enabled",
                var x => throw new Exception($"Failed to get the touchpad state: {x}")
            };

            var toggleTouchpadStateResult = ProcessHelper.CreateProcessAndReadOutput(GNOME_GSETTINGS, $"{GNOME_SET_TOUCHPAD_STATE_ARGS} {targetState}");
            if (!String.IsNullOrEmpty(toggleTouchpadStateResult.Trim()))
            {
                throw new Exception("Failed to toggle the touchpad state");
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Error.WriteLine($"Failed to set toggle the touchpad state: {ex.Message}");
            return false;
        }
    }
}