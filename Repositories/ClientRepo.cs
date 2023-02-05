using ASP.Net_MVC_Assignment.Data;
using ASP.Net_MVC_Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.Net_MVC_Assignment.Repositories
{

    public class ClientRepo
    {
        ApplicationDbContext _db;

        public ClientRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Add new client record
        /// 
        /// 1. Used this method when a user registers website
        /// </summary>
        /// <param name="client"></param>

        public void AddClient(Client client)
        {
            _db.Clients.Add(client);
            _db.SaveChanges();
        }

        /// <summary>
        /// Get client's information based on client's email
        /// 
        /// 1. Used this method when a user registers or login
        /// 2. Returns clients data based on matched client's email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Client</returns>
        public Client GetClientId(string email)
        {
            var clients = _db.Clients.Where(c => c.Email == email).FirstOrDefault();

            return clients;
        }

    }
}