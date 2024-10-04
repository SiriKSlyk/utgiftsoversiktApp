using utgiftsoversikt.Models;
using utgiftsoversikt.Repos;


namespace utgiftsoversikt.Services
{
    public interface IBudgetService
    {
        bool Create(Budget budget);
        List<Budget> GetAll(string userId);
        Budget GetById(string id);
        bool Delete(Budget budget);
        bool Update(Budget budget);
    }
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public bool Create(Budget budget)
        {
            budget.Id = Guid.NewGuid().ToString();
            _budgetRepo.Create(budget);

            return _budgetRepo.Write().Result;
        }

        public bool Delete(Budget budget)
        {
            _budgetRepo.Delete(budget);
            return _budgetRepo.Write().Result;
        }

        public List<Budget> GetAll(string userId)
        {
            return _budgetRepo.GetAll(userId);
        }

        public Budget GetById(string id)
        {
            return _budgetRepo.GetById(id);
        }

        public bool Update(Budget budget)
        {
            _budgetRepo.RemoveTrace(budget);
            _budgetRepo.Update(budget);

            return _budgetRepo.Write().Result;
        }

    }
}