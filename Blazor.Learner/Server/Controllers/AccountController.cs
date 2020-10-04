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
            var devs = await _context.Accounts.ToListAsync();
            return Ok(devs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dev = await _context.Accounts.FirstOrDefaultAsync(a=>a.Id == id);
            return Ok(dev);
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