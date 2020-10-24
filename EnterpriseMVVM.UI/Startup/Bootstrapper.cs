using Autofac;
using Prism.Events;
using EnterpriseMVVM.DataAccess;
using EnterpriseMVVM.UI.Base;
using EnterpriseMVVM.UI.Data.Lookups;
using EnterpriseMVVM.UI.Data.Repositories;
using EnterpriseMVVM.UI.Service;
using EnterpriseMVVM.UI.ViewModel;

namespace EnterpriseMVVM.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            //Registering Base Classes
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<EnterpriseMVVMDbContext>().AsSelf();

            //Registering the lookups
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            //Registering MainWindow & MianWindowViewModel
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            //Registering Category
            builder.RegisterType<CategoryMainViewModel>().Keyed<IEntityViewModel>(nameof(CategoryMainViewModel));
            builder.RegisterType<CategoryNavigationViewModel>().As<ICategoryNavigationViewModel>();
            builder.RegisterType<CategoryDetailViewModel>().As<ICategoryDetailViewModel>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();

            //Registering Product
            builder.RegisterType<ProductMainViewModel>().Keyed<IEntityViewModel>(nameof(ProductMainViewModel));
            builder.RegisterType<ProductNavigationViewModel>().As<IProductNavigationViewModel>();
            builder.RegisterType<ProductDetailViewModel>().As<IProductDetailViewModel>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();

            return builder.Build();
        }
    }
}
