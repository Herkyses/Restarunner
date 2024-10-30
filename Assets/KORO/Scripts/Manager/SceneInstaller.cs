
using Zenject;

public class SceneInstaller : MonoInstaller
{
    public CheckOrderBillsPanel orderBillPanel;
    public PlacePanelController shoppingManagerPanel;

    public override void InstallBindings()
    {
        
        Container.Bind<CheckOrderBillsPanel>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PanelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<OrderPanelController>().FromComponentInHierarchy().AsSingle();

        //Container.BindInstance(orderBillPanel).AsSingle();
        //Container.BindInstance(shoppingManagerPanel).AsSingle();
    }
}
