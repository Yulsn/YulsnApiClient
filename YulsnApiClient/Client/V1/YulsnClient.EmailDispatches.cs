using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnReadEmailDispatchDto> CreateEmailDispatchAsync(YulsnCreateEmailDispatchDto yulsnCreateEmailDispatchDto) =>
            SendAsync<YulsnReadEmailDispatchDto>(new HttpRequestMessage(HttpMethod.Post, "api/v1/EmailDispatches")
            {
                Content = JsonContent(yulsnCreateEmailDispatchDto)
            });

        public Task<string> CreateEmail2DispatchAsync(YulsnCreateEmail2DispatchDto yulsnCreateEmail2DispatchDto) =>
            SendAsync<string>(new HttpRequestMessage(HttpMethod.Post, "api/v1/Email2Dispatches")
            {
                Content = JsonContent(yulsnCreateEmail2DispatchDto)
            });

        public Task<string> SendSingleEmail2DispatchAsync(string triggerId, string email, params (string key, object value)[] dynamic) =>
            CreateEmail2DispatchAsync(new YulsnCreateEmail2DispatchDto
            {
                TriggerId = triggerId,
                Scope = new YulsnEmail2ScopeDto
                {
                    Emails = new Dictionary<string, YulsnEmail2ContactSettingsDto>
                    {
                        [email] = new YulsnEmail2ContactSettingsDto
                        {
                            Dynamic = dynamic.ToDictionary(kvp => kvp.key, kvp => kvp.value)
                        }
                    }
                }
            });

        public Task<string> SendSingleEmail2DispatchAsync(string triggerId, int contactId, params (string key, object value)[] dynamic) =>
            CreateEmail2DispatchAsync(new YulsnCreateEmail2DispatchDto
            {
                TriggerId = triggerId,
                Scope = new YulsnEmail2ScopeDto
                {
                    Contacts = new Dictionary<int, YulsnEmail2ContactSettingsDto>
                    {
                        [contactId] = new YulsnEmail2ContactSettingsDto
                        {
                            Dynamic = dynamic.ToDictionary(kvp => kvp.key, kvp => kvp.value)
                        }
                    }
                }
            });
    }
}