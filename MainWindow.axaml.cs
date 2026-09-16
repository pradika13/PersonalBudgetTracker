using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Interactivity;

using PersonalBudgetTracker.Models;
using PersonalBudgetTracker.Services;

namespace PersonalBudgetTracker
{
    public partial class MainWindow : Window
    {
        private readonly BudgetManager budgetManager;
        private readonly FileManager fileManager;

        public MainWindow()
        {
            InitializeComponent();

            budgetManager = new BudgetManager();

            string filePath =
                System.IO.Path.Combine(
                    AppContext.BaseDirectory,
                    "transactions.json");

            fileManager = new FileManager(filePath);

            TransactionDate.SelectedDate =
                DateTimeOffset.Now;

            LoadSavedTransactions();

            UpdateCategory();

            RefreshDisplay();
        }

        private void AddTransaction_Click(
            object? sender,
            RoutedEventArgs e)
        {
            try
            {
                MessageText.Text = "";

                // Validate amount
                if (!decimal.TryParse(
                    AmountBox.Text,
                    out decimal amount))
                {
                    MessageText.Text =
                        "Please enter a valid amount.";

                    return;
                }

                if (amount <= 0)
                {
                    MessageText.Text =
                        "Amount must be greater than zero.";

                    return;
                }

                // Validate description
                string description =
                    DescriptionBox.Text?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(description))
                {
                    MessageText.Text =
                        "Please enter a description.";

                    return;
                }

                // Validate date
                if (TransactionDate.SelectedDate == null)
                {
                    MessageText.Text =
                        "Please select a transaction date.";

                    return;
                }

                DateTime date =
                    TransactionDate.SelectedDate
                        .Value.DateTime;

                int id =
                    budgetManager.GetNextId();

                string type =
                    GetComboBoxText(TypeBox);

                Transaction transaction;

                // POLYMORPHISM:
                // Transaction can reference either
                // IncomeTransaction or ExpenseTransaction.
                if (type == "Income")
                {
                    transaction =
                        new IncomeTransaction(
                            id,
                            amount,
                            date,
                            description);
                }
                else
                {
                    string category =
                        GetComboBoxText(CategoryBox);

                    transaction =
                        new ExpenseTransaction(
                            id,
                            amount,
                            date,
                            description,
                            category);
                }

                budgetManager.AddTransaction(
                    transaction);

                SaveTransactions();

                RefreshDisplay();

                ClearInputs();

                MessageText.Text =
                    "Transaction added successfully.";
            }
            catch (Exception ex)
            {
                MessageText.Text =
                    "Unable to add transaction: "
                    + ex.Message;
            }
        }

        private void DeleteTransaction_Click(
            object? sender,
            RoutedEventArgs e)
        {
            try
            {
                if (TransactionList.SelectedItem
                    is not TransactionDisplay selected)
                {
                    MessageText.Text =
                        "Please select a transaction to delete.";

                    return;
                }

                bool deleted =
                    budgetManager.DeleteTransaction(
                        selected.Id);

                if (deleted)
                {
                    SaveTransactions();

                    RefreshDisplay();

                    MessageText.Text =
                        "Transaction deleted successfully.";
                }
                else
                {
                    MessageText.Text =
                        "Transaction could not be found.";
                }
            }
            catch (Exception ex)
            {
                MessageText.Text =
                    "Unable to delete transaction: "
                    + ex.Message;
            }
        }

        private void Save_Click(
            object? sender,
            RoutedEventArgs e)
        {
            try
            {
                SaveTransactions();

                MessageText.Text =
                    "Data saved successfully.";
            }
            catch (Exception ex)
            {
                MessageText.Text =
                    "Unable to save data: "
                    + ex.Message;
            }
        }

        private void Clear_Click(
            object? sender,
            RoutedEventArgs e)
        {
            ClearInputs();

            MessageText.Text = "";
        }

        private void TypeBox_SelectionChanged(
            object? sender,
            SelectionChangedEventArgs e)
        {
            UpdateCategory();
        }

        private void UpdateCategory()
        {
            if (CategoryBox == null ||
                TypeBox == null)
            {
                return;
            }

            string type =
                GetComboBoxText(TypeBox);

            CategoryBox.IsEnabled =
                type == "Expense";
        }

        private string GetComboBoxText(
            ComboBox comboBox)
        {
            if (comboBox.SelectedItem
                is ComboBoxItem item)
            {
                return item.Content?.ToString()
                       ?? "";
            }

            return "";
        }

        private void RefreshDisplay()
        {
            List<TransactionDisplay> display =
                budgetManager.Transactions
                    .Select(t =>
                        new TransactionDisplay
                        {
                            Id = t.Id,

                            DateDisplay =
                                t.Date.ToString(
                                    "dd/MM/yyyy"),

                            Type =
                                t.GetTransactionType(),

                            Description =
                                t.Description,

                            Category =
                                t.GetCategory(),

                            AmountDisplay =
                                t.Amount.ToString("C")
                        })
                    .ToList();

            TransactionList.ItemsSource =
                display;

            IncomeText.Text =
                budgetManager
                    .GetTotalIncome()
                    .ToString("C");

            ExpenseText.Text =
                budgetManager
                    .GetTotalExpenses()
                    .ToString("C");

            BalanceText.Text =
                budgetManager
                    .GetBalance()
                    .ToString("C");
        }

        private void ClearInputs()
        {
            AmountBox.Text = "";

            DescriptionBox.Text = "";

            TypeBox.SelectedIndex = 0;

            CategoryBox.SelectedIndex = 0;

            TransactionDate.SelectedDate =
                DateTimeOffset.Now;
        }

        private void SaveTransactions()
        {
            fileManager.SaveTransactions(
                budgetManager.Transactions);
        }

        private void LoadSavedTransactions()
        {
            try
            {
                List<Transaction> loaded =
                    fileManager.LoadTransactions();

                budgetManager.ReplaceTransactions(
                    loaded);
            }
            catch (Exception)
            {
                MessageText.Text =
                    "Saved data could not be loaded.";
            }
        }
    }

    public class TransactionDisplay
    {
        public int Id { get; set; }

        public string DateDisplay { get; set; } = "";

        public string Type { get; set; } = "";

        public string Description { get; set; } = "";

        public string Category { get; set; } = "";

        public string AmountDisplay { get; set; } = "";
    }
}