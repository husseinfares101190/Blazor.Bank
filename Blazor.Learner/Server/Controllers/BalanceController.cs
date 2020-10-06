using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class BalanceController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public BalanceController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Balance balance)
        {
            balance.AccountNumber = balance.Account.Id;
            _context.Add(balance);
            Account account = balance.Account;
            account.Balances.Add(balance);
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(balance.Id);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put(Balance balance)
        {

            _context.Entry(balance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(balance.Id);
        }

    }
}
