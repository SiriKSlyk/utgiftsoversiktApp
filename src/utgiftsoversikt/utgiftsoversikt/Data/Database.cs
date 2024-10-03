using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using utgiftsoversikt.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace utgiftsoversikt.Data
{
    public class Database
    {

        public static bool IsLocal = true;

        private static string GetUId(string email)
        {
            return Database.users.Find(u => u.Email.ToLower() == email.ToLower()).Id;
        }

        private static Month CalculateExpense(List<Expense> exps)
        {



            var sums = new Dictionary<string, decimal>
            {
                {"food", 0 },
                {"debt", 0},
                {"house", 0},
                {"etc", 0 },
                {"cloths", 0 },
                {"saving", 0 },
                {"transport", 0 },
                {"sum", 0 }
            };
            foreach (var item in exps)
            {
                sums[item.Category.ToLower()] += item.Sum;
                sums["sum"] += item.Sum;
            }
            return new Month
            {
                UserId = GetUId("test@mail.com"),
                House = sums.GetValueOrDefault("house"),
                Food = sums.GetValueOrDefault("food"),
                Cloths = sums.GetValueOrDefault("cloths"),
                Etc = sums.GetValueOrDefault("etc"),
                Saving = sums.GetValueOrDefault("saving"),
                Debt = sums.GetValueOrDefault("debt"),
                MonthYear = "092024",
                Sum = sums.GetValueOrDefault("sum"),
                Transport = sums.GetValueOrDefault("transport"),
                BudgetId = budgets.First().Id
            };
        }


        public static List<User> users = new List<User>()
        {
            new User() { Email = "Test@mail.com", First_name = "Test", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test1@mail.com", First_name = "Test1", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test2@mail.com", First_name = "Test2", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test3@mail.com", First_name = "Test3", Last_name = "Test", BudgetId = "" },

        };

        public static List<Expense> expenses = new List<Expense>()
        {
            new Expense {UserId = GetUId("test@mail.com"), Category="House", Month="092024", Shop="-", Sum=1000, Description="Strøm"},
            new Expense {UserId = GetUId("test@mail.com"), Category="Debt", Month="091024", Shop="-", Sum=15000, Description="Huslån"},
            new Expense {UserId = GetUId("test@mail.com"), Category="Food", Month="091824", Shop="Rema 1000", Sum=100},
            new Expense {UserId = GetUId("test@mail.com"), Category="Food", Month="091724", Shop="Meny", Sum=80},
            new Expense {UserId = GetUId("test@mail.com"), Category="Food", Month="091024", Shop="Coop Obs", Sum=500},
            new Expense {UserId = GetUId("test@mail.com"), Category="Food", Month="092924", Shop="Kiwi", Sum=300},
            new Expense {UserId = GetUId("test@mail.com"), Category="Food", Month="092324", Shop="Spar", Sum=200},
            new Expense {UserId = GetUId("test@mail.com"), Category="Etc", Month="092024", Shop="Netflix", Sum=129},
            new Expense {UserId = GetUId("test@mail.com"), Category="Food", Month="092424", Shop="Kiwi", Sum=100},
            new Expense {UserId = GetUId("test@mail.com"), Category="Food", Month="092824", Shop="Kiwi", Sum=100}
        };

        public static List<Budget> budgets = new List<Budget>()
        {
            new Budget {UserId=GetUId("test@mail.com"), Debt=15000, Transport=1000, Etc=1000, Food=1000, House=1000, Saving=1000, Sum=6000}
        };

        public static List<Month> months = new List<Month>()
        {
            CalculateExpense(expenses),
            //new Month {UserId=users.First().Id, House=1000, Food=1000, Cloths=500, Etc=100, Saving=10000, Debt=1000, MonthYear="092024", Sum=14600, Transport=1000, BudgetId=budgets.First().Id}
        };


        public static User GetByEmail(string email)
        {
            return users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }


    }
}
