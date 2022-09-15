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
    public class PayoutPageViewModel : ViewModelBase
    {
        
        public ICommand Add { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }
        public ICommand Search { get; private set; }
        private ObservableCollection<Payout> _payouts = new ObservableCollection<Payout>();
        public ObservableCollection<Payout> Payouts
        {
            get => _payouts;
            set => this.RaiseAndSetIfChanged(ref _payouts, value);
        }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
        private Payout _payoutSelected;
        public Payout PayoutSelected
        {
            get => _payoutSelected;
            set => this.RaiseAndSetIfChanged(ref _payoutSelected, value);
        }
        private Employee _employeeSelected;
        public Employee EmployeeSelected
        {
            get => _employeeSelected;
            set => this.RaiseAndSetIfChanged(ref _employeeSelected, value);
        }
        private ObservableCollection<Employee> _employeesAll = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> EmployeesAll
        {
            get => _employeesAll;
            set => this.RaiseAndSetIfChanged(ref _employeesAll, value);
        }
        ApplicationContext db;
        public PayoutPageViewModel(ApplicationContext applicationContext)
        {
            db = applicationContext;
            Payouts = new(db.Payouts.ToList());
            PayoutSelected = new Payout();
            EmployeesAll = new(db.Employees.ToList());
            Add = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(PayoutSelected))
                    {
                        if (ApplicationContext.validData(PayoutSelected.Employee))
                        {
                            var obj = new Payout
                            {
                                Sum = PayoutSelected.Sum,
                                Employee = PayoutSelected.Employee,
                            };
                            Payouts.Add(obj);
                            db.Payouts.Add(obj);
                            db.SaveChanges();
                            PayoutSelected = new Payout();
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Edit = ReactiveCommand.Create(() =>
            {

                try
                {
                    if (ApplicationContext.validData(PayoutSelected))
                    {
                        if (ApplicationContext.validData(PayoutSelected.Employee))
                        {
                            db.Payouts.Update(PayoutSelected);
                            db.SaveChanges();
                            PayoutSelected = new Payout();
                        }
                    }
                }
                catch (Exception ex) { }
            });
            Remove = ReactiveCommand.Create(() =>
            {

                if (PayoutSelected != null)
                {
                    db.Payouts.Remove(PayoutSelected);
                    Payouts.Remove(PayoutSelected);
                    db.SaveChanges();
                    PayoutSelected = new Payout();
                }
            });
            Search = ReactiveCommand.Create(() =>
            {

                if (SearchText == null)
                {
                    Payouts = new(db.Payouts
                    .Include(x => x.Employee)
                    .ToList());
                    return;
                }
                Payouts = new(db.Payouts
                .Include(x => x.Employee)
                .Where(x => x.Employee.Name.Contains(SearchText))
                .ToList());
            });
        }

    }
}
