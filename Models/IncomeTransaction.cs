using System;

namespace PersonalBudgetTracker.Models
{
    public class IncomeTransaction : Transaction
    {
        public IncomeTransaction(
            int id,
            decimal amount,
            DateTime date,
            string description)
            : base(id, amount, date, description)
        {
        }

        public override string GetTransactionType()
        {
            return "Income";
        }
    }
}