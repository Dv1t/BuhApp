using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CloudBuh.Entities;
using CloudBuh.Item;

namespace CloudBuh
{
    public static class RestService
    {
        private static readonly string apiPath = "api/transactions";
        public static readonly HttpClient Client = new HttpClient();

        public static async Task<TransactionEntity> AddTransaction(double amount, string description, bool plus, DateTime dateOfTransaction)
        {
            var entity = new
            {
                Value = amount,
                Description = description,
                Plus = plus,
                DateOfTransaction = dateOfTransaction
            };
            HttpResponseMessage response = await Client.PostAsJsonAsync(
                apiPath, entity);
            if (response.StatusCode==System.Net.HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<TransactionEntity>();
            else
                return null;
        }

        public static async Task<IEnumerable<TransactionListItem>> GetTransactionsAsync()
        {
            HttpResponseMessage response = await Client.GetAsync(apiPath);
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<TransactionEntity> result = await response.Content.ReadFromJsonAsync<IEnumerable<TransactionEntity>>();
                return result.Select(item=> new TransactionListItem(item));
            }
            return new List<TransactionListItem>();
        }

        public static async Task<bool>DeleteTransaction(TransactionListItem item)
        {
            HttpResponseMessage response = await Client.DeleteAsync($"{apiPath}/{item.Id}");
            return response.StatusCode==System.Net.HttpStatusCode.OK;
        }
    }
}
