using MW_WebApp_Nuget.Model;
using MW_WebApp_Nuget.Data;
using MW_WebApp_Nuget.Repository.Interface;
using System.Data;
using Dapper;

namespace MW_WebApp_Nuget.Repository
{
    public class PersonsRepository: IPersonsRepository
    {
        private readonly DapperContext _context;
        public PersonsRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsData_SP()
        {
            using (var connection = this._context.CreateDBConnection())
            { 
                //Execute stored procedure and map the returned result to a Customer object  
                var response = await connection.QueryAsync<Person>("GetAllPersons", commandType: CommandType.StoredProcedure);
                return response;
            }
        }
        
        public async Task<IEnumerable<Person>> GetAllPersonsData_SQL()
        {
            using (var connection = this._context.CreateDBConnection())
            { 
                //Execute stored procedure and map the returned result to a Customer object  
                var response = await connection.QueryAsync<Person>("SELECT * FROM Person", commandType: CommandType.Text);
                return response;
            }
        }
    }
}
