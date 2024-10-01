using utgiftsoversikt.Models;

namespace utgiftsoversikt.utils
{
    public class BudgetUtils
    {

        public static bool ValidateBudget(Budget budget)
        {
            var calSum = CalculateSum(budget);


            return budget.Sum == calSum;

        }

        // If the total sum of all rowns is less then the sum => add the remaining to savings
        public static bool ChangeBudget(Budget budget)
        {
            var calSum = CalculateSum(budget);
            if (!ValidateBudget(budget))
            {
                if (calSum < budget.Sum)
                {
                    budget.Saving += (budget.Sum - calSum);
                    return true;
                }
            }
            return false;
        }
        public static decimal CalculateSum(Budget budget)
        {
            return budget.House + budget.Food + budget.Transport + budget.Saving + budget.Debt + budget.Etc;
        }
    }
}
