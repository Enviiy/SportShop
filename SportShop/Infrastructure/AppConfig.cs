namespace SportShop.Infrastructure;

public class AppConfig
{
    public Company company { get; set; } = new();
    public Database database { get; set; } = new();
}

public class Company
{
    public string? CompanyName { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyPhoneShort { get; set; }
    public string? CompanyEmail { get; set; }
}

public class Database
{
    public string? connectionString { get; set; }
}