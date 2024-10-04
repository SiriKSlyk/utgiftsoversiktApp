using utgiftsoversikt.Models;

namespace utgiftsoversikt.utils
{
    public class BudgetUtils
    {
        // Calculates the correct sum for a budget based on all categories
        public static decimal CalculateSum(Budget budget)
        {
            return budget.House + budget.Food + budget.Transport + budget.Saving + budget.Debt + budget.Etc;
        }
    }
}