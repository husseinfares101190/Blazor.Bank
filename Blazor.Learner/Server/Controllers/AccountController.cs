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
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public AccountController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return Ok(accounts);
        }

        [HttpGet("balancedates/{id}")]
        public async Task<IActionResult> GetBalanceDates(int id)
        {
            List<DateTime> dates = await _context.Balances.Where(b => b.AccountNumber == id).Select(b => b.BalanceDate).ToListAsync();
            return Ok(dates);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Account account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
            return Ok(account);
        }


        [HttpGet("balances/{accountId}")]
        public async Task<IActionResult> GetBalances(int accountId)
        {
            Balance [] balances = await _context.Balances.Where(b => b.AccountNumber == accountId).ToArrayAsync();
            return Ok(balances);
        }


        [HttpGet("transactions/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            int [] transactionsId =  (from e in _context.Transactions
                                 join d in _context.BalanceTransactions on e.Id equals d.TransactionId 
                                 join i in _context.Balances on d.BalanceId equals i.Id  
                                 where i.AccountNumber == accountId
                                 select e.Id).ToArray() ;

            Transaction [] transactions = _context.Transactions.Where(t => transactionsId.Contains(t.Id)).ToArray();
                
            return Ok(transactions);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Account account)
        {
            _context.Add(account);
            await _context.SaveChangesAsync();
            return Ok(account.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Account account)
        {
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var acc = new Account { Id = id };
            _context.Remove(acc);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("cancleUpdates")]
        public async Task<IActionResult> PostCancelUpdates(Account account)
        {
            // For production
            var accountEntry = _context.Entry(account);
            accountEntry.CurrentValues.SetValues(accountEntry.OriginalValues);
            accountEntry.State = EntityState.Unchanged;
            return Ok(account.Id);
        }

    }
}