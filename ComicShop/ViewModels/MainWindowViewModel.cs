using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static ComicShop.ViewModels.ActOfSalePageViewModel;

namespace ComicShop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public ActOfSalePageViewModel ActOfSalePageViewModel { get; }
        public ComicPageViewModel ComicPageViewModel { get; }
        public DeliveryPageViewModel DeliveryPageViewModel { get; }
        public EmployeePageViewModel EmployeePageViewModel { get; }
        public PayoutPageViewModel PayoutPageViewModel { get; }
        public PositionPageViewModel PositionPageViewModel { get; }
        public ProviderPageViewModel ProviderPageViewModel { get; }
        public StoragePageViewModel StoragePageViewModel { get; }


        public MainWindowViewModel()
        {
            var connection = DBConnection.getConnection();
         
            ActOfSalePageViewModel = new ActOfSalePageViewModel(connection);
            ComicPageViewModel = new ComicPageViewModel(connection);
            DeliveryPageViewModel = new DeliveryPageViewModel(connection);
            EmployeePageViewModel = new EmployeePageViewModel(connection);
            PayoutPageViewModel = new PayoutPageViewModel(connection); // ?????????
            PositionPageViewModel = new PositionPageViewModel(connection);
            ProviderPageViewModel = new ProviderPageViewModel(connection);
            StoragePageViewModel = new StoragePageViewModel(connection);
           

        }

        public string Greeting => "Welcome to Avalonia!";

      
    }
}
