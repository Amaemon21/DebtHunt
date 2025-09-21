using Zenject;

public class GameplayProvider : BaseProvider, IInitializable
{
    private readonly GameplayContainer _container;

    public GameplayProvider(GameplayContainer container)
    {
        _container = container;
    }

    public void Initialize()
    {
        Add<CameraSystem>(_container);
        Add<InventoryService>(_container);
        Add<HotbarController>(_container);
    }
}