using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using utgiftsoversikt.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace utgiftsoversikt.Data
{
    // Mock of a database with starting values
    public class Database
    {

        public static bool IsLocal = true;


        // Takes all expenses and calculates the montly total expenses in categories and returnes the month with this numbers
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
                UserId =    users.First().Id,
                House =     sums.GetValueOrDefault("house"),
                Food =      sums.GetValueOrDefault("food"),
                Cloths =    sums.GetValueOrDefault("cloths"),
                Etc =       sums.GetValueOrDefault("etc"),
                Saving =    sums.GetValueOrDefault("saving"),
                Debt =      sums.GetValueOrDefault("debt"),
                MonthYear = "092024",
                Sum =       sums.GetValueOrDefault("sum"),
                Transport = sums.GetValueOrDefault("transport"),
                BudgetId =  budgets.First().Id
            };
        }
        // List of users
        public static List<User> users = new List<User>()
        {
            new User() { Email = "Test@mail.com", First_name = "Ole", Last_name = "Olsen", BudgetId = "" },
            new User() { Email = "Test1@mail.com", First_name = "Test1", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test2@mail.com", First_name = "Test2", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test3@mail.com", First_name = "Test3", Last_name = "Test", BudgetId = "" },

        };
        // List of expenses for user Test@mail.com
        public static List<Expense> expenses = new List<Expense>()
        {
            new Expense {UserId = users.First().Id, Date="2024-09-18",Category="House", Month="092024", Shop="-", Sum=1000, Description="Strøm"},
            new Expense {UserId = users.First().Id, Date="2024-09-20",Category="Debt", Month="092024", Shop="-", Sum=10000, Description="Huslån"},
            new Expense {UserId = users.First().Id, Date="2024-09-16",Category="Food", Month="092024", Shop="Rema 1000", Sum=100},
            new Expense {UserId = users.First().Id, Date="2024-09-25",Category="Food", Month="092024", Shop="Meny", Sum=80},
            new Expense {UserId = users.First().Id, Date="2024-09-10",Category="Food", Month="092024", Shop="Coop Obs", Sum=500},
            new Expense {UserId = users.First().Id, Date="2024-09-15",Category="Food", Month="092024", Shop="Kiwi", Sum=300},
            new Expense {UserId = users.First().Id, Date="2024-09-14",Category="Food", Month="092024", Shop="Spar", Sum=200},
            new Expense {UserId = users.First().Id, Date="2024-09-15",Category="Etc", Month="092024", Shop="Netflix", Sum=129},
            new Expense {UserId = users.First().Id, Date="2024-09-15",Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Date="2024-09-01",Category="Food", Month="092024", Shop="Kiwi", Sum=100}
        };
        
        public static List<Budget> budgets = new List<Budget>()
        {
            new Budget {UserId = users.First().Id, Debt=15000, Transport=2000, Etc=1000, Food=2000, House=10000, Saving=5000, Sum=25000}
        };
        // Returnes a month with the correct expenses calculated
        public static List<Month> months = new List<Month>()
        {
            CalculateExpense(expenses),
        };

    }
}
