using wt_lab3_3mvc_besman.Models;

namespace wt_lab3_3mvc_besman.Views;

public class BankAccountView
{
    public void ShowAccounts(List<BankAccount> accounts)
    {
        Console.WriteLine("=== СПИСОК СЧЕТОВ ===");
        if (accounts.Count == 0)
        {
            Console.WriteLine("Список пуст.");
            return;
        }

        foreach (var acc in accounts)
        {
            Console.WriteLine(
                $"{acc.Id}. {acc.Owner} | " +
                $"{acc.AccountNumber} | " +
                $"{acc.Balance:F2} {acc.Currency} | " +
                $"Статус: {(acc.IsActive ? "активен" : "заблокирован")}"
            );
        }
    }

    public void ShowAccount(BankAccount acc)
    {
        Console.WriteLine("=== СЧЁТ ===");
        Console.WriteLine($"ID: {acc.Id}");
        Console.WriteLine($"Владелец: {acc.Owner}");
        Console.WriteLine($"Номер счёта: {acc.AccountNumber}");
        Console.WriteLine($"Баланс: {acc.Balance:F2} {acc.Currency}");
        Console.WriteLine($"Статус: {(acc.IsActive ? "активен" : "заблокирован")}");
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ShowTotal(decimal total, string currency)
    {
        Console.WriteLine("=== ИТОГО ===");
        Console.WriteLine($"Общая сумма по всем счетам: {total:F2} {currency}");
    }
}