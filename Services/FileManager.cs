using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using PersonalBudgetTracker.Models;

namespace PersonalBudgetTracker.Services
{
    public class FileManager
    {
        private readonly string filePath;

        public FileManager(string filePath)
        {
            this.filePath = filePath;
        }

        public void SaveTransactions(
            IEnumerable<Transaction> transactions)
        {
            List<StoredTransaction> data =
                transactions.Select(t => new StoredTransaction
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    Type = t.GetTransactionType(),
                    Category = t.GetCategory()
                }).ToList();

            JsonSerializerOptions options =
                new JsonSerializerOptions
                {
                    WriteIndented = true
                };

            string json =
                JsonSerializer.Serialize(data, options);

            File.WriteAllText(filePath, json);
        }

        public List<Transaction> LoadTransactions()
        {
            List<Transaction> transactions = new();

            if (!File.Exists(filePath))
            {
                return transactions;
            }

            string json = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return transactions;
            }

            List<StoredTransaction>? data =
                JsonSerializer.Deserialize<List<StoredTransaction>>(json);

            if (data == null)
            {
                return transactions;
            }

            foreach (StoredTransaction item in data)
            {
                if (item.Type == "Income")
                {
                    transactions.Add(
                        new IncomeTransaction(
                            item.Id,
                            item.Amount,
                            item.Date,
                            item.Description));
                }
                else if (item.Type == "Expense")
                {
                    transactions.Add(
                        new ExpenseTransaction(
                            item.Id,
                            item.Amount,
                            item.Date,
                            item.Description,
                            item.Category));
                }
            }

            return transactions;
        }
    }
}