using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using utgiftsoversikt.Models;

namespace utgiftsoversikt.utils
{
    public class MonthUtils
    {

        public static Month AddToMonth(Expense exp, Month month)
        {
            if (month == null || month == null)
            {
                return null;
            }
            var property = month.GetType().GetProperty(exp.Category); // Find the corresponding variable to change based on the category
            if (property == null)
            {
                property = month.GetType().GetProperty("Etc");
            }
            
            if (property != null && property.CanWrite) // Check if variable exists
            {
                decimal oldValue = (decimal) property.GetValue(month); // Gets the old value to be able to update it
                property.SetValue(month, exp.Sum + oldValue); // Sets the updated value for the correct category
                month.Sum += exp.Sum;
                return month;
            }
            return null;
        }
        public static Month SubFromMonth(Expense exp, Month month)
        {
            
            var property = month.GetType().GetProperty(exp.Category);
            if (property != null && property.CanWrite)
            {
                decimal oldValue = (decimal)property.GetValue(month);
                property.SetValue(month, oldValue - exp.Sum);
                month.Sum -= exp.Sum;
                return month;
            }
            return null;
        }
        public static Month EditMonth(Expense expOld, Expense expNew, Month monthOld, Month newMonth)
        {
            // Check if any is null
            if (expOld == null || expNew == null || monthOld == null && newMonth == null)
            {
                return null;
            }

            // Subtract old sum from Old Month
            if (SubFromMonth(expOld, monthOld) == null)
                return null;

            // Add new sum to New month
            return AddToMonth(expNew, newMonth);

        }
    }
}
