using System;

namespace PersonalBudgetTracker.Models
{
    public abstract class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        protected Transaction(
            int id,
            decimal amount,
            DateTime date,
            string description)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Description = description;
        }

        public abstract string GetTransactionType();

        public virtual string GetCategory()
        {
            return "-";
        }
    }
}