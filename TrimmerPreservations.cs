namespace ZenbookDuoTouchPadToggle;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public class TrimmerPreservations
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ITouchpadController))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TouchpadStateManager<>))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(GNOMETouchpadController))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(XInputTouchpadController))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ProcessHelper))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CommandLineMethodValueAttribute))]

    [ModuleInitializer]
    public static void InitializeModule()
    {
        /* This code is executed before anything else
         * and in this case it prevents trimming of the 
         * types specified above when we publish using trimming/aot 
         *
         * If you add new ITouchController implementations, make sure
         * you add them to the attribute list above
         */
    }
}