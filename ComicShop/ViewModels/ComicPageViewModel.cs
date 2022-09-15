using ComicShop.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ComicShop.ViewModels
{
    public class ComicPageViewModel : ViewModelBase
    {
        
        public ICommand Add { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Search { get; private set; }
        private ObservableCollection<Comic> _comics = new ObservableCollection<Comic>();
        public ObservableCollection<Comic> Comics
        {
            get => _comics;
            set => this.RaiseAndSetIfChanged(ref _comics, value);
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private Comic _comicSelected;
        public Comic ComicSelected
        {
            get => _comicSelected;
            set => this.RaiseAndSetIfChanged(ref _comicSelected, value);
        }
        ApplicationContext db;
        public ComicPageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            Comics = new(db.Comics.ToList());
            ComicSelected = new Comic();
            Add = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(ComicSelected))
                    {
                        var obj = new Comic
                        {
                            Title = ComicSelected.Title,
                            Description = ComicSelected.Description,
                            Price = ComicSelected.Price,
                        };
                        Comics.Add(obj);
                        db.Comics.Add(obj);
                        db.SaveChanges();
                        ComicSelected = new Comic();
                    }
                }
                catch (Exception ex) { }
            });
            Edit = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(ComicSelected))
                    {
                        db.Comics.Update(ComicSelected);
                        db.SaveChanges();
                        ComicSelected = new Comic();
                    }
                }
                catch (Exception ex) { }
            });
            Remove = ReactiveCommand.Create(() =>
            {

                if (ComicSelected != null)
                {
                    db.Comics.Remove(ComicSelected);
                    Comics.Remove(ComicSelected);
                    db.SaveChanges();
                    ComicSelected = new Comic();
                }
            });
            Search = ReactiveCommand.Create(() =>
            {

                if (SearchText == null)
                {
                    Comics = new(db.Comics
                    .ToList());
                    return;
                }
                Comics = new(db.Comics
                .Where(x => x.Title.Contains(SearchText))
                .ToList());
            });
        }

    }
}
