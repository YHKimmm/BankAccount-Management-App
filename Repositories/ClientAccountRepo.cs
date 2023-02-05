using ASP.Net_MVC_Assignment.Data;
using ASP.Net_MVC_Assignment.Models;
using ASP.Net_MVC_Assignment.ViewModels;

namespace ASP.Net_MVC_Assignment.Repositories
{
    public class ClientAccountRepo
    {
        ApplicationDbContext _db;

        public ClientAccountRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Get the client account records
        /// 
        /// 1. Joining two table such as bank account table and client table
        /// 2. Looping through joined client account view model data then return it
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<ClientAccountVM></returns>
        public List<ClientAccountVM> GetRecords(int id)
        {
            List<ClientAccountVM> vm = new List<ClientAccountVM>();

            var clientAccounts =
                           _db.BankAccounts
                           .Join(_db.ClientAccounts, b => b.AccountNum, ca => ca.AccountNum,
                           (b, ca) => new {
                               ClientId = ca.ClientID,
                               AccountNum = b.AccountNum,
                               AccountType = b.AccountType,
                               Balance = b.Balance
                           })
                           .Join(_db.Clients, ba => ba.ClientId, c => c.ClientID,
                           (ba, c) => new {
                               AccountNum = ba.AccountNum,
                               LastName = c.LastName,
                               FirstName = c.FirstName,
                               AccountType = ba.AccountType,
                               Email = c.Email,
                               ClientId = c.ClientID,
                               Balance = ba.Balance
                           }).Where(c => c.ClientId == id);


            foreach (var clientAccount in clientAccounts)
            {

                vm.Add(new ClientAccountVM
                {
                    ClientID = clientAccount.ClientId,
                    FirstName = clientAccount.FirstName,
                    LastName = clientAccount.LastName,
                    AccountNum = clientAccount.AccountNum,
                    AccountType = clientAccount.AccountType,

                });
            }

            return vm;
        }

        /// <summary>
        /// Get client account detail
        /// 
        /// 1. Pass this returned view model data to the detail and edit view
        /// 2. Populates each view model's attributes based on matched bank account data and client data
        /// 3. Matched bank account data and client data is validated by account number and client id
        /// </summary>
        /// <param name="accountNum"></param>
        /// <param name="clientId"></param>
        /// <returns>ClientAccountDetailEditVM</returns>
        public ClientAccountDetailEditVM GetDetail(int accountNum, int clientId)
        {

            var bankAccounts = _db.BankAccounts.Where(ba => ba.AccountNum == accountNum).FirstOrDefault();
            var clients = _db.Clients.Where(c => c.ClientID == clientId).FirstOrDefault();

            ClientAccountDetailEditVM vm = new ClientAccountDetailEditVM
            {
                AccountNum = bankAccounts.AccountNum,
                LastName = clients.LastName,
                FirstName = clients.FirstName,
                Email = clients.Email,
                AccountType = bankAccounts.AccountType,
                Balance = bankAccounts.Balance,
                ClientID = clients.ClientID
            };
       

            return vm;
        }

        /// <summary>
        /// Creates client account records
        /// 
        /// 1. Populates new client account table's attributes for create
        /// 2. Add the populated client account data
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="bankAccountNum"></param>

        public void CreateClientAccountRecord(int clientID, int bankAccountNum)
        {
           ClientAccount clientAccount = new ClientAccount()
           {
               ClientID = clientID,
               AccountNum = bankAccountNum
           };
           
            _db.Add(clientAccount);
            _db.SaveChanges();
        }


        /// <summary>
        /// Delete client account record
        /// 
        /// 1. Populates new client account table's attributes for delete
        /// 2. Remove the populated client account data
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="accountNum"></param>
        public void DeleteClientAccountRecord(int clientID , int accountNum)
        {

            ClientAccount clientAccount = new ClientAccount()
            {
                ClientID = clientID,
                AccountNum = accountNum
            };

            _db.Remove(clientAccount);
            _db.SaveChanges();
        }

    }
}
