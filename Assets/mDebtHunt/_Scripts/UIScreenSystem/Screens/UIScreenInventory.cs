using Zenject;

public class UIScreenInventory : UIScreen
{
    [Inject] private readonly InputSystem _inputSystem;

    protected override void OnOpen()
    {
        _inputSystem.DisablePlayerMap();
        ShowCursor();
    }

    protected override void OnClose()
    {
        _inputSystem.EnablePlayerMap();
        HideCursor();
    }
}