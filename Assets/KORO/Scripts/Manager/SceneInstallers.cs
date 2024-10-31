
using Zenject;

public class SceneInstallers : MonoInstaller
{
    public CheckOrderBillsPanel orderBillPanel;
    public PlacePanelController shoppingManagerPanel;

    public override void InstallBindings()
    {
        Container.Bind<CheckOrderBillsPanel>().FromComponentInHierarchy().AsSingle();
        Container.Bind<OrderPanelController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ShopManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TableController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<TableAvailablePanel>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlacePanelController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlaceController>().FromComponentInHierarchy().AsSingle();
    
        //Container.BindInstance(shoppingManagerPanel).AsSingle();
    }
}
