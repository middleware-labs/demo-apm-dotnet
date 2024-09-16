using MW_WebApp_Nuget.Model;

namespace MW_WebApp_Nuget.Repository.Interface
{
    public interface IPersonsRepository
    {
        Task<IEnumerable<Person>> GetAllPersonsData_SP();
        Task<IEnumerable<Person>> GetAllPersonsData_SQL();
    }
}
