using Blazor.Learner.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Learner.Client.Services
{
    public interface IBalanceService
    {
       public Task<HttpResponseMessage> CreateBalance(Balance balance);

        public Task<HttpResponseMessage> EditBalance(Balance balance);
    }
}
