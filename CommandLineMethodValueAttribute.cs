namespace ZenbookDuoTouchPadToggle;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class CommandLineMethodValueAttribute : Attribute
{
    public string Name { get; set; }

    public CommandLineMethodValueAttribute() => Name = "";

    public CommandLineMethodValueAttribute(string name) => Name = name;
}
