using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;


namespace Logitude.BL.Mocks
{
    public class MockLoggedContactUtil : ILoggedContactUtil
    {
        public ContactPM GetLoggedContact(int tenant)
        {
            string expectedLoggedUserId = "myUser";
            ContactPM loggedcontact = new ContactPM()
            {
                Id = expectedLoggedUserId,
                DontShowLocal = true,
            };

            return loggedcontact;
        }

        public Contact GetLoggedContactsIncludingCustomerCareForWR(int tenant)
        {
            string expectedLoggedUserId = "myUser";
            Contact loggedcontact = new Contact()
            {
                Id = expectedLoggedUserId,
                DontShowLocalLabels = true,
            };

            return loggedcontact;
        }

    }
}
