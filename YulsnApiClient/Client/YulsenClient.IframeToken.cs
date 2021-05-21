using System.Threading.Tasks;
using YulsnApiClient.Models;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        public Task<YulsnReadIframeToken> GetIframeTokenAsync(string tokenId) =>
            SendAsync<YulsnReadIframeToken>($"/api/v1/IframeToken?tokenId={tokenId}");
    }
}
