using System.Windows;
using OverallAuthWPF.Views;
using OveralllAuth_V1.ViewModels;
using OveralllAuth_V1.Views;
using Prism.Ioc;

namespace OverallAuthWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HomeUC,HomeUCViewModel>();
            containerRegistry.RegisterForNavigation<UserUC,UserUCViewModel>();
            containerRegistry.RegisterForNavigation<RoleUC,RoleUCViewModel>();
            containerRegistry.RegisterForNavigation<MenuUC,MenuUCViewModel>();
        }
    }
}
