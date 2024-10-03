
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.Repos;
using utgiftsoversikt.utils;


namespace utgiftsoversikt.Services
{
    public interface IExpenseService
    {
        bool Create(Expense expense);
        List<Expense> GetAllByUserIdAndMonth(string userId, string month);
        Expense GetById(string id);
        bool Delete(Expense expense);
        bool Update(Expense expense);

    }
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepo _expenseRepo;
        private readonly IMonthRepo _monthRepo;
        private readonly IUserRepo _userRepo;
        

        public ExpenseService(IExpenseRepo expenseRepo, IMonthRepo monthRepo, IUserRepo userRepo)
        {
            _expenseRepo = expenseRepo;
            _monthRepo = monthRepo;
            _userRepo = userRepo;
        }

        public bool Create(Expense expense)
        {

            // add logic to check if expense was created in database

            var month = _monthRepo.GetByUserIdAndMonth(expense.UserId, expense.Month);
            var newMonth = MonthUtils.AddToMonth(expense, month);
            if (month == null)
                return false;
            var res1 = _expenseRepo.Create(expense);
            var res2 = _monthRepo.Update(newMonth);

            return _expenseRepo.Write().Result && _monthRepo.Write().Result;
        }

        public bool Delete(Expense expense)
        {

            if (expense != null)
                _expenseRepo.RemoveTrace(expense);


            if (_expenseRepo.Delete(expense))
            {
                var month = _monthRepo.GetByUserIdAndMonth(expense.UserId, expense.Month);
                var newMonth = MonthUtils.SubFromMonth(expense, month);
                var result = _monthRepo.Update(newMonth);
                // Change the sum of the month
                return _expenseRepo.Write().Result && _monthRepo.Write().Result;
            }
            return true;

        }

        public List<Expense> GetAllByUserIdAndMonth(string userId, string month)
        {
            return _expenseRepo.GetAll(userId, month);
        }

        public Expense GetById(string id)
        {
            return _expenseRepo.GetById(id);
        }

        //public bool Update(Expense expense)
        //{
        //    var exp = GetById(expense.Id); // Get old Expense before modifying database

        //    _expenseRepo.RemoveTrace(exp);

        //    var monthIdOld = exp.Month; 
        //    var oldMonth = _monthRepo.GetByUserIdAndMonth(exp.UserId, exp.Month); // get old month with expenses to be updated
        //    exp = expense;
        //    var newMonth = oldMonth; // The month to be updated
        //    if (monthIdOld != exp.Month) // If the expense has another month then before, we change the month to be updated
        //        newMonth = _monthRepo.GetByUserIdAndMonth(exp.UserId, exp.Month);

        //    var monthUpdate = MonthUtils.EditMonth(exp, expense, oldMonth, newMonth);
        //    var resExp = _expenseRepo.Update(exp);
        //    var resMonth = _monthRepo.Update(monthUpdate);


        //    return _monthRepo.Write().Result && _expenseRepo.Write().Result;
        //}

        public bool Update(Expense expense)
        {
            var oldExp = GetById(expense.Id); // Getting the original expense before updated values
            _expenseRepo.RemoveTrace(oldExp);

            var oldMonth = _monthRepo.GetByUserIdAndMonth(oldExp.UserId, oldExp.Month);
            var newMonth = _monthRepo.GetByUserIdAndMonth(oldExp.UserId, expense.Month);


            MonthUtils.SubFromMonth(oldExp, oldMonth);
            MonthUtils.AddToMonth(expense, newMonth);

            _expenseRepo.Update(expense);

            if (oldMonth != newMonth)
            {
                _monthRepo.Update(oldMonth);
            }
            _monthRepo.Update(newMonth);

            return _monthRepo.Write().Result && _expenseRepo.Write().Result;




        }
    }
}