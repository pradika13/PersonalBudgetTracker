# Personal Budget Tracker

## Project Description

Personal Budget Tracker is a C# desktop application developed for ITS203 Object-Oriented Design and Programming.

The application allows users to record income and expenses, organise expenses by category, view transaction history, and calculate total income, total expenses, and the current balance.

Transaction data is stored locally using JSON.

## Features

- Add income transactions
- Add expense transactions
- Select expense categories
- View transaction history
- Calculate total income
- Calculate total expenses
- Calculate current balance
- Delete transactions
- Validate transaction amounts
- Save and load transaction data using JSON
- Handle file and JSON errors

## Technologies Used

- C#
- .NET
- Avalonia UI
- System.Text.Json
- Visual Studio Code
- Git
- GitHub

## OOP Design

The project uses several object-oriented programming principles.

### Abstraction

`Transaction` is an abstract base class containing common transaction information and methods.

### Inheritance

`IncomeTransaction` and `ExpenseTransaction` inherit from the `Transaction` class.

### Polymorphism

The application uses overridden methods such as `GetTransactionType()` and `GetCategory()` so different transaction types can provide their own behaviour.

### Encapsulation

Transaction data and the transaction collection are controlled through classes and properties. `BudgetManager` manages the transaction collection and calculations.

## Main Classes

- `Transaction` – abstract base class for transactions.
- `IncomeTransaction` – represents income.
- `ExpenseTransaction` – represents expenses and their categories.
- `StoredTransaction` – represents transaction data used for JSON storage.
- `BudgetManager` – manages transactions and calculations.
- `FileManager` – saves and loads transaction data using JSON.

## How to Run

1. Open the project in Visual Studio Code.
2. Open the terminal.
3. Run:

```bash
dotnet restore
dotnet build
dotnet run