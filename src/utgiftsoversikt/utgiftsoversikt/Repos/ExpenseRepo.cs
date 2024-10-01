using Microsoft.EntityFrameworkCore;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.utils;

namespace utgiftsoversikt.Repos
{
    public interface IExpenseRepo
    {
        List<Expense> GetAll(string userId, string month);
        Expense GetById(string id);
        bool Create(Expense expense);
        bool Update(Expense expense);
        bool Delete(Expense expense);
        void RemoveTrace(Expense exp);
        Task<bool> Write();
    }


    public class ExpenseRepo : IExpenseRepo
    {

        private readonly CosmosContext _context;

        public ExpenseRepo(CosmosContext context)
        {
            _context = context;
        }

        public List<Expense> GetAll(string userId, string month)
        {
            return _context.Expenses.Where(e => e.UserId == userId && e.Month == month).ToList();
        }

        public Expense GetById(string id)
        {
            // User id for future authorication: Do this user own this expense
            return _context.Expenses?.FirstOrDefault(e => e.Id == id);
        }

        public bool Create(Expense expense)
        {
            _context.Expenses?.Add(expense);
            return true;

        }

        public bool Update(Expense expense)
        {
            _context.Expenses?.Update(expense);

            return true;
        }
        public bool Delete(Expense expense)
        {

            _context?.Expenses?.Remove(expense);

            return true;
        }

        public void RemoveTrace(Expense exp)
        {
            var trackedExp = _context.ChangeTracker.Entries<Expense>()
            .FirstOrDefault(e => e.Entity.Id == exp.Id);

            if (trackedExp != null)
            {
                // Fjern den eksisterende sporing
                _context.Entry(trackedExp.Entity).State = EntityState.Detached;
            }
        }

        public async Task<bool> Write()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
