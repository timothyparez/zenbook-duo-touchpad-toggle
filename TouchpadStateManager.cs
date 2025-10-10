public class TouchpadStateManager<T> where T : ITouchpadController, new()
{
    public bool ToggleTouchpadState() => new T().TryToggleTouchpadState();
}
