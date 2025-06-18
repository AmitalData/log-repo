public record TenantStatusPM
{
    public bool DoBlocking { get; set; }
    public string BlockType { get; set; } = string.Empty;
    public int? SuspendDaysLeft { get; set; }
    public int? TrialDaysLeft { get; set; }
    public int? PaidDaysLeft { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int? ExpirationDaysLeft { get; set; }
}
