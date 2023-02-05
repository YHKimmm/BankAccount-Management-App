using ASP.Net_MVC_Assignment.Data;
using ASP.Net_MVC_Assignment.Repositories;
using ASP.Net_MVC_Assignment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.Net_MVC_Assignment.Utilities
{
    public class ListSort
    {
        ApplicationDbContext _db;

        public ListSort(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Sorting account information based on account number and account type
        /// 
        /// 1.Sort out the account number and account type
        /// 2.Each sorted items could be descending or ascending
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="id"></param>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="page"></param>
        /// <returns>List<ClientAccountVM></returns>

        public List<ClientAccountVM> GetListsBySorting(string sortOrder, int id, string searchString, string currentFilter, int? page)
        {
            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);

            var lists = clientAccountRepo.GetRecords(id);

            if (!String.IsNullOrEmpty(searchString))
            {
                lists = lists.Where(l => l.AccountType.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "Type":
                    lists = lists.OrderBy(l => l.AccountType).ToList();
                    break;
                case "number_desc":
                    lists = lists.OrderByDescending(l => l.AccountNum).ToList();
                    break;
                case "type_desc":
                    lists = lists.OrderByDescending(l => l.AccountType).ToList();
                    break;
                default:
                    lists = lists.OrderBy(l => l.AccountNum).ToList();
                    break;
            }

            return lists;
        }


    }
}
