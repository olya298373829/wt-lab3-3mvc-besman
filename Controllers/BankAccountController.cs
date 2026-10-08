using wt_lab3_3mvc_besman.Models;
using wt_lab3_3mvc_besman.Views;

namespace wt_lab3_3mvc_besman.Controllers;

public class BankAccountController
{
    private readonly List<BankAccount> accounts;
    private readonly BankAccountView view;

    public BankAccountController()
    {
        accounts = new List<BankAccount>
        {
            new BankAccount { Id = 1, Owner = "Иванов И.И.", AccountNumber = "BY01 1111 2222 3333", Balance = 1500.50m, Currency = "BYN", IsActive = true },
            new BankAccount { Id = 2, Owner = "Петров П.П.", AccountNumber = "BY02 4444 5555 6666", Balance = 320.00m,  Currency = "BYN", IsActive = true },
            new BankAccount { Id = 3, Owner = "Сидорова А.А.", AccountNumber = "BY03 7777 8888 9999", Balance = 0m,      Currency = "BYN", IsActive = false }
        };
        view = new BankAccountView();
    }

    // ===== Базовые операции =====

    public void Index()
    {
        view.ShowAccounts(accounts);
    }

    public void Show(int id)
    {
        var acc = accounts.FirstOrDefault(a => a.Id == id);
        if (acc == null)
        {
            view.ShowMessage("Счёт не найден.");
            return;
        }
        view.ShowAccount(acc);
    }

    public void Add(BankAccount account)
    {
        accounts.Add(account);
        view.ShowMessage("Счёт успешно добавлен.");
    }

    public void Delete(int id)
    {
        var acc = accounts.FirstOrDefault(a => a.Id == id);
        if (acc == null)
        {
            view.ShowMessage("Счёт не найден.");
            return;
        }
        accounts.Remove(acc);
        view.ShowMessage("Счёт успешно удалён.");
    }

    // ===== Банковские операции =====

    public void Deposit(int id, decimal amount)
    {
        var acc = accounts.FirstOrDefault(a => a.Id == id);
        if (acc == null) { view.ShowMessage("Счёт не найден."); return; }
        if (!acc.IsActive) { view.ShowMessage("Счёт заблокирован."); return; }
        if (amount <= 0) { view.ShowMessage("Сумма должна быть положительной."); return; }

        acc.Balance += amount;
        view.ShowMessage($"Счёт пополнен на {amount:F2} {acc.Currency}. Новый баланс: {acc.Balance:F2} {acc.Currency}");
    }

    public void Withdraw(int id, decimal amount)
    {
        var acc = accounts.FirstOrDefault(a => a.Id == id);
        if (acc == null) { view.ShowMessage("Счёт не найден."); return; }
        if (!acc.IsActive) { view.ShowMessage("Счёт заблокирован."); return; }
        if (amount <= 0) { view.ShowMessage("Сумма должна быть положительной."); return; }
        if (acc.Balance < amount) { view.ShowMessage("Недостаточно средств."); return; }

        acc.Balance -= amount;
        view.ShowMessage($"Снято {amount:F2} {acc.Currency}. Новый баланс: {acc.Balance:F2} {acc.Currency}");
    }

    public void Transfer(int fromId, int toId, decimal amount)
    {
        var from = accounts.FirstOrDefault(a => a.Id == fromId);
        var to   = accounts.FirstOrDefault(a => a.Id == toId);

        if (from == null || to == null) { view.ShowMessage("Один из счетов не найден."); return; }
        if (!from.IsActive || !to.IsActive) { view.ShowMessage("Один из счетов заблокирован."); return; }
        if (amount <= 0) { view.ShowMessage("Сумма должна быть положительной."); return; }
        if (from.Balance < amount) { view.ShowMessage("Недостаточно средств."); return; }

        from.Balance -= amount;
        to.Balance   += amount;
        view.ShowMessage($"Перевод {amount:F2} {from.Currency} выполнен успешно.");
    }

    public void Search(string text)
    {
        var result = accounts
            .Where(a => a.Owner.Contains(text, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (result.Count == 0) { view.ShowMessage("Ничего не найдено."); return; }
        view.ShowAccounts(result);
    }

    public void TotalBalance()
    {
        decimal total = accounts.Where(a => a.IsActive).Sum(a => a.Balance);
        view.ShowTotal(total, "BYN");
    }

    public void ShowActive()
    {
        var active = accounts.Where(a => a.IsActive).ToList();
        if (active.Count == 0) { view.ShowMessage("Активных счетов нет."); return; }
        view.ShowAccounts(active);
    }

    // ===== Обработка пользовательского ввода =====

    private void ShowAccountByUserInput()
    {
        Console.Write("Введите ID счёта: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { view.ShowMessage("Некорректный ID."); return; }
        Show(id);
    }

    private void AddAccountByUserInput()
    {
        Console.Write("Введите ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { view.ShowMessage("Некорректный ID."); return; }

        Console.Write("Введите владельца: ");
        string owner = Console.ReadLine() ?? "";

        Console.Write("Введите номер счёта: ");
        string number = Console.ReadLine() ?? "";

        Console.Write("Введите начальный баланс: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal balance)) { view.ShowMessage("Некорректный баланс."); return; }

        Console.Write("Введите валюту (по умолчанию BYN): ");
        string currency = Console.ReadLine() ?? "BYN";
        if (string.IsNullOrWhiteSpace(currency)) currency = "BYN";

        Add(new BankAccount { Id = id, Owner = owner, AccountNumber = number, Balance = balance, Currency = currency, IsActive = true });
    }

    private void DeleteAccountByUserInput()
    {
        Console.Write("Введите ID счёта для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { view.ShowMessage("Некорректный ID."); return; }
        Delete(id);
    }

    private void DepositByUserInput()
    {
        Console.Write("Введите ID счёта: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { view.ShowMessage("Некорректный ID."); return; }
        Console.Write("Введите сумму пополнения: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount)) { view.ShowMessage("Некорректная сумма."); return; }
        Deposit(id, amount);
    }

    private void WithdrawByUserInput()
    {
        Console.Write("Введите ID счёта: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { view.ShowMessage("Некорректный ID."); return; }
        Console.Write("Введите сумму снятия: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount)) { view.ShowMessage("Некорректная сумма."); return; }
        Withdraw(id, amount);
    }

    private void TransferByUserInput()
    {
        Console.Write("Введите ID счёта-отправителя: ");
        if (!int.TryParse(Console.ReadLine(), out int fromId)) { view.ShowMessage("Некорректный ID."); return; }
        Console.Write("Введите ID счёта-получателя: ");
        if (!int.TryParse(Console.ReadLine(), out int toId)) { view.ShowMessage("Некорректный ID."); return; }
        Console.Write("Введите сумму перевода: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount)) { view.ShowMessage("Некорректная сумма."); return; }
        Transfer(fromId, toId, amount);
    }

    private void SearchByUserInput()
    {
        Console.Write("Введите часть имени владельца: ");
        string text = Console.ReadLine() ?? "";
        Search(text);
    }

    // ===== Меню =====

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== БАНК. МЕНЮ ===");
            Console.WriteLine("1. Показать все счета");
            Console.WriteLine("2. Найти счёт по ID");
            Console.WriteLine("3. Добавить счёт");
            Console.WriteLine("4. Удалить счёт");
            Console.WriteLine("5. Пополнить счёт");
            Console.WriteLine("6. Снять средства");
            Console.WriteLine("7. Перевод между счетами");
            Console.WriteLine("8. Поиск по владельцу");
            Console.WriteLine("9. Показать активные счета");
            Console.WriteLine("10. Общая сумма по счетам");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            var command = Console.ReadLine();

            switch (command)
            {
                case "1": Index(); break;
                case "2": ShowAccountByUserInput(); break;
                case "3": AddAccountByUserInput(); break;
                case "4": DeleteAccountByUserInput(); break;
                case "5": DepositByUserInput(); break;
                case "6": WithdrawByUserInput(); break;
                case "7": TransferByUserInput(); break;
                case "8": SearchByUserInput(); break;
                case "9": ShowActive(); break;
                case "10": TotalBalance(); break;
                case "0": return;
                default: view.ShowMessage("Неизвестная команда."); break;
            }
        }
    }
}