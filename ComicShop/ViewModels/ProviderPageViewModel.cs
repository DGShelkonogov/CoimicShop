using ComicShop.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ComicShop.ViewModels
{
    public class ProviderPageViewModel : ViewModelBase
    {
        public ICommand Add { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Search { get; private set; }
        private ObservableCollection<Provider> _providers = new ObservableCollection<Provider>();
        public ObservableCollection<Provider> Providers
        {
            get => _providers;
            set => this.RaiseAndSetIfChanged(ref _providers, value);
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private Provider _providerSelected;
        public Provider ProviderSelected
        {
            get => _providerSelected;
            set => this.RaiseAndSetIfChanged(ref _providerSelected, value);
        }
        private Comic _comicSelected;
        public Comic ComicSelected
        {
            get => _comicSelected;
            set {
                if(value != null)
                {
                    if (ProviderSelected.Сomics.ToList().Exists(x => x == value))
                    {
                        ProviderSelected.Сomics.Remove(value);
                    }
                    else
                    {
                        ProviderSelected.Сomics.Add(value);
                    }

                }
                this.RaiseAndSetIfChanged(ref _comicSelected, value);
            }
        }
        private ObservableCollection<Comic> _comicsAll = new ObservableCollection<Comic>();
        public ObservableCollection<Comic> ComicsAll
        {
            get => _comicsAll;
            set => this.RaiseAndSetIfChanged(ref _comicsAll, value);
        }
        ApplicationContext db;
        public ProviderPageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            Providers = new(db.Providers.ToList());
            ProviderSelected = new Provider();
            ComicsAll = new(db.Comics.ToList());
            Add = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(ProviderSelected))
                    {
                        if (ApplicationContext.validData(ProviderSelected.Сomics))
                        {
                            var obj = new Provider
                            {
                                Name = ProviderSelected.Name,
                                CountryProvider = ProviderSelected.CountryProvider,
                                Сomics = ProviderSelected.Сomics,
                            };
                            Providers.Add(obj);
                            db.Providers.Add(obj);
                            db.SaveChanges();
                            ProviderSelected = new Provider();
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Edit = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(ProviderSelected))
                    {
                        if (ApplicationContext.validData(ProviderSelected.Сomics))
                        {
                            db.Providers.Update(ProviderSelected);
                            db.SaveChanges();
                            ProviderSelected = new Provider();
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Remove = ReactiveCommand.Create(() =>
            {

                if (ProviderSelected != null)
                {
                    db.Providers.Remove(ProviderSelected);
                    Providers.Remove(ProviderSelected);
                    db.SaveChanges();
                    ProviderSelected = new Provider();
                }
            });
            Search = ReactiveCommand.Create(() =>
            {

                if (SearchText == null)
                {
                    Providers = new(db.Providers
                    .Include(x => x.Сomics)
                    .ToList());
                    return;
                }
                Providers = new(db.Providers
                .Include(x => x.Сomics)
                .Where(x => x.Name.Contains(SearchText))
                .ToList());
            });
        }
    }
}
