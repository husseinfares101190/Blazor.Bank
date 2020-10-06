using Blazor.Learner.Server.Data;
using Blazor.Learner.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accounts = await _context.Transactions.ToListAsync();
            return Ok(accounts);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Transaction transaction = await _context.Transactions.FirstOrDefaultAsync(tran => tran.Id == id);
            List<BalanceTransaction> affectedBalances = await _context.BalanceTransactions.Where(bt => bt.TransactionId == transaction.Id).ToListAsync();
            affectedBalances.ForEach(b =>{
                Balance balance = _context.Balances.FirstOrDefault(bal => bal.Id == b.BalanceId);
                balance.BalanceAmount -= transaction.TransactionAmount;
                _context.Entry(balance).State = EntityState.Modified;
                b.Balance = null;
                _context.Entry(b).State = EntityState.Modified;
            });
            _context.Remove(transaction);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpPost("{accountId}")]
        public async Task<IActionResult> Post(int accountId, Transaction transaction)
        {
            Account account = _context.Accounts.Single(account => account.Id == accountId);
            List<Balance> balanses = _context.Balances.Where(b =>
             b.BalanceDate <= transaction.TransactionDate && b.Account.Id == account.Id).ToList();
            if (balanses.Count != 0)
            {
                balanses.ForEach(b =>
                {
                    b.BalanceAmount += transaction.TransactionAmount;
                    BalanceTransaction balanceTransaction = new BalanceTransaction
                    {
                        Balance = b,
                        Transaction = transaction,
                        BalanceId = b.Id,
                        TransactionId = transaction.Id

                    };
                    if (b.BalanceTransactions == null)
                    {
                        b.BalanceTransactions = new List<BalanceTransaction>();
                    }
                    b.BalanceTransactions.Add(balanceTransaction);
                    _context.Entry(b).State = EntityState.Modified;
                });
            }

            _context.Attach(account);
            await _context.SaveChangesAsync();
            return Ok(account.Id);
        }


    }
}