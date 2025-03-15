using Scripts;
using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Input>().ToSelf().FromComponentsInHierarchy().AsSingle().NonLazy();
    }
}