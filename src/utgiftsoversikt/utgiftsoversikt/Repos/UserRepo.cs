using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.EntityFrameworkCore;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;

namespace utgiftsoversikt.Repos
{
    public interface IUserRepo
    {
        bool AddUser(User user);
        List<User> GetAllUsers();
        User GetUserById(string id);
        bool IdExist(string id);
        bool EmailExist(string email);
        bool DeleteUser(User user);
        bool UpdateUserByUser(User newUser);
        Task<User> GetUserByEmail(string email);
        void RemoveTrace(User user);
        Task<bool> Write();

    }
    
    
    public class UserRepo: IUserRepo
    {

        private readonly CosmosContext _context;

        public UserRepo(CosmosContext context)
        {
            _context = context;
        }

        // Creates and give user a new uniqe id
        public bool AddUser(User user)
        {
            
            _context.Users.Add(user);
            return Write().Result;
        }

        public List<User> GetAllUsers()
        {

            var users = _context.Users.ToList();
            return users;
        }

        public User GetUserById(string id)
        {
            var user = _context.Users?.FirstOrDefault(u => u.Id == id);

            return user;

        }

        // Endres senere til et unikt felt
        public async Task<User> GetUserByEmail(string name)
        {
            /*using var client = new CosmosClientBuilder("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;DisableServerCertificateValidation=true").WithLimitToEndpoint(true).Build();
            await client.CreateDatabaseIfNotExistsAsync("cosmos");*/
            var user = _context.Users?.FirstOrDefault(u => u.Email.ToLower() == name.ToLower());
            //var user = Database.users.First();

            return user;
        }

        public bool UpdateUserByUser(User user)
        {

            RemoveTrace(user);

            _context.Users?.Update(user);
            return Write().Result;

        }

        public bool DeleteUser(User user)
        {
            RemoveTrace(user);

            _context.Users?.Remove(user);
            return Write().Result;
        }

        public bool EmailExist(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            return user != null;

        }

        public bool IdExist(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            return user != null;

        }
        public void RemoveTrace(User user)
        {
            var trackedUser = _context.ChangeTracker.Entries<User>()
            .FirstOrDefault(e => e.Entity.Id == user.Id);
            
            if (trackedUser != null)
            {
                // Fjern den eksisterende sporing
                _context.Entry(trackedUser.Entity).State = EntityState.Detached;
            }
        }
        public async Task<bool> Write()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
