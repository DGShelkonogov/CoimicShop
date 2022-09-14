using ComicShop.Models;
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
    public class PositionPageViewModel : ViewModelBase
    {
        public ICommand Add { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Search { get; private set; }

        private ObservableCollection<Position> _positions = new ObservableCollection<Position>();
        public ObservableCollection<Position> Positions
        {
            get => _positions;
            set => this.RaiseAndSetIfChanged(ref _positions, value);
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private Position _positionSelected;
        public Position PositionSelected
        {
            get => _positionSelected;
            set => this.RaiseAndSetIfChanged(ref _positionSelected, value);
        }

        ApplicationContext db;
        public PositionPageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            Positions = new(db.Positions.ToList());
            PositionSelected = new Position();
            Add = ReactiveCommand.Create(() =>
            {   
                try
                {
                    if (ApplicationContext.validData(PositionSelected))
                    {
                        var position = new Position
                        {
                            Salary = PositionSelected.Salary,
                            Title = PositionSelected.Title
                        };
                        Positions.Add(position);
                        db.Positions.Add(position);
                        db.SaveChanges();
                        PositionSelected = new Position();
                    }
                }
                catch (Exception ex) { }
            });
            Edit = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(PositionSelected))
                    {
                        db.Positions.Update(PositionSelected);
                        db.SaveChanges();
                        PositionSelected = new Position();
                    }
                }
                catch (Exception ex) { }
            });
            Remove = ReactiveCommand.Create(() =>
            {

                if (PositionSelected != null)
                {
                    db.Positions.Remove(PositionSelected);
                    Positions.Remove(PositionSelected);
                    db.SaveChanges();
                    PositionSelected = new Position();
                }
            });
            Search = ReactiveCommand.Create(() =>
            {

                if (SearchText == null)
                {
                    Positions = new(db.Positions
                    .ToList());
                    return;
                }
                Positions = new(db.Positions
                .Where(x => x.Title.Contains(SearchText))
                .ToList());
            });
        }

    }
}
