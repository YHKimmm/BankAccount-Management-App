using ASP.Net_MVC_Assignment.Data;
using ASP.Net_MVC_Assignment.Models;
using ASP.Net_MVC_Assignment.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.Net_MVC_Assignment.Repositories
{
    public class BankAccountRepo
    {
        ApplicationDbContext _db;

        public BankAccountRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Creates bank account record with bank account view model
        /// 
        /// 1. Save bank account with populating each view model's attributes and create client account record
        /// 2. Returns bank account number and message once the bank account is created
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <param name="clientID"></param>
        /// <returns> Tuple<int,string> </returns>
        public Tuple<int,string> CreateBankRecord(BankAccountVM bankAccountVM, int clientID)
        {
            string createMessage;

            BankAccount bankAccount = new BankAccount
            {
                AccountType = bankAccountVM.AccountType,
                Balance = bankAccountVM.Balance

            };

            try
            {
                _db.Add(bankAccount);
                _db.SaveChanges();
                ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);
                clientAccountRepo.CreateClientAccountRecord(clientID, bankAccount.AccountNum);

                createMessage = $"Success creating your {bankAccount.AccountType} account, your new account number is {bankAccount.AccountNum}";

            }
            catch (Exception ex)
            {
                createMessage = ex.Message;

            }
            return Tuple.Create(bankAccount.AccountNum, createMessage);
        }

        /// <summary>
        /// Edits bank account record with bank account view model
        /// 
        /// 1.Updates bank account with populating each view model's attributes
        /// 2. Returns bank account number and message once the bank account is updated
        /// </summary>
        /// <param name="bankAccountVM"></param>
        /// <returns> Tuple<int,string> </returns>
        public Tuple<int,string> EditBankRecord(BankAccountVM bankAccountVM)
        {
            string updateMessage;
            BankAccount bankAccount = new BankAccount
            {
                AccountNum = bankAccountVM.AccountNum,
                AccountType = bankAccountVM.AccountType,
                Balance = bankAccountVM.Balance

            };

            try
            {
                _db.Update(bankAccount);
                _db.SaveChanges();
 
                updateMessage = $"Success editing {bankAccount.AccountType} account No.{bankAccount.AccountNum}";
            }
            catch (Exception ex)
            {
                updateMessage = ex.Message;
            }
            return Tuple.Create(bankAccount.AccountNum, updateMessage);
        }

        /// <summary>
        /// Get the bank accounts with the same bank account number when it is called in Delete GET method in Controller
        /// 
        /// Populates each bank accunt view model's attributes based on the matched bank account number
        /// 
        /// </summary>
        /// <param name="accountNum"></param>
        /// <returns>BankAccountVM</returns>
        public BankAccountVM GetBankAccount(int accountNum)
        {
            var bankAccounts = _db.BankAccounts.Where(ba => ba.AccountNum == accountNum).FirstOrDefault();

            BankAccountVM bankAccountVM = new BankAccountVM
            {
                AccountNum = bankAccounts.AccountNum,
                AccountType = bankAccounts.AccountType,
                Balance = bankAccounts.Balance,
            };

            return bankAccountVM;
        }

        /// <summary>
        /// Deletes bank account record 
        /// 
        /// 1.Remove client account record(bridge table) first then remove bank accounts based on matched bank account number
        /// 2. Returns message when the bank account is deleted
        /// </summary>
        /// <param name="accountNum"></param>
        /// <param name="clientID"></param>
        /// <returns>deleteMessage(string)</returns>

        public string DeleteBankRecord(int accountNum, int clientID)
        {

            string deleteMessage;

            var bankAccounts = _db.BankAccounts.Where(ba => ba.AccountNum == accountNum).FirstOrDefault();

            try
            {
                ClientAccountRepo clientAccountRepo = new ClientAccountRepo(_db);
                clientAccountRepo.DeleteClientAccountRecord(clientID, bankAccounts.AccountNum);
                _db.Remove(bankAccounts);
                _db.SaveChanges();

                deleteMessage = $"Success deleting {bankAccounts.AccountType} account No.{bankAccounts.AccountNum}";
            }
            catch (Exception ex)
            {

                deleteMessage = ex.Message;
            }
            return deleteMessage;
        }

    }
}
