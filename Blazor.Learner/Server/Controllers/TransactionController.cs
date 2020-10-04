using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Learner.Server.Data;
using Blazor.Learner.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Learner.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TransactionController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpPost("{accountId}")]
        public async Task<IActionResult> Post(int accountId, Transaction transaction)
        {
            Account account = _context.Accounts.Single(account => account.Id == accountId);
            List<Balance> balanses =_context.Balances.Where(b => 
            b.BalanceDate <= transaction.TransactionDate && b.Account.Id == account.Id).ToList();

            Balance balanseLatest = _context.Balances.Where(b =>
             b.BalanceDate == transaction.TransactionDate && b.Account.Id == account.Id).SingleOrDefault();


            if (balanses.Count != 0)
            {
                balanses.ForEach(b => { b.BalanceAmount += transaction.TransactionAmount;
                    BalanceTransaction balanceTransaction = new BalanceTransaction
                    {
                        Balance = b,
                        Transaction = transaction,
                        BalanceId = b.Id,
                        TransactionId = transaction.Id

                      };
                    if(b.BalanceTransactions == null)
                    {
                        b.BalanceTransactions = new List<BalanceTransaction>();
                    }
                    b.BalanceTransactions.Add(balanceTransaction);
                    _context.Entry(b).State = EntityState.Modified;
                });
            }

            if(balanseLatest == null)
            {

                var balance = new Balance
                {
                    Account = account,
                    BalanceAmount = transaction.TransactionAmount,
                    AccountNumber = account.Id,
                    BalanceDate = transaction.TransactionDate
                };
                BalanceTransaction balanceTransaction = new BalanceTransaction
                {
                    Balance = balance,
                    Transaction = transaction,
                    BalanceId = balance.Id,
                    TransactionId = transaction.Id

                };
                if (balance.BalanceTransactions == null)
                {
                    balance.BalanceTransactions = new List<BalanceTransaction>();
                }
                balance.BalanceTransactions.Add(balanceTransaction);
                if (account.Balances == null)
                {
                    account.Balances = new List<Balance>();
                }
                account.Balances.Add(balance);
                _context.Attach(account);
            }
            await _context.SaveChangesAsync();
            return Ok(account.Id);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var acc = new Account { Id = id };
            _context.Remove(acc);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}