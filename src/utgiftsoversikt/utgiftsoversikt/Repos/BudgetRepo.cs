using Microsoft.EntityFrameworkCore;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.Data;

namespace utgiftsoversikt.Repos
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll(string userId);
        Budget GetById(string id);
        bool Create(Budget budget);
        bool Update(Budget budget);
        bool Delete(Budget budget);
        void RemoveTrace(Budget budget);
        Task<bool> Write();

    }

    public class BudgetRepo : IBudgetRepo
    {
        private readonly CosmosContext _context;

        public BudgetRepo(CosmosContext context)
        {
            _context = context;
        }

        public List<Budget> GetAll(string userId)
        {
            return _context.Budget?.Where(e => e.UserId == userId).ToList();
        }

        public Budget GetById(string id)
        { 
            return _context.Budget?.FirstOrDefault(e => e.Id == id);
        }

        public bool Create(Budget budget)
        {
            _context.Budget?.Add(budget);
            return Write().Result;
        }

        public bool Update(Budget budget)
        {
            _context.Budget?.Update(budget);
            return Write().Result;
        }
        public bool Delete(Budget budget)
        {
            _context?.Budget?.Remove(budget);
            return Write().Result;
        }
        public void RemoveTrace(Budget budget)
        {
            var trackedBudget = _context.ChangeTracker.Entries<Budget>()
            .FirstOrDefault(e => e.Entity.Id == budget.Id);

            if (trackedBudget != null)
            {
                // Fjern den eksisterende sporing
                _context.Entry(trackedBudget.Entity).State = EntityState.Detached;
            }
        }
        public async Task<bool> Write()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
