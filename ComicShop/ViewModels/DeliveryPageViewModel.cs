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
    public class DeliveryPageViewModel : ViewModelBase
    {
        public ICommand Add { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Search { get; private set; }

        public ICommand IncrementationComicAmount { get; private set; }
        public ICommand DecrementationComicAmount { get; private set; }
        public ICommand AddComic { get; private set; }
        public ICommand RemoveComic { get; private set; }
        public ICommand SaveActOfSale { get; private set; }
        public ICommand Back { get; private set; }


        private ObservableCollection<Delivery> _deliverys = new ObservableCollection<Delivery>();
        private ObservableCollection<Comic> _comics = new ObservableCollection<Comic>();
        private ObservableCollection<Storage> _storages = new ObservableCollection<Storage>();

        private ObservableCollection<ComicStorage> _comicsStorage = new ObservableCollection<ComicStorage>();



        private ComicStorage _selectedComicStorageListBox;
        private Comic _selectedComicComboBox;
        private Storage _selectedStorageComboBox;


        private bool _visibleListMode;
        private bool _visibleEditMode;
        private bool _storagesIsEnabled;

        private int _amountComic;

        private string _searchText;
        private Delivery _selectedDelivery;


        public ObservableCollection<Delivery> Deliverys
        {
            get => _deliverys;
            set => this.RaiseAndSetIfChanged(ref _deliverys, value);
        }
        
     
        public ObservableCollection<Storage> Storages
        {
            get => _storages;
            set => this.RaiseAndSetIfChanged(ref _storages, value);
        }


        public ObservableCollection<Comic> Comics
        {
            get => _comics;
            set => this.RaiseAndSetIfChanged(ref _comics, value);
        }

        public ObservableCollection<ComicStorage> ComicsStorage
        {
            get => _comicsStorage;
            set => this.RaiseAndSetIfChanged(ref _comicsStorage, value);
        }

        public Delivery SelectedDelivery
        {
            get => _selectedDelivery;
            set => this.RaiseAndSetIfChanged(ref _selectedDelivery, value);
        }
         
     

        public Comic SelectedComicComboBox
        {
            get => _selectedComicComboBox;
            set => this.RaiseAndSetIfChanged(ref _selectedComicComboBox, value);
        }

        public Storage SelectedStorageComboBox
        {
            get => _selectedStorageComboBox;
            set => this.RaiseAndSetIfChanged(ref _selectedStorageComboBox, value);
        }


        public ComicStorage SelectedComicStorageListBox
        {
            get => _selectedComicStorageListBox;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedComicStorageListBox, value);
                if (_selectedComicStorageListBox != null)
                {
                    AmountComic = _selectedComicStorageListBox.Amount;
                    SelectedComicComboBox = _selectedComicStorageListBox.Comic;
                }
            }
        }

        public bool VisibleListMode
        {
            get => _visibleListMode;
            set => this.RaiseAndSetIfChanged(ref _visibleListMode, value);
        }

        public bool VisibleEditMode
        {
            get => _visibleEditMode;
            set => this.RaiseAndSetIfChanged(ref _visibleEditMode, value);
        }
        
        public bool StoragesIsEnabled
        {
            get => _storagesIsEnabled;
            set => this.RaiseAndSetIfChanged(ref _storagesIsEnabled, value);
        }

        public int AmountComic
        {
            get => _amountComic;
            set => this.RaiseAndSetIfChanged(ref _amountComic, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }



        ApplicationContext db;
        public DeliveryPageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            VisibleListMode = true;
            VisibleEditMode = false;

            StoragesIsEnabled = true;

            Deliverys = new(db.Deliverys
                .Include(x => x.Сomics)
                .Include(x => x.Storage)
                .ToList());
            Comics = new(db.Comics.ToList());
            Storages = new(db.Storages
                .Include(x => x.Provider)
                .ThenInclude(x => x.Сomics)
                .ToList());
            

            Add = ReactiveCommand.Create(() =>
            {
                SelectedComicComboBox = null;
                SelectedComicStorageListBox = null;
                SelectedStorageComboBox = null;
                ComicsStorage = new ObservableCollection<ComicStorage>();
                AmountComic = 1;

                VisibleListMode = false;
                VisibleEditMode = true;
                StoragesIsEnabled = true;
            });


            IncrementationComicAmount = ReactiveCommand.Create(() =>
            {
                AmountComic++;
                if (SelectedComicStorageListBox != null)
                    SelectedComicStorageListBox.Amount++;
            });

            DecrementationComicAmount = ReactiveCommand.Create(() =>
            {
                if (AmountComic > 1)
                {
                    AmountComic--;
                    if (SelectedComicStorageListBox != null)
                        SelectedComicStorageListBox.Amount--;
                }
            });


            AddComic = ReactiveCommand.Create(() =>
            {
                if (SelectedComicComboBox != null)
                {
                    ComicsStorage.Add(new ComicStorage() { Amount = AmountComic, Comic = SelectedComicComboBox });
                    StoragesIsEnabled = false;
                }
            });


            RemoveComic = ReactiveCommand.Create(() =>
            {
                if (SelectedComicStorageListBox != null)
                    ComicsStorage.Remove(SelectedComicStorageListBox);
                if(ComicsStorage.Count == 0)
                    StoragesIsEnabled = true;
            });


            SaveActOfSale = ReactiveCommand.Create(() =>
            {
                if (ComicsStorage.Count > 0)
                {
                    var obj = new Delivery()
                    {
                        Сomics = ComicsStorage,
                        Storage = SelectedStorageComboBox
                    };

                    foreach(var item in ComicsStorage)
                    {
                        var comicStorage = SelectedStorageComboBox.Сomics.FirstOrDefault(x => x.Comic == item.Comic);

                        if (comicStorage == null)
                        {
                            SelectedStorageComboBox.Сomics.Add(item);
                        }
                        else
                        {
                            comicStorage.Amount += item.Amount;
                        }   
                    }

                    Deliverys.Add(obj);
                    db.Deliverys.Add(obj);
                    db.SaveChanges();

                    VisibleListMode = true;
                    VisibleEditMode = false;
                }
            });


            Back = ReactiveCommand.Create(() =>
            {
                VisibleListMode = true;
                VisibleEditMode = false;
            });

            Remove = ReactiveCommand.Create(() =>
            {
                db.Deliverys.Remove(SelectedDelivery);
                Deliverys.Remove(SelectedDelivery);
                db.SaveChanges();
            });

            Search = ReactiveCommand.Create(() =>
            {

                if (SearchText == null)
                {
                    Deliverys = new(db.Deliverys
                        .Include(x => x.Storage)
                        .Include(x => x.Сomics)
                        .ToList());
                    return;
                }
                Deliverys = new(db.Deliverys
                    .Include(x => x.Storage)
                    .Include(x => x.Сomics)
                    .Where(x => x.Storage.Title.ToString()
                    .Contains(SearchText)).ToList());
            });
        }
    }
}
