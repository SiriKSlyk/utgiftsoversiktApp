namespace utgiftsoversikt.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email {  get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public bool Is_admin { get; set; } = false;
        public string BudgetId { get; set; } // Ref to the current budget in use


    }

    
}