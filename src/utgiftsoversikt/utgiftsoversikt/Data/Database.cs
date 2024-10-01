using System.ComponentModel.DataAnnotations;
using utgiftsoversikt.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace utgiftsoversikt.Data
{
    public class Database
    {
        public static List<User> users = new List<User>()
        { 
            new User() { Email = "Test@mail.com", First_name = "Test", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test1@mail.com", First_name = "Test1", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test2@mail.com", First_name = "Test2", Last_name = "Test", BudgetId = "" },
            new User() { Email = "Test3@mail.com", First_name = "Test3", Last_name = "Test", BudgetId = "" },
        
        };

        public static List<Expense> expenses = new List<Expense>()
        {
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100},
            new Expense {UserId = users.First().Id, Category="Food", Month="092024", Shop="Kiwi", Sum=100}
        };

        public static List<Budget> budgets = new List<Budget>()
        {
            new Budget {UserId=users.First().Id, Debt=1000, Transport=1000, Etc=1000, Food=1000, House=1000, Saving=1000, Sum=6000}
        };

        public static List<Month> months = new List<Month>()
        {
            new Month {UserId=users.First().Id, House=1000, Food=1000, Cloths=500, Etc=100, Saving=10000, Debt=1000, MonthYear="092024", Sum=14600, Transport=1000, BudgetId=budgets.First().Id}
        };


        public static User GetByEmail(string email)
        {
            return users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        
    }
}
