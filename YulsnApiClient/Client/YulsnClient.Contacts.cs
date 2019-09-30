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
        public Task<YulsnContact> GetContact(string secret)
        {
            string url = "/api/v1/Contacts?secret=" + secret;

            return sendAsync<YulsnContact>(url);
        }

        public Task DeleteContact(int contactId)
        {
            string url = "/api/v1/Contacts/" + contactId;

            return sendAsync<object>(HttpMethod.Delete, url);
        }
    }
}