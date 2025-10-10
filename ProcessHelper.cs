namespace ZenbookDuoTouchPadToggle;
public static class ProcessHelper
{
    public static string CreateProcessAndReadOutput(string filename, string arguments)
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
}