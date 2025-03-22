using Scripts;
using Zenject;

public class UIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ScreenFade>().ToSelf().FromComponentsInHierarchy().AsSingle().NonLazy();
        Container.Bind<UIMenu>().ToSelf().FromComponentsInHierarchy().AsSingle().NonLazy();
    }
}