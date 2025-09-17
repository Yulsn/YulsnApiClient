using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V2;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class VoucherTests(Setup setup) : IClassFixture<Setup>
    {
        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task Get_Ok_WhenManage()
        {
            /* 
             * Requirements:
             * - there must be at least one active voucher group with at least one active group voucher
             * - the group voucher must have enough time to assign (ex: 24 hours)
             * 
             * Tested endpoints:
             * -GET     /api/v2/{accountId}/VoucherGroups/{voucherGroupId}
             * -GET     /api/v2/{accountId}/VoucherGroups/Active
             * -GET     /api/v2/{accountId}/VoucherGroups/{voucherGroupId}/Vouchers/{voucherId}
             * -GET     /api/v2/{accountId}/VoucherGroups/{voucherGroupId}/Vouchers/Active
             * -POST    /api/v2/{accountId}/VoucherCodes/Import
             * -POST    /api/v2/{accountId}/VoucherAssignments
             * -PATCH   /api/v2/{accountId}/VoucherAssignments
             * -GET     /api/v2/{accountId}/VoucherAssignments/Contact/{contactId}
             * -GET     /api/v2/{accountId}/VoucherAssignments/Contact/{contactId}/{voucherAssignmentId}
             * -GET     /api/v2/{accountId}/VoucherAssignments/VoucherCode/{code}
             */

            List<YulsnVoucherGroup> voucherGroups;
            YulsnVoucher groupVoucher = null;

            // get voucher groups
            {
                voucherGroups = await _yulsnClient.GetActiveVoucherGroupsAsync<YulsnVoucherGroup>();

                Assert.NotNull(voucherGroups);
                Assert.True(voucherGroups.Count > 0);
            }

            // get a voucher group and its active vouchers
            {
                foreach (var voucherGroup in voucherGroups)
                {
                    YulsnVoucherGroup _voucherGroup = await _yulsnClient.GetVoucherGroupByIdAsync<YulsnVoucherGroup>(voucherGroup.Id);

                    Assert.NotNull(_voucherGroup);
                    Assert.Equal(voucherGroup.Id, _voucherGroup.Id);

                    List<YulsnVoucher> groupVouchers = await _yulsnClient.GetActiveVouchersFromVoucherGroupAsync<YulsnVoucher>(voucherGroup.Id);

                    if (groupVouchers is not null && groupVouchers.Count > 0)
                    {
                        groupVoucher = groupVouchers[0];

                        YulsnVoucher _groupVoucher = await _yulsnClient.GetVoucherFromVoucherGroupAsync<YulsnVoucher>(voucherGroup.Id, groupVoucher.VoucherId);

                        Assert.NotNull(_groupVoucher);
                        Assert.Equal(groupVoucher.VoucherId, _groupVoucher.VoucherId);

                        break;
                    }
                }

                Assert.NotNull(groupVoucher); // using this active group voucher for next tests
            }

            // import voucher codes
            {
                YulsnVoucherCodeImportRequest request = new()
                {
                    ColumnDelimiter = ";",
                    Encoding = "utf-8",
                    FirstLineIsHeader = true,
                    Fields =
                    [
                        new() { CsvField = nameof(YulsnVoucherCode.Code), LoyaltiiField = nameof(YulsnVoucherCode.Code) },
                        new() { CsvField = nameof(YulsnVoucherCode.ValidFrom), LoyaltiiField = nameof(YulsnVoucherCode.ValidFrom) },
                        new() { CsvField = nameof(YulsnVoucherCode.ValidTo), LoyaltiiField = nameof(YulsnVoucherCode.ValidTo) },
                    ],
                    VoucherId = groupVoucher.VoucherId
                };

                DateTime now = DateTime.UtcNow;
                var validFrom = now.Date.ToString("o"); // ISO 8601, UTC
                var validTo = now.AddDays(1).Date.ToString("o");

                // ensure that there are enough valid codes
                List<List<string>> fileData =
                [
                    [nameof(YulsnVoucherCode.Code), nameof(YulsnVoucherCode.ValidFrom), nameof(YulsnVoucherCode.ValidTo)],
                    [$"xunit_{Guid.NewGuid():N}", validFrom, validTo],
                    [$"xunit_{Guid.NewGuid():N}", validFrom, validTo],
                ];

                request.File = Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, fileData.Select(o => string.Join(request.ColumnDelimiter, o))));

                YulsnVoucherCodeImportResponse response = await _yulsnClient.ImportVoucherCodesAsync(request);

                Assert.NotNull(response);
                Assert.True(response.BulkImportId > 0);
            }

            int assignmentId_1;

            // assign voucher to contact
            {
                YulsnPostVoucherAssignmentResponse response = await _yulsnClient.AssignVoucherToContactAsync(new YulsnPostVoucherAssignmentRequest
                {
                    ContactId = _model.ValidContactId,
                    VoucherId = groupVoucher.VoucherId,
                    VoucherGroupId = groupVoucher.VoucherGroupId,
                });

                Assert.NotNull(response);
                Assert.True(response.VoucherAssignmentId > 0);

                YulsnVoucherAssignment assignment = await _yulsnClient.GetVoucherAssignmentByContactAsync(_model.ValidContactId, response.VoucherAssignmentId);

                Assert.NotNull(assignment);
                Assert.NotNull(assignment.VoucherCode);
                Assert.Equal(response.VoucherAssignmentId, assignment.Id);
                Assert.Equal(groupVoucher.VoucherId, assignment.VoucherId);
                Assert.Equal(groupVoucher.VoucherGroupId, assignment.VoucherGroupId);
                Assert.Equal(_model.ValidContactId, assignment.ContactId);
                Assert.Equal(VoucherAssignmentStatus.Assigned, assignment.Status);

                assignmentId_1 = response.VoucherAssignmentId;
            }

            int assignmentId_2;

            // reserve then assign voucher to contact
            {
                YulsnPostVoucherAssignmentResponse response = await _yulsnClient.AssignVoucherToContactAsync(new YulsnPostVoucherAssignmentRequest
                {
                    ContactId = _model.ValidContactId,
                    VoucherId = groupVoucher.VoucherId,
                    VoucherGroupId = groupVoucher.VoucherGroupId,
                    Reserve = true
                });

                Assert.NotNull(response);
                Assert.True(response.VoucherAssignmentId > 0);

                YulsnVoucherAssignment assignment = await _yulsnClient.GetVoucherAssignmentByContactAsync(_model.ValidContactId, response.VoucherAssignmentId);

                Assert.NotNull(assignment);
                Assert.NotNull(assignment.VoucherCode);
                Assert.Equal(response.VoucherAssignmentId, assignment.Id);
                Assert.Equal(groupVoucher.VoucherId, assignment.VoucherId);
                Assert.Equal(groupVoucher.VoucherGroupId, assignment.VoucherGroupId);
                Assert.Equal(_model.ValidContactId, assignment.ContactId);
                Assert.Equal(VoucherAssignmentStatus.Reserved, assignment.Status);

                assignmentId_2 = response.VoucherAssignmentId;
            }

            // update voucher assignment status (Reserved -> Assigned)
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;

                await _yulsnClient.UpdateVoucherAssignmentAsync(new YulsnPatchVoucherAssignmentRequest
                {
                    VoucherAssingmentId = assignmentId_2,
                    Status = VoucherAssignmentStatus.Assigned,
                    StatusChanged = now.AddHours(-1),
                });

                YulsnVoucherAssignment assignment = await _yulsnClient.GetVoucherAssignmentByContactAsync(_model.ValidContactId, assignmentId_2);

                Assert.NotNull(assignment);
                Assert.NotNull(assignment.VoucherCode);
                Assert.Equal(assignmentId_2, assignment.Id);
                Assert.Equal(groupVoucher.VoucherId, assignment.VoucherId);
                Assert.Equal(groupVoucher.VoucherGroupId, assignment.VoucherGroupId);
                Assert.Equal(_model.ValidContactId, assignment.ContactId);
                Assert.Equal(VoucherAssignmentStatus.Assigned, assignment.Status);
                Assert.NotNull(assignment.StatusChanged);
                Assert.True(assignment.StatusChanged < now);
            }

            YulsnVoucherAssignment assignment_1;

            // update status to Redeemed then try update again to Assigned (should stay Redeemed)
            // Assigned -> Redeemed is allowed
            // Redeemed -> Assigned is NOT allowed
            {
                await _yulsnClient.UpdateVoucherAssignmentAsync(new YulsnPatchVoucherAssignmentRequest
                {
                    VoucherAssingmentId = assignmentId_1,
                    Status = VoucherAssignmentStatus.Redeemed
                });

                YulsnVoucherAssignment assignment = await _yulsnClient.GetVoucherAssignmentByContactAsync(_model.ValidContactId, assignmentId_1);

                Assert.NotNull(assignment);
                Assert.NotNull(assignment.VoucherCode);
                Assert.Equal(assignmentId_1, assignment.Id);
                Assert.Equal(groupVoucher.VoucherId, assignment.VoucherId);
                Assert.Equal(groupVoucher.VoucherGroupId, assignment.VoucherGroupId);
                Assert.Equal(_model.ValidContactId, assignment.ContactId);
                Assert.Equal(VoucherAssignmentStatus.Redeemed, assignment.Status);
                Assert.NotNull(assignment.StatusChanged);

                try
                {
                    await _yulsnClient.UpdateVoucherAssignmentAsync(new YulsnPatchVoucherAssignmentRequest
                    {
                        VoucherAssingmentId = assignmentId_1,
                        Status = VoucherAssignmentStatus.Assigned
                    });

                    Assert.Fail("Expected ProblemDetails was not thrown.");
                }
                catch (ProblemDetails problem)
                {
                    Assert.Equal((int)HttpStatusCode.Conflict, problem.Status);
                    Assert.Equal(HttpStatusCode.Conflict.ToString(), problem.Title);
                    Assert.Contains("Could not update the status", problem.Detail);
                }

                assignment = await _yulsnClient.GetVoucherAssignmentByContactAsync(_model.ValidContactId, assignmentId_1);

                Assert.NotNull(assignment);
                Assert.NotNull(assignment.VoucherCode);
                Assert.Equal(assignmentId_1, assignment.Id);
                Assert.Equal(groupVoucher.VoucherId, assignment.VoucherId);
                Assert.Equal(groupVoucher.VoucherGroupId, assignment.VoucherGroupId);
                Assert.Equal(_model.ValidContactId, assignment.ContactId);
                Assert.Equal(VoucherAssignmentStatus.Redeemed, assignment.Status);
                Assert.NotNull(assignment.StatusChanged);

                assignment_1 = assignment;
            }

            // get voucher assignment by voucher code
            {
                YulsnVoucherAssignment assignment = await _yulsnClient.GetVoucherAssignmentByCodeAsync<YulsnVoucherAssignment>(assignment_1.VoucherCode.Code);

                Assert.NotNull(assignment);
                Assert.NotNull(assignment.VoucherCode);
                Assert.Equal(assignmentId_1, assignment.Id); // the assigned assignment was redeemed
                Assert.Equal(groupVoucher.VoucherId, assignment.VoucherId);
                Assert.Equal(groupVoucher.VoucherGroupId, assignment.VoucherGroupId);
                Assert.Equal(_model.ValidContactId, assignment.ContactId);
                Assert.Equal(VoucherAssignmentStatus.Redeemed, assignment.Status);
            }

            // get all voucher assignments by contact
            {
                List<YulsnVoucherAssignment> assignments = await _yulsnClient.GetVoucherAssignmentsByContactAsync(_model.ValidContactId);

                Assert.NotNull(assignments);
                Assert.True(assignments.Count >= 2); // at least the two we created
                Assert.Contains(assignments, o => o.Id == assignmentId_1);
                Assert.Contains(assignments, o => o.Id == assignmentId_2);
            }
        }
    }
}