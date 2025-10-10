
var assembly = Assembly.GetExecutingAssembly();

var touchpadControllersInfo = assembly.GetTypes()
                                      .Select(t => new { T = t, A = t.GetCustomAttribute<CommandLineMethodValueAttribute>() })
                                      .Where(x => x.A != null && !String.IsNullOrWhiteSpace(x.A.Name));
                                      
var method = new Option<string>("--method")
{
    Description = "The method to use to try and toggle the touchpad state",
    Required = false,
    DefaultValueFactory = (x) => "xinput",
};

method.Validators.Add(x =>
{
    var methodName = x.GetValue(method);
    
    var supportedMethods = touchpadControllersInfo.Select(info => info!.A!.Name).ToList();
    if (!supportedMethods.Contains(methodName!))
    {
        x.AddError($"The method '{methodName}' is not supported");
    }
});


var rootCommand = new RootCommand("Application to enable/disable the touchpad on an Asus Zenbook Duo");
rootCommand.Options.Add(method);

var parseResult = rootCommand.Parse(args);
if (parseResult.Errors.Count == 0 && !String.IsNullOrWhiteSpace(parseResult.GetValue(method)))
{
    var methodName = parseResult.GetValue(method);
    var methodInfo = touchpadControllersInfo.FirstOrDefault(x => x.A?.Name == methodName);
    if (methodInfo != null)
    {
        var touchController = Activator.CreateInstance(methodInfo.T) as ITouchpadController;
        touchController?.TryToggleTouchpadState();
    }
}
else
{
    parseResult.Errors.ToList().ForEach(e => Console.WriteLine(e));
}
