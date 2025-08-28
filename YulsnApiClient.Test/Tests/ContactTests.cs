using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YulsnApiClient.Client;
using YulsnApiClient.Models.V1;
using YulsnApiClient.Test.Abstractions;

namespace YulsnApiClient.Test.Tests
{
    public class ContactTests(Setup setup) : IClassFixture<Setup>
    {
        private class YulsnTestContact : YulsnContact
        {
            public string Email { get; set; }
            public string Secret { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public byte[] Version { get; set; }
            public string RegistrationSource { get; set; }
        }

        private class PatchContactsResultDto
        {
            public required List<string> Updated { get; set; }
            public required List<string> NotFound { get; set; }
        }

        private readonly YulsnClient _yulsnClient = setup.ServiceProvider.GetService<YulsnClient>();
        private readonly TestModel _model = setup.Repository.Model;

        [Fact]
        public async Task GetContactById()
        {
            var contact = await _yulsnClient.GetContactByIdAsync<YulsnContact>(_model.ValidContactId);

            Assert.NotNull(contact);
        }

        [Fact]
        public async Task CreateDeleteContact()
        {
            string salt = $"{Guid.NewGuid():N}";
            string email = $"test+{salt}@integration.xunit";

            YulsnCreateContact newContact = new()
            {
                Email = email,
                RegistrationDateTime = DateTimeOffset.Now,
                RegistrationIp = "1.1.1.1",
                RegistrationSource = "YulsnApiClient.Test",
            };

            YulsnTestContact contact;

            {
                contact = await _yulsnClient.CreateContactAsync<YulsnCreateContact, YulsnTestContact>(newContact);

                Assert.NotNull(contact);
                Assert.Equal(email, contact.Email);
            }

            {
                await _yulsnClient.DeleteContactAsync(contact.Id);

                var deletedContact = await _yulsnClient.GetContactByIdAsync<YulsnContact>(contact.Id);

                Assert.Null(deletedContact);
            }
        }

        [Fact]
        public async Task SetContactPasswordAndLogin()
        {
            string newPassword = Guid.NewGuid().ToString("N");
            string wrongPassword = Guid.NewGuid().ToString("N");
            string validIp = "1.1.1.1";
            string source = "XUnitTest";

            await _yulsnClient.SetContactPasswordAsync(_model.ValidContactId, newPassword);

            YulsnContact contact = await _yulsnClient.LoginContactAsync<YulsnContact>(_model.ValidContactId, new YulsnLoginContact
            {
                Password = newPassword,
                Ip = validIp,
                Source = source
            });

            Assert.NotNull(contact);
            Assert.Equal(_model.ValidContactId, contact.Id);

            try
            {
                var contactWrong = await _yulsnClient.LoginContactAsync<YulsnContact>(_model.ValidContactId, new YulsnLoginContact
                {
                    Password = wrongPassword,
                    Ip = validIp,
                    Source = source
                });

                Assert.Null(contactWrong);
            }
            catch (HttpRequestException e)
            {
                // Password is not valid
                if (!e.Message.Contains("422"))
                    throw;
            }
        }

        [Fact]
        public async Task Get_Ok_WhenFoundById()
        {
            int id = _model.ValidContactId;

            Dictionary<string, object> response = await _yulsnClient.GetContactAsDictionaryByIdAsync<object>(id);

            ValidateContactFields(response, expectedId: id, expectedDynamicField: _model.ValidContactDynamicStringFieldValue);
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidId()
        {
            int id = TestModel.InvalidId;

            YulsnContact contact = await _yulsnClient.GetContactByIdAsync<YulsnContact>(id);

            Assert.Equal(default, contact);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundBySecret()
        {
            string secret = _model.ValidContactSecret;

            Dictionary<string, object> response = await _yulsnClient.GetContactAsDictionaryBySecretAsync<object>(secret);

            ValidateContactFields(response, expectedSecret: secret, expectedDynamicField: _model.ValidContactDynamicStringFieldValue);
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidSecret()
        {
            string secret = TestModel.InvalidSecret;

            YulsnContact contact = await _yulsnClient.GetContactBySecretAsync<YulsnContact>(secret);

            Assert.Equal(default, contact);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundByEmail()
        {
            string email = _model.ValidContactEmail;

            Dictionary<string, object> response = await _yulsnClient.GetContactAsDictionaryByEmailAsync<object>(email);

            ValidateContactFields(response, expectedEmail: email, expectedDynamicField: _model.ValidContactDynamicStringFieldValue);
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidEmail()
        {
            string email = TestModel.InvalidEmail;

            YulsnContact contact = await _yulsnClient.GetContactByEmailAsync<YulsnContact>(email);

            Assert.Equal(default, contact);
        }

        [Fact]
        public async Task Get_Ok_WhenFoundByPhone()
        {
            string phone = _model.ValidContactPhone;

            Dictionary<string, object> response = await _yulsnClient.GetContactAsDictionaryByPhoneAsync<object>(phone);

            ValidateContactFields(response, expectedPhone: phone, expectedDynamicField: _model.ValidContactDynamicStringFieldValue);
        }

        [Fact]
        public async Task Get_NotFound_WhenInvalidPhone()
        {
            string phone = TestModel.InvalidPhone;

            YulsnContact contact = await _yulsnClient.GetContactByPhoneAsync<YulsnContact>(phone);

            Assert.Equal(default, contact);
        }

        [Fact]
        public async Task PostPatchDelete_Success_WhenManage()
        {
            int contactId;
            string secret;
            string version;
            IDictionary<string, object> fields;

            string salt = $"{Guid.NewGuid():N}";
            string email = $"test+{salt}@integration.com";

            // post a contact
            {
                Dictionary<string, object> requestContent = new()
                {
                    { nameof(YulsnTestContact.Email), email },
                    { nameof(YulsnTestContact.FirstName), "Test" },
                    { nameof(YulsnTestContact.LastName), "Integration" },
                    { nameof(YulsnTestContact.RegistrationSource), "IntegrationTest" },
                    { _model.ValidContactDynamicStringFieldName, $"Dynamic field test {salt}" }
                };

                IDictionary<string, object> response = await _yulsnClient.SendAsync<Dictionary<string, object>>(
                    request: new HttpRequestMessage(HttpMethod.Post, $"api/v1/Contacts")
                    {
                        Content = _yulsnClient.JsonContent(requestContent)
                    },
                    apiVersion: YulsnApiVersion.V1);

                (contactId, secret, version, fields) = ValidateContactFields(response,
                    expectedEmail: email,
                    expectedDynamicField: requestContent[_model.ValidContactDynamicStringFieldName]);

                Assert.True(fields.TryGetValueAsString(nameof(YulsnTestContact.FirstName), out string _firstName));
                Assert.NotNull(_firstName);
                Assert.Equal(requestContent[nameof(YulsnTestContact.FirstName)], _firstName);

                Assert.True(fields.TryGetValueAsString(nameof(YulsnTestContact.LastName), out string _lastName));
                Assert.NotNull(_lastName);
                Assert.Equal(requestContent[nameof(YulsnTestContact.LastName)], _lastName);

                Assert.True(fields.TryGetValueAsString(nameof(YulsnTestContact.RegistrationSource), out string _registrationSource));
                Assert.NotNull(_registrationSource);
                Assert.Equal(requestContent[nameof(YulsnTestContact.RegistrationSource)], _registrationSource);
            }

            // patch a contact by id and version
            {
                Dictionary<string, object> requestContent = new()
                {
                    { nameof(YulsnTestContact.FirstName), "Patched Test" },
                    { nameof(YulsnTestContact.LastName), "Patched Integration" },
                    { _model.ValidContactDynamicStringFieldName, $"Patched dynamic field test {salt}" },
                    { nameof(YulsnTestContact.Version), version }
                };

                IDictionary<string, object> response = await _yulsnClient.SendAsync<Dictionary<string, object>>(
                    request: new HttpRequestMessage(HttpMethod.Patch, $"api/v1/Contacts/{contactId}")
                    {
                        Content = _yulsnClient.JsonContent(requestContent)
                    },
                    apiVersion: YulsnApiVersion.V1);

                (_, _, version, _) = ValidateContactFields(response,
                    expectedId: contactId,
                    expectedEmail: email,
                    expectedFirstName: requestContent[nameof(YulsnTestContact.FirstName)],
                    expectedLastName: requestContent[nameof(YulsnTestContact.LastName)],
                    expectedDynamicField: requestContent[_model.ValidContactDynamicStringFieldName]);
            }

            // put a contact
            {
                /*
                 * POSSIBLE ISSUE:
                 * -static fields that are not in the request content are set to be NULL
                 * -dynamic fields that are not in the request content are NOT set to be null
                 * 
                 * QUESTION:
                 * -should both static and dynamic fields be set the same way?
                 */

                Dictionary<string, object> requestContent = new()
                {
                    { nameof(YulsnTestContact.Email), email },
                    { nameof(YulsnTestContact.FirstName), "Put Test" },
                    { _model.ValidContactDynamicStringFieldName, (object)null },
                    { nameof(YulsnTestContact.Version), version }
                };

                IDictionary<string, object> response = await _yulsnClient.SendAsync<Dictionary<string, object>>(
                    request: new HttpRequestMessage(HttpMethod.Put, $"api/v1/Contacts/{contactId}")
                    {
                        Content = _yulsnClient.JsonContent(requestContent)
                    },
                    apiVersion: YulsnApiVersion.V1);

                (_, _, _, fields) = ValidateContactFields(response,
                    expectedId: contactId,
                    expectedEmail: email,
                    expectedFirstName: requestContent[nameof(YulsnTestContact.FirstName)]);

                Assert.False(fields.TryGetValueAsString(nameof(YulsnTestContact.LastName), out string _));
                Assert.False(fields.TryGetValueAsString(_model.ValidContactDynamicStringFieldName, out string _));
            }

            Dictionary<string, object> patchContent = new()
            {
                { nameof(YulsnTestContact.FirstName), "Patched 2 Test" },
                { nameof(YulsnTestContact.LastName), "Patched 2 Integration" },
                { nameof(YulsnTestContact.Phone), salt.Truncate(20) },
                { _model.ValidContactDynamicStringFieldName, $"Patched v2 {salt}" },
            };

            // patch a contact by field(s)
            {
                string[] keys = [secret];

                PatchContactsResultDto responseContent = await _yulsnClient.SendAsync<PatchContactsResultDto>(
                    request: new HttpRequestMessage(HttpMethod.Patch, $"api/v1/Contacts")
                    {
                        Content = _yulsnClient.JsonContent(new
                        {
                            KeyType = "Secret",
                            Keys = keys,
                            Fields = patchContent
                        })
                    },
                    apiVersion: YulsnApiVersion.V1);

                Assert.NotNull(responseContent);
                Assert.Empty(responseContent.NotFound);
                Assert.NotEmpty(responseContent.Updated);
                Assert.Empty(responseContent.Updated.Except(keys));
            }

            // check the contact after the 2nd patch
            {
                Dictionary<string, object> response = await _yulsnClient.GetContactAsDictionaryByIdAsync<object>(contactId);

                ValidateContactFields(response,
                    expectedId: contactId,
                    expectedEmail: email,
                    expectedPhone: patchContent[nameof(YulsnTestContact.Phone)],
                    expectedFirstName: patchContent[nameof(YulsnTestContact.FirstName)],
                    expectedLastName: patchContent[nameof(YulsnTestContact.LastName)],
                    expectedDynamicField: patchContent[_model.ValidContactDynamicStringFieldName]);
            }

            // delete a contact
            {
                await _yulsnClient.DeleteContactAsync(contactId);

                YulsnContact contact = await _yulsnClient.GetContactByIdAsync<YulsnContact>(contactId);

                Assert.Equal(default, contact);
            }
        }

        private (int id, string secret, string version, IDictionary<string, object> fields) ValidateContactFields(
          IDictionary<string, object> fields,
          int? expectedId = null,
          string expectedSecret = null,
          string expectedEmail = null,
          object expectedPhone = null,
          object expectedFirstName = null,
          object expectedLastName = null,
          object expectedDynamicField = null)
        {
            Assert.True(fields.TryGetValueAsLong(nameof(YulsnTestContact.Id), out long? _id));
            Assert.NotNull(_id);

            Assert.True(fields.TryGetValueAsString(nameof(YulsnTestContact.Secret), out string _secret));
            Assert.NotNull(_secret);

            Assert.True(fields.TryGetValueAsString(nameof(YulsnTestContact.Version), out string _version));
            Assert.NotNull(_version);

            bool hasEmail = fields.TryGetValueAsString(nameof(YulsnTestContact.Email), out string _email);
            bool hasPhone = fields.TryGetValueAsString(nameof(YulsnTestContact.Phone), out string _phone);
            bool hasFirstName = fields.TryGetValueAsString(nameof(YulsnTestContact.FirstName), out string _firstName);
            bool hasLastName = fields.TryGetValueAsString(nameof(YulsnTestContact.LastName), out string _lastName);

            Assert.Contains(_model.ValidContactDynamicStringFieldName, fields.Keys);
            bool hasDynamicField = fields.TryGetValueAsString(_model.ValidContactDynamicStringFieldName, out string _dynamicField);

            if (expectedId is not null)
            {
                Assert.Equal(expectedId, _id);
            }

            if (expectedSecret is not null)
            {
                Assert.Equal(expectedSecret, _secret);
            }

            if (expectedEmail is not null)
            {
                Assert.True(hasEmail);
                Assert.NotNull(_email);
                Assert.Equal(expectedEmail, _email);
            }

            if (expectedPhone is not null)
            {
                Assert.True(hasPhone);
                Assert.NotNull(_phone);
                Assert.Equal(expectedPhone, _phone);
            }

            if (expectedFirstName is not null)
            {
                Assert.True(hasFirstName);
                Assert.NotNull(_firstName);
                Assert.Equal(expectedFirstName, _firstName);
            }

            if (expectedLastName is not null)
            {
                Assert.True(hasLastName);
                Assert.NotNull(_lastName);
                Assert.Equal(expectedLastName, _lastName);
            }

            if (expectedDynamicField is not null)
            {
                Assert.True(hasDynamicField);
                Assert.NotNull(_dynamicField);
                Assert.Equal(expectedDynamicField, _dynamicField);
            }

            return ((int)_id.Value, _secret, _version, fields);
        }

        /*
GET api/v1/Contacts	
Returns all contactIds in system

GET api/v1/Contacts?field={field}&value={value}	
Will return array of contacts by dynamic field or empty array if no contacts are found. The returned list of fields are cached.

GET api/v1/Contacts/PatchFields	
Will return a list of contact fields which can be PATCHed on a batch of contacts.

GET api/v1/Contacts/DynamicFields	
Will get Account specific fields. The fields will be valid to use in all requests, but will not be shown in the documentation, because it is account wide documentation.

GET api/v1/Contacts/GetContactsBaseInfo	
Returns basic info about all contacts in the system.

GET api/v1/Contacts/Registration?timespan={timespan}&source={source}&ip={ip}	
Will return array of registered contacts by Registration timespan, RegistrationSource or RegistrationIp (can combine). Will return empty array if nothing is found.

GET api/v1/Contacts/{id}/PersonDataFile	
Will return url of person data file

POST api/v1/Contacts/{id}/Login	
Validates a password for a specific user. Will return contact or HTTP 404 if contact is not found. The returned list of fields are cached.

POST api/v1/Contacts/{id}/SetPassword	
Sets the password for a user. Will return HTTP 200 or HTTP 404 if contact is not found.

POST api/v1/Contacts/{id}/UnblockEmail	
Unblocks a contact that is bounced by email. Returns false if the contact is not bounced by email.
 */
    }
}
