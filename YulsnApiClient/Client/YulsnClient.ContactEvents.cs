using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models;
using System.Net.Http;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<List<T>> GetContactEvents<T>(string type, int lastId) where T : YulsnReadContactEvent =>
            SendAsync<List<T>>($"api/v1/ContactEvents?type={type}&lastId={lastId}");

        public Task<List<T>> GetContactEvents<T>(string type, string subtype = null, DateTimeOffset? from = null, DateTimeOffset? to = null, int? contactId = null) where T : YulsnReadContactEvent
        {
            StringBuilder url = new StringBuilder($"api/v1/ContactEvents?type={type}");

            if (subtype != null)
                url.Append($"&subtype={subtype}");

            if (from != null)
                url.Append($"&from={from}");

            if (to != null)
                url.Append($"&to={to}");

            if (contactId != null)
                url.Append($"&contactId={contactId}");

            return SendAsync<List<T>>(url.ToString());
        }

        public Task CreateContactEvent<T>(List<T> yulsnCreateContactEvents, bool ignoreUnknownContacts = false) where T : YulsnCreateContactEvent =>
            SendAsync<object>(HttpMethod.Post, $"api/v1/ContactEvents?ignoreUnknownContacts={ignoreUnknownContacts}", yulsnCreateContactEvents);
    }
}