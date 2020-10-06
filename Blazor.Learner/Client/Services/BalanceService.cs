using Blazor.Learner.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Blazor.Learner.Client.Services
{
    public class BalanceService : IBalanceService
    {

        private readonly HttpClient _httpClient;

        public BalanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
         async Task<HttpResponseMessage> IBalanceService.CreateBalance(Balance balance)
        {

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/balance", balance);
            return response;
           

        }

        async Task<HttpResponseMessage> IBalanceService.EditBalance(Balance balance)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("api/balance/edit", balance);
            return response;
        }
    }
}
