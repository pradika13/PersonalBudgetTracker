using System.Collections.Generic;
using System.Linq;
using PersonalBudgetTracker.Models;

namespace PersonalBudgetTracker.Services
{
    public class BudgetManager
    {
        private readonly List<Transaction> transactions = new();

        public IReadOnlyList<Transaction> Transactions
        {
            get
            {
                return transactions.AsReadOnly();
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);
        }

        public bool DeleteTransaction(int id)
        {
            Transaction? transaction =
                transactions.FirstOrDefault(t => t.Id == id);

            if (transaction == null)
            {
                return false;
            }

            transactions.Remove(transaction);

            return true;
        }

        public decimal GetTotalIncome()
        {
            return transactions
                .OfType<IncomeTransaction>()
                .Sum(t => t.Amount);
        }

        public decimal GetTotalExpenses()
        {
            return transactions
                .OfType<ExpenseTransaction>()
                .Sum(t => t.Amount);
        }

        public decimal GetBalance()
        {
            return GetTotalIncome() - GetTotalExpenses();
        }

        public int GetNextId()
        {
            if (transactions.Count == 0)
            {
                return 1;
            }

            return transactions.Max(t => t.Id) + 1;
        }

        public void ReplaceTransactions(
            IEnumerable<Transaction> loadedTransactions)
        {
            transactions.Clear();
            transactions.AddRange(loadedTransactions);
        }
    }
}