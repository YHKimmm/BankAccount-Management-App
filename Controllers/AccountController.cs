using ASP.Net_MVC_Assignment.Data;
using ASP.Net_MVC_Assignment.Repositories;
using ASP.Net_MVC_Assignment.Utilities;
using ASP.Net_MVC_Assignment.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.Net_MVC_Assignment.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext _db;

        public AccountController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index(string message, string sortOrder, string searchString, string currentFilter, int? page)
        {

            if (message == null)
            {
                message = "";
            }

            ViewData["Message"] = message;

            ListSort listSort = new ListSort(_db);

            int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            if (ViewData["UserName"] == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                ViewData["numberSortParm"] = "number_desc";
            }
            else
            {
                ViewData["numberSortParm"] = sortOrder == "Number" ?
                                                        "number_desc" : "Number";
            }

            ViewData["typeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // This two View data used in Index Razor View in order to avoid crash once user sorted item and go to the next or previous page

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            var sortRecords = listSort.GetListsBySorting(sortOrder, clientID, searchString, currentFilter, page);

            var source = sortRecords.AsQueryable();

            int pageSize = 4;

            return View(PaginatedList<ClientAccountVM>.Create(source.AsNoTracking(), page ?? 1, pageSize));

        }

        public IActionResult Details(int id, string? message)
        {
            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);

            int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));

            ClientAccountDetailEditVM vm = clientAccountRepo.GetDetail(id, clientID);

            if (message == null)
            {
                message = "";
            }

            vm.Message = message;

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            return View(vm);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            ViewData["AccountType"] = new SelectList(_db.BankAccountTypes, "AccountType", "AccountType");

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            return View();
        }

        /// <summary>
        /// Create bank account record
        /// 
        /// 1. Redirects to the detail page with the message and the matched account number
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns>IActionResult</returns>
        [HttpPost] // POST: Account/Create
        public IActionResult Create(BankAccountVM bankAccountVM)
        {

            int accountNumber = 0;

            string createMessage = "";

            int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));

            // Ensure data is valid.
            if (ModelState.IsValid)
            {
                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);

                Tuple<int, string> createBankRecrod =
                    bankAccountRepo.CreateBankRecord(bankAccountVM, clientID);

                accountNumber = createBankRecrod.Item1;
                createMessage = createBankRecrod.Item2;

            }
            return RedirectToAction("Details", "Account",
               new { id = accountNumber, message = createMessage });

        }
        public IActionResult Edit(int id)
        {
            ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);

            int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));

            ClientAccountDetailEditVM vm = clientAccountRepo.GetDetail(id, clientID);

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");

            ViewData["AccountType"] = new SelectList(_db.BankAccountTypes, "AccountType", "AccountType");

            return View(vm);
        }

        /// <summary>
        /// Edit bank account record
        /// 
        /// 1. Redirects to the detail page with the message and the matched account number
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public IActionResult Edit(BankAccountVM bankAccountVM)
        {
            string updateMessage = "";
            int accountNumber = 0;

            // Ensure data is valid.
            if (ModelState.IsValid)
            {

                BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);

                Tuple<int, string> editBankRecord = bankAccountRepo.EditBankRecord(bankAccountVM);

                accountNumber = editBankRecord.Item1;
                updateMessage = editBankRecord.Item2;

            }

            return RedirectToAction("Details", "Account",
                 new { id = accountNumber, message = updateMessage });
        }

        public IActionResult Delete(int id)
        {

            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);

            BankAccountVM bankAccountVM = bankAccountRepo.GetBankAccount(id);


            if (bankAccountVM != null)
            {
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            }

            return View(bankAccountVM);
        }

        /// <summary>
        /// Deletes bank account record
        /// 
        /// 1. Deletes bank account record based on the matched account number and client id
        /// 2. Redirects to the index page with the message
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(BankAccountVM bankAccountVM)
        {
            string deleteMessage;
            BankAccountRepo bankAccountRepo = new BankAccountRepo(_db);

            int clientID = Convert.ToInt32(HttpContext.Session.GetString("ClientID"));

            deleteMessage = bankAccountRepo.DeleteBankRecord(bankAccountVM.AccountNum, clientID);

            return RedirectToAction("Index", "Account",
                  new { message = deleteMessage });
        }

    }

    /// <summary>
    /// Creates Pagination on the Index Page
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex,
                        int pageSize)
        {
            var count = source.Count();
            var items =
                source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
