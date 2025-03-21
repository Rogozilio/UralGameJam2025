using Scripts;
using Zenject;

public class UIFadeInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ScreenFade>().ToSelf().FromComponentsInHierarchy().AsSingle().NonLazy();
    }
}