using ComicShop.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ComicShop.ViewModels
{
    public class ActOfSalePageViewModel : ViewModelBase
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
       

        private ObservableCollection<ActOfSale> _actOfSales = new ObservableCollection<ActOfSale>();
        private ObservableCollection<Comic> _comics = new ObservableCollection<Comic>();
        private ObservableCollection<ComicStorage> _comicsInAct = new ObservableCollection<ComicStorage>();

         

        private ComicStorage _selectedComicStorageListBox;
        private Comic _selectedComicComboBox;

        private bool _visibleListMode;
        private bool _visibleEditMode;

        private int _amountComic;

        private string _searchText;
        private ActOfSale _actOfSale;


        public ObservableCollection<ActOfSale> ActOfSales
        {
            get => _actOfSales;
            set => this.RaiseAndSetIfChanged(ref _actOfSales, value);
        }


        public ObservableCollection<Comic> Comics
        {            
            get => _comics;
            set => this.RaiseAndSetIfChanged(ref _comics, value);
        }

        public ObservableCollection<ComicStorage> ComicsInAct
        {
            get => _comicsInAct;
            set => this.RaiseAndSetIfChanged(ref _comicsInAct, value);
        }

        public ActOfSale SelectedActOfSale
        {
            get => _actOfSale;
            set => this.RaiseAndSetIfChanged(ref _actOfSale, value);
        }

        public Comic SelectedComicComboBox
        {
            get => _selectedComicComboBox;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedComicComboBox, value);
            }
        }

        public ComicStorage SelectedComicStorageListBox
        {
            get => _selectedComicStorageListBox;
            set 
            { 
                this.RaiseAndSetIfChanged(ref _selectedComicStorageListBox, value);
                if(_selectedComicStorageListBox != null)
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
        public ActOfSalePageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            VisibleListMode = true;
            VisibleEditMode = false;

            ActOfSales = new(db.ActOfSales.ToList());
            Comics = new(db.Comics.ToList());

            Comics = new ObservableCollection<Comic>
            {
                new Comic
                {
                    Title = "123",
                    Price = 123
                },
                new Comic
                {
                    Title = "312",
                    Price = 123
                },
                new Comic
                {
                    Title = "345",
                    Price = 123
                }
            };


            Add = ReactiveCommand.Create(() =>
            {
                SelectedComicComboBox = null;
                SelectedComicStorageListBox = null;
                ComicsInAct = new ObservableCollection<ComicStorage>();
                AmountComic = 1;
               
                VisibleListMode = false;
                VisibleEditMode = true;
            });


            IncrementationComicAmount = ReactiveCommand.Create(() =>
            {
                AmountComic++;
                if(SelectedComicStorageListBox != null)
                    SelectedComicStorageListBox.Amount++;
            });

            DecrementationComicAmount = ReactiveCommand.Create(() =>
            {
                if(AmountComic > 1)
                {
                    AmountComic--;
                    if (SelectedComicStorageListBox != null)
                        SelectedComicStorageListBox.Amount--;
                }
            });


            AddComic = ReactiveCommand.Create(() =>
            {
                if(SelectedComicComboBox != null)
                    ComicsInAct.Add(new ComicStorage() { Amount = AmountComic, Comic = SelectedComicComboBox});
            });


            RemoveComic = ReactiveCommand.Create(() =>
            {
                if (SelectedComicStorageListBox != null)
                    ComicsInAct.Remove(SelectedComicStorageListBox);
            });


            SaveActOfSale = ReactiveCommand.Create(() =>
            {
                if(ComicsInAct.Count > 0)
                {
                    int sum = 0;
                    foreach (var i in ComicsInAct)
                        sum += (i.Amount * i.Comic.Price);
                    
                    ActOfSales.Add(new ActOfSale { Comics = ComicsInAct, Date = DateTime.Now, Sum = sum });
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
                ActOfSales.Remove(SelectedActOfSale);
            });

            Search = ReactiveCommand.Create(() =>
            {
                
                if (SearchText == null)
                {
                    ActOfSales = new(db.ActOfSales.Include(x => x.Comics).ToList());
                    return;
                }
                ActOfSales = new(db.ActOfSales
                    .Include(x => x.Comics)
                    .Where(x => x.Date.ToString()
                    .Contains(SearchText)).ToList());
               
            });
        }
    }
}
