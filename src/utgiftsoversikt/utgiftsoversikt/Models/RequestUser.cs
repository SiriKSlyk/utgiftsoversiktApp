namespace utgiftsoversikt.Models
{
    public class RequestUser
    {
        public string? Id { get; set; }
        public string Email { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public bool? Is_admin { get; set; }
        public string? BudgetId { get; set; }
    }
}
