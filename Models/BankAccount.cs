namespace wt_lab3_3mvc_besman.Models;

public class BankAccount
{
    public int Id { get; set; }
    public string Owner { get; set; } = "";
    public string AccountNumber { get; set; } = "";
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "BYN";
    public bool IsActive { get; set; }
}