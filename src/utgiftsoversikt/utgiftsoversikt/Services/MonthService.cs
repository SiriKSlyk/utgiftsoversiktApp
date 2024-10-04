using utgiftsoversikt.Models;
using utgiftsoversikt.Repos;


namespace utgiftsoversikt.Services
{
    public interface IMonthService
    {
        bool Create(Month month);
        Month GetById(string id);
        Month GetByUserIdAndMonth(string userId, string month);
        List<Month> GetAll(string userId);
        List<Month> GetAllInYear(string userId, string year);
        bool Update(Month month);
        bool Delete(Month month);

    }
    public class MonthService : IMonthService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMonthRepo _monthRepo;

        public MonthService(IMonthRepo monthRepo, IUserRepo userRepo)
        {
            _monthRepo = monthRepo;
            _userRepo = userRepo;
        }

        public bool Create(Month month)
        {
            _monthRepo.Create(month);
            return _monthRepo.Write().Result;
        }
        public Month GetById(string id)
        {
            return _monthRepo.GetById(id);
        }

        public Month GetByUserIdAndMonth(string userId, string month)
        {
            return _monthRepo.GetByUserIdAndMonth(userId, month);
        }
        public List<Month> GetAll(string userId)
        {
            return _monthRepo.GetAll(userId);
        }
        public List<Month> GetAllInYear(string userId, string year)
        {
            int yearInt = int.Parse(year);
           // Checks if year is valid
            if (1950 <= yearInt && yearInt <= 2024) {
                return _monthRepo.GetAllInYear(userId, year);
            }
            return new List<Month>();

        }
        public bool Update(Month month)
        {
            _monthRepo.RemoveTrace(month);
            _monthRepo.Update(month);

            return _monthRepo.Write().Result;
        }
        public bool Delete(Month month)
        { 
            _monthRepo.RemoveTrace(month);
            _monthRepo.Delete(month);
            return _monthRepo.Write().Result;
        }
    }
}