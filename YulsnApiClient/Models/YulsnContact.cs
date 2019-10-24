using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models
{
    public class YulsnContact
    {
        public int Id { get; set; }
    }
    public class YulsnCreateContact
    {
        /// <summary>
        /// Date and time of registration. In most cases should be set to current time.
        /// </summary>
        public DateTimeOffset RegistrationDateTime { get; set; }
        /// <summary>
        /// Registration source for the new contact, should be set to whatever reffered new contact.
        /// Required, Length: inclusive between 0 and 50 
        /// </summary>
        public string RegistrationSource { get; set; }
        /// <summary>
        /// Registartion IPv4 for contact.
        /// Required, Length: inclusive between 0 and 16
        /// </summary>
        public string RegistrationIp { get; set; }
        /// <summary>
        /// Store responsible for creating the contact.
        /// </summary>
        public int? RegistrationStoreId { get; set; }
        /// <summary>
        /// Contact Email, must be unique within account.
        /// Length: inclusive between 0 and 250
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Email of this contact has been confirmed by clicking confirmation link in an email or some other mechanism ensuring that contact has access to email.
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// Contact Phone, must be unique withn account.
        /// Length: inclusive between 0 and 20
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Phone of this contact has been confirmed by clicking confirmation link in an text message or some other mechanism ensuring that contact has access to phone.
        /// </summary>
        public bool PhoneConfirmed { get; set; }
        /// <summary>
        /// First name of contact.
        /// Length: inclusive between 0 and 50
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of contact.
        /// Length: inclusive between 0 and 50
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Id of preferred store.
        /// </summary>
        public int? PreferredStoreId { get; set; }
        /// <summary>
        /// ContactCompanyId if the Contact is associated with a Company. Null if the contact is not associated with a ContactCompany.
        /// </summary>
        public int? ContactCompanyId { get; set; }
        /// <summary>
        /// Source that was specified when latest permission was set.
        /// </summary>
        public string LatestPermissionSource { get; set; }

    }

    public class YulsnLoginContact
    {
        public string Password { get; set; }
        public string Ip { get; set; }
        public string Source { get; set; }
    }


}
