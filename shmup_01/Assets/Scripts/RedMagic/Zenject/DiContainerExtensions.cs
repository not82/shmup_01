using Zenject;

namespace RedMagic.Zenject
{
    public static class DiContainerExtensions
    {
        public static void BindController<TController>(this DiContainer container)
            where TController : new()
        {
            container
                .BindInterfacesAndSelfTo<TController>()
                .AsSingle()
                .NonLazy();
        }
    }
}