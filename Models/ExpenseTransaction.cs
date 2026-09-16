using System;

namespace PersonalBudgetTracker.Models
{
    public class ExpenseTransaction : Transaction
    {
        public string Category { get; set; }

        public ExpenseTransaction(
            int id,
            decimal amount,
            DateTime date,
            string description,
            string category)
            : base(id, amount, date, description)
        {
            Category = category;
        }

        public override string GetTransactionType()
        {
            return "Expense";
        }

        public override string GetCategory()
        {
            return Category;
        }
    }
}