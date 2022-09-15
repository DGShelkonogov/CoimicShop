﻿using ComicShop.Models;
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
    public class StoragePageViewModel : ViewModelBase
    {

        public ICommand Add { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Search { get; private set; }

        private ObservableCollection<Storage> _storages = new ObservableCollection<Storage>();
        public ObservableCollection<Storage> Storages
        {
            get => _storages;
            set => this.RaiseAndSetIfChanged(ref _storages, value);
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private Storage _storageSelected;
        public Storage StorageSelected
        {
            get => _storageSelected;
            set => this.RaiseAndSetIfChanged(ref _storageSelected, value);
        }
       
        private ObservableCollection<Provider> _providersAll = new ObservableCollection<Provider>();
        public ObservableCollection<Provider> ProvidersAll
        {
            get => _providersAll;
            set => this.RaiseAndSetIfChanged(ref _providersAll, value);
        }
        private ComicStorage _comicstorageSelected;
        public ComicStorage ComicStorageSelected
        {
            get => _comicstorageSelected;
            set => this.RaiseAndSetIfChanged(ref _comicstorageSelected, value);
        }
        private ObservableCollection<ComicStorage> _comicstoragesAll = new ObservableCollection<ComicStorage>();
        public ObservableCollection<ComicStorage> ComicStoragesAll
        {
            get => _comicstoragesAll;
            set => this.RaiseAndSetIfChanged(ref _comicstoragesAll, value);
        }
        ApplicationContext db;
        public StoragePageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            Storages = new(db.Storages.ToList());
            StorageSelected = new Storage();
            ProvidersAll = new(db.Providers.ToList());
            ComicStoragesAll = new(db.ComicStorages.ToList());
            Add = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(StorageSelected))
                    {
                        if (ApplicationContext.validData(StorageSelected.Provider))
                        {
                            if (ApplicationContext.validData(StorageSelected.Сomics))
                            {
                                var obj = new Storage
                                {
                                    Title = StorageSelected.Title,
                                    Provider = StorageSelected.Provider,
                                    Сomics = StorageSelected.Сomics,
                                };
                                Storages.Add(obj);
                                db.Storages.Add(obj);
                                db.SaveChanges();
                                StorageSelected = new Storage();
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Edit = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(StorageSelected))
                    {
                        if (ApplicationContext.validData(StorageSelected.Provider))
                        {
                            if (ApplicationContext.validData(StorageSelected.Сomics))
                            {
                                db.Storages.Update(StorageSelected);
                                db.SaveChanges();
                                StorageSelected = new Storage();
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Remove = ReactiveCommand.Create(() =>
            {

                if (StorageSelected != null)
                {
                    db.Storages.Remove(StorageSelected);
                    Storages.Remove(StorageSelected);
                    db.SaveChanges();
                    StorageSelected = new Storage();
                }
            });
            Search = ReactiveCommand.Create(() =>
            {

                if (SearchText == null)
                {
                    Storages = new(db.Storages
                    .Include(x => x.Provider)
                    .Include(x => x.Сomics)
                    .ToList());
                    return;
                }
                Storages = new(db.Storages
                .Include(x => x.Provider)
                .Include(x => x.Сomics)
                .Where(x => x.Title.Contains(SearchText))
                .ToList());
            });
        }


    }
}
