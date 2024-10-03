
using System.Diagnostics.Eventing.Reader;
using utgiftsoversikt.Models;
using utgiftsoversikt.Repos;


namespace utgiftsoversikt.Services
{
    public interface IUserService
    {
        bool CreateUser(User user);
        List<User> FindAllUsers();
        User GetUserById(string id);
        User GetUserByEmail(string email);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool IdExist(string id);
        bool EmailExist(string email);

    }
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;


        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;

        }

        public bool CreateUser(User user)
        {

            user = new User() { Id = Guid.NewGuid().ToString(), First_name = user.First_name, Last_name = user.Last_name, Email = user.Email, Is_admin = false };
            _userRepo.AddUser(user);
            return _userRepo.Write().Result;

        }

        public List<User> FindAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public User GetUserById(string id)
        {
            return _userRepo.GetUserById(id);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepo.GetUserByEmail(email).Result;
        }

        public bool UpdateUser(User user)
        {
            var oldUser = _userRepo.GetUserById(user.Id);

            // Forcing values not to change
            user.Id = oldUser.Id;
            user.Is_admin = oldUser.Is_admin;
            user.BudgetId = oldUser.BudgetId;
            _userRepo.UpdateUserByUser(user);

            return _userRepo.Write().Result;
        }

        public bool DeleteUser(User user)
        {
            _userRepo.DeleteUser(user);
            return _userRepo.Write().Result;
        }
        public bool UserExist(User user)
        {
            return _userRepo.IdExist(user.Id) && _userRepo.EmailExist(user.Email);
        }
        public bool IdExist(string id)
        {
            return _userRepo.IdExist(id);
        }
        public bool EmailExist(string email)
        {

            return _userRepo.EmailExist(email);

        }
    }
}