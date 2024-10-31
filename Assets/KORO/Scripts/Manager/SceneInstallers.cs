
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

        Container.Bind<PanelManager>().FromComponentInHierarchy().AsSingle();
        //Container.BindInstance(shoppingManagerPanel).AsSingle();
    }
}
