namespace utgiftsoversikt.Models
{
    public class Budget
    {
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public decimal House { get; set; } = 0M;
        public decimal Food { get; set; } = 0M;
        public decimal Transport { get; set; } = 0M;
        public decimal Debt { get; set; } = 0M;
        public decimal Saving { get; set; } = 0M;
        public decimal Etc { get; set; } = 0M;
        public decimal Sum { get; set; } = 0M;
    }
}
