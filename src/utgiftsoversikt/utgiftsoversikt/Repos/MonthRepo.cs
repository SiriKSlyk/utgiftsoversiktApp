using Microsoft.EntityFrameworkCore;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;

namespace utgiftsoversikt.Repos
{
    public interface IMonthRepo
    {
        Month GetById(string id);
        List<Month> GetAll(string userId);
        Month GetByUserIdAndMonth(string userId, string month);
        List<Month> GetAllInYear(string userId, string year);
        bool Create(Month budget);
        bool Update(Month budget);
        bool Delete(Month budget);
        void RemoveTrace(Month month);
        Task<bool> Write();

    }

    public class MonthRepo : IMonthRepo
    {
        private readonly CosmosContext _context;

        public MonthRepo(CosmosContext context)
        {
            _context = context;
        }

        public Month GetById(string id)
        {
            return _context.Month?.FirstOrDefault(m => m.Id == id);
        }
        public List<Month> GetAll(string userId)
        {
            var result = _context.Month?.Where(m => m.UserId == userId).ToList();
            return result != null ? result : new List<Month>();
        }

        public Month GetByUserIdAndMonth(string userId, string month)
        {
            return _context.Month?.FirstOrDefault(m => m.UserId == userId && m.MonthYear == month);
        }

        public List<Month> GetAllInYear(string userId, string year)
        {
            return _context.Month?
                .Where(m => m.UserId == userId && m.MonthYear.Substring(2, m.MonthYear.Length) == year) // Correct user, and check if year is correct by removing month.
                .ToList();
        }

        public bool Create(Month month)
        { 
            _context.Month?.Add(month);
            return true;
        }

        public bool Update(Month month)
        {
            _context.Month?.Update(month);
            return true;
        }
        public bool Delete(Month month)
        {
            _context.Month?.Remove(month);
            return true;
        }

        public void RemoveTrace(Month month)
        {
            var trackedMonth = _context.ChangeTracker.Entries<Month>()
            .FirstOrDefault(e => e.Entity.Id == month.Id);

            if (trackedMonth != null)
            {
                // Fjern den eksisterende sporing
                _context.Entry(trackedMonth.Entity).State = EntityState.Detached;
            }
        }
        public async Task<bool> Write()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
