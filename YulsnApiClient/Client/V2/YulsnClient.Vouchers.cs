using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using YulsnApiClient.Models.V2;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>
        /// Returns a voucher group
        /// </summary>
        /// param name="voucherGroupId">Id of VoucherGroup</param>
        /// <returns></returns>
        public Task<T> GetVoucherGroupByIdAsync<T>(int voucherGroupId) where T : YulsnVoucherGroup =>
            SendAsync<T>(HttpMethod.Get, $"api/v2/{AccountId}/VoucherGroups/{voucherGroupId}", YulsnApiVersion.V2);

        /// <summary>
        /// Returns all active voucher groups
        /// </summary>
        /// <returns></returns>
        public Task<List<T>> GetActiveVoucherGroupsAsync<T>() where T : YulsnVoucherGroup =>
            SendAsync<List<T>>(HttpMethod.Get, $"api/v2/{AccountId}/VoucherGroups/Active", YulsnApiVersion.V2);

        /// <summary>
        /// Returns all active vouchers of a voucher group
        /// </summary>
        /// <param name="voucherGroupId">Id of VoucherGroup</param>
        /// <returns></returns>
        public Task<List<T>> GetActiveVouchersFromVoucherGroupAsync<T>(int voucherGroupId) where T : YulsnVoucher =>
            SendAsync<List<T>>(HttpMethod.Get, $"api/v2/{AccountId}/VoucherGroups/{voucherGroupId}/Vouchers/Active", YulsnApiVersion.V2);

        /// <summary>
        /// Returns a voucher of a voucher group
        /// </summary>
        /// <param name="voucherGroupId">Id of VoucherGroup</param>
        /// <param name="voucherId">Id of Voucher</param>
        /// <returns></returns>
        public Task<T> GetVoucherFromVoucherGroupAsync<T>(int voucherGroupId, int voucherId) where T : YulsnVoucher =>
            SendAsync<T>(HttpMethod.Get, $"api/v2/{AccountId}/VoucherGroups/{voucherGroupId}/Vouchers/{voucherId}", YulsnApiVersion.V2);

        /// <summary>
        /// Assigns a voucher in a group to a contact
        /// </summary>
        /// <returns></returns>
        public Task<YulsnPostVoucherAssignmentResponse> AssignVoucherToContactAsync(YulsnPostVoucherAssignmentRequest request) =>
            SendAsync<YulsnPostVoucherAssignmentResponse>(HttpMethod.Post, $"api/v2/{AccountId}/VoucherAssignments", request, YulsnApiVersion.V2);

        /// <summary>
        /// Updates the status on a voucher assignment
        /// </summary>
        /// <returns></returns>
        public Task UpdateVoucherAssignmentAsync(YulsnPatchVoucherAssignmentRequest request) =>
            SendAsync<object>(new HttpMethod("PATCH"), $"api/v2/{AccountId}/VoucherAssignments", request, YulsnApiVersion.V2);

        /// <summary>
        /// Returns the vouchers and voucher codes of all voucher assingments of a contact
        /// </summary>
        /// <param name="contactId">Id of Contact</param>
        /// <returns></returns>
        public Task<List<YulsnVoucherAssignment>> GetVoucherAssignmentsByContactAsync(int contactId) =>
            SendAsync<List<YulsnVoucherAssignment>>(HttpMethod.Get, $"api/v2/{AccountId}/VoucherAssignments/Contact/{contactId}", YulsnApiVersion.V2);

        /// <summary>
        /// Returns the voucher and voucher code of the voucher assingment by a contact
        /// </summary>
        /// <param name="contactId">Id of Contact</param>
        /// <param name="voucherAssignmentId">Id of Vouucher assignment</param>
        /// <returns></returns>
        public Task<YulsnVoucherAssignment> GetVoucherAssignmentByContactAsync(int contactId, int voucherAssignmentId) =>
            SendAsync<YulsnVoucherAssignment>(HttpMethod.Get, $"api/v2/{AccountId}/VoucherAssignments/Contact/{contactId}/{voucherAssignmentId}", YulsnApiVersion.V2);

        /// <summary>
        /// Returns voucher assingment of a voucher code
        /// </summary>
        /// <param name="code">VoucherCode</param>
        /// <returns></returns>
        public Task<YulsnVoucherAssignment> GetVoucherAssignmentByCodeAsync<T>(string code) =>
            SendAsync<YulsnVoucherAssignment>(HttpMethod.Get, $"api/v2/{AccountId}/VoucherAssignments/VoucherCode/{code}", YulsnApiVersion.V2);

        /// <summary>
        /// Creates and starts a bulk import of voucher codes.
        /// Use a link ('FileUrl' as string) or the file directly ('File' as byte[]) for the import file. This file is then kept, respecting the account's delete policy.
        /// </summary>
        /// <returns></returns>
        public Task<YulsnVoucherCodeImportResponse> ImportVoucherCodesAsync(YulsnVoucherCodeImportRequest request) =>
            SendAsync<YulsnVoucherCodeImportResponse>(HttpMethod.Post, $"api/v2/{AccountId}/VoucherCodes/Import", request, YulsnApiVersion.V2);
    }
}