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
    public class EmployeePageViewModel : ViewModelBase
    {
        public ICommand Add { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Search { get; private set; }

        private ObservableCollection<Employee> _employees = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => this.RaiseAndSetIfChanged(ref _employees, value);
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private Employee _employeeSelected;
        public Employee EmployeeSelected
        {
            get => _employeeSelected;
            set => this.RaiseAndSetIfChanged(ref _employeeSelected, value);
        }

        private DateTimeOffset? _dateOfBirth;
        public DateTimeOffset? DateOfBirth
        {
            get => _dateOfBirth;
            set => this.RaiseAndSetIfChanged(ref _dateOfBirth, value);
        }

        private ObservableCollection<Position> _positionsAll = new ObservableCollection<Position>();
        public ObservableCollection<Position> PositionsAll
        {
            get => _positionsAll;
            set => this.RaiseAndSetIfChanged(ref _positionsAll, value);
        }
        ApplicationContext db;
        public EmployeePageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            Employees = new(db.Employees.ToList());
            EmployeeSelected = new Employee();
            PositionsAll = new(db.Positions.ToList());
            Add = ReactiveCommand.Create(() =>
            {

                try
                {
                    EmployeeSelected.DateOfBirth = ((DateTimeOffset)DateOfBirth).DateTime;
                    if (ApplicationContext.validData(EmployeeSelected))
                    {
                        if (ApplicationContext.validData(EmployeeSelected.Position))
                        {
                            var obj = new Employee
                            {
                                Name = EmployeeSelected.Name,
                                Surname = EmployeeSelected.Surname,
                                MiddleName = EmployeeSelected.MiddleName,
                                DateOfBirth = EmployeeSelected.DateOfBirth,
                                Position = EmployeeSelected.Position,
                                WorkTime = EmployeeSelected.WorkTime
                            };
                          

                            Employees.Add(obj);
                            db.Employees.Add(obj);
                            db.SaveChanges();
                            EmployeeSelected = new Employee();
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Edit = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(EmployeeSelected))
                    {
                        if (ApplicationContext.validData(EmployeeSelected.Position))
                        {
                            db.Employees.Update(EmployeeSelected);
                            db.SaveChanges();
                            EmployeeSelected = new Employee();
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Remove = ReactiveCommand.Create(() =>
            {

                if (EmployeeSelected != null)
                {
                    db.Employees.Remove(EmployeeSelected);
                    Employees.Remove(EmployeeSelected);
                    db.SaveChanges();
                    EmployeeSelected = new Employee();
                }
            });
            Search = ReactiveCommand.Create(() =>
            {

                if (SearchText == null)
                {
                    Employees = new(db.Employees
                    .Include(x => x.DateOfBirth)
                    .Include(x => x.Position)
                    .ToList());
                    return;
                }
                Employees = new(db.Employees
                .Include(x => x.Position)
                .Where(x => x.Name.Contains(SearchText))
                .ToList());
            });
        }

    }
}
