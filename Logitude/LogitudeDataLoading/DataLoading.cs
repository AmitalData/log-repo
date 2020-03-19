using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace LogitudeDataLoading
{
    public class DataLoading
    {
        public string LoadCustomersFromAfile(int tenant)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            ICommonDataContext otherObjectContext = CommonDataContext.GetContext(tenant);

            CountryRepository countryRep = new CountryRepository(otherObjectContext);
            StateRepository stateRep = new StateRepository(otherObjectContext);
            Dictionary<string, State> statesDictionary = stateRep.GetStates(tenant).ToDictionary(d => d.Code + ',' + d.CountryId, o => o);
            Dictionary<string, Country> countrieysDictionary = countryRep.GetCountries(tenant).ToDictionary(d => d.Code, o => o);

            ContactRepository contactRep = new ContactRepository(otherObjectContext);
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            Contact systemContact = contactRep.GetSingleContactByEmail(systemContactEmail, tenant);
            ContactQuery contactQuery = new ContactQuery(contactRep);
            List<ContactPM> contacts = GetContactPMsByTenant(tenant);
            ContactPM cont = contacts.Where(d => d.Email == "imardo@il.loreal.com").FirstOrDefault();

            string status = "success";

            CustomerRepository customerrep = new CustomerRepository(otherObjectContext);
            CustomerQuery customerQuery = new CustomerQuery(customerrep);
            List<CustomerPM> customers = null;
            AddressRepository addressRep = new AddressRepository(otherObjectContext);
            AddressQuery addressQuery = new AddressQuery(addressRep);
            List<AddressPM> addresses = null;
            AddressService addressService = new AddressService(otherObjectContext, tenant);

            var query = (from card in otherObjectContext.Cards
                         join address in otherObjectContext.Addresses on card.Id equals address.CardId
                         where card.PartnerTypeId == "CS" && address.AddressTypeId == "M" && card.Tenant == tenant
                         select new { Id = card.Id, Code = card.Code, Name = card.EnglishName, Address1 = address.Address1 });
            Dictionary<string, string> customerCodesDect = new Dictionary<string, string>();

            foreach (var rec in query)
            {
                customerCodesDect.Add(rec.Id, (rec.Name + rec.Address1));
            }

            CardRepository cardRep = new CardRepository(tenant);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog.FileName);
                string customrsString = sr.ReadToEnd();
                sr.Close();


                string[] stringLineArray = customrsString.Split('\n');
                string[] readCustomersCodesData = null;
                Customer c;

                int counter = 0;
                for (int i = 0; i < stringLineArray.Length; i = i + 10)
                {
                    counter++;
                    //try
                    //{
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(2, 0, 0) }))
                    {
                        for (int j = i; j < i + 10 && j < stringLineArray.Count(); j++)
                        {


                            if (!string.IsNullOrEmpty(stringLineArray[j]))
                            {
                                stringLineArray[j] = stringLineArray[j].TrimEnd('\r');
                                readCustomersCodesData = stringLineArray[j].Split('\t');

                                if (readCustomersCodesData.Length >= 2)
                                {
                                    if (readCustomersCodesData[1].Length > 59)
                                        readCustomersCodesData[1] = readCustomersCodesData[1].Substring(0, 59);

                                    if (readCustomersCodesData[1].Trim() != String.Empty)
                                    {
                                        // 0      1         2          3           4         5    6        7        8      9    10    11          12        13                            14      
                                        //Type	Name	VAT Number	Address 1	Address 2	Zip	 City	State	Country	 Phone	Fax	 Email	Contact Name   ReceivablesAccountingCard PayablesAccountingCard

                                        string type = readCustomersCodesData[0].Trim() == "NULL" ? null : (readCustomersCodesData[0]);
                                        string name = readCustomersCodesData[1].Trim() == "NULL" ? null : (readCustomersCodesData[1].Length > 59 ? readCustomersCodesData[1].Substring(0, 59) : readCustomersCodesData[1]);
                                        string customerName = readCustomersCodesData[1].Trim() == "NULL" ? null : (readCustomersCodesData[1].Length > 59 ? readCustomersCodesData[1].Substring(0, 59) : readCustomersCodesData[1]);
                                        string vatNumber = readCustomersCodesData[2].Trim() == "NULL" ? null : (readCustomersCodesData[2].Length > 19 ? readCustomersCodesData[2].Substring(0, 19) : readCustomersCodesData[2]);
                                        string address1 = readCustomersCodesData[3].Trim() == "NULL" ? null : (readCustomersCodesData[3].Length > 64 ? readCustomersCodesData[3].Substring(0, 64) : readCustomersCodesData[3]);
                                        string address2 = readCustomersCodesData[4].Trim() == "NULL" ? null : (readCustomersCodesData[4].Length > 64 ? readCustomersCodesData[4].Substring(0, 64) : readCustomersCodesData[4]);
                                        string zip = readCustomersCodesData[5].Trim() == "NULL" ? null : (readCustomersCodesData[5].Length > 14 ? readCustomersCodesData[5].Substring(0, 14) : readCustomersCodesData[5]);
                                        string city = readCustomersCodesData[6].Trim() == "NULL" ? null : (readCustomersCodesData[6].Length > 24 ? readCustomersCodesData[6].Substring(0, 24) : readCustomersCodesData[6]);
                                        string stateCode = readCustomersCodesData[7].Trim() == "NULL" ? null : readCustomersCodesData[7].Trim();
                                        string countryCode = readCustomersCodesData[8].Trim() == "NULL" ? null : readCustomersCodesData[8].Trim();
                                        string phone = readCustomersCodesData[9].Trim() == "NULL" ? null : readCustomersCodesData[9].Trim();
                                        string fax = readCustomersCodesData[10].Trim() == "NULL" ? null : readCustomersCodesData[10].Trim();
                                        string email = readCustomersCodesData[11].Trim() == "NULL" ? null : readCustomersCodesData[11].Trim();
                                        string contactName = readCustomersCodesData[12].Trim() == "NULL" ? null : readCustomersCodesData[12].Trim();

                                        string receivablesAccountingCard = null;
                                        if (readCustomersCodesData.Length >= 14)
                                        {
                                            receivablesAccountingCard = readCustomersCodesData[13].Trim() == "NULL" ? null : (readCustomersCodesData[13].Trim());
                                        }

                                        string payablesAccountingCard = null;
                                        if (readCustomersCodesData.Length >= 15)
                                        {
                                            payablesAccountingCard = readCustomersCodesData[14].Trim() == "NULL" ? null : (readCustomersCodesData[14].Trim());
                                        }

                                        Country country = null;
                                        if (countrieysDictionary.Keys.Contains(countryCode))
                                        {
                                            country = countrieysDictionary[countryCode];
                                        }

                                        State state = null;
                                        if (country != null)
                                        {

                                            if (statesDictionary.Keys.Contains(stateCode + ',' + country.Id))
                                            {
                                                state = statesDictionary[stateCode + ',' + country.Id];
                                            }
                                        }

                                        //if doesn't exist add customer with address and contact.
                                        if (!customerCodesDect.Values.Contains((name + address1)))
                                        {

                                            AddressPM address = new AddressPM()
                                            {
                                                Name = name,
                                                Description = "Main Address",
                                                Address1 = address1,
                                                Address2 = address2,
                                                ZipCode = zip,
                                                StateId = state != null ? state.Id : null,
                                                CountryId = country != null ? country.Id : null,
                                                City = city,
                                                PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null,
                                                FaxNumber = fax,
                                                AddressTypeId = "M",
                                                Tenant = tenant,

                                            };


                                            CustomerPM customer = new CustomerPM()
                                            {
                                                EnglishName = customerName,
                                                VatNumber = vatNumber,
                                                Tenant = tenant,
                                                IsHybrid = true,
                                                Code = CodeCounter.GetNumber("Customer", tenant).ToString(),
                                                PartnerTypeId = "CS",
                                                //AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null,

                                                CustomerStatusCode = "ACT",
                                                IsCustomer = type.ToLower() == "customer" ? true : false,
                                                ReceivablesAccountingCard = receivablesAccountingCard,
                                                PayablesAccountingCard = payablesAccountingCard,
                                            };

                                            if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                            {
                                                string contactEnglishName = contactName;
                                                if (string.IsNullOrEmpty(contactName))
                                                {
                                                    contactEnglishName = email.Split('@')[0];
                                                }

                                                ContactPM contactPM = null;
                                                if (!string.IsNullOrEmpty(email))
                                                {
                                                    contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                }

                                                else if (!string.IsNullOrEmpty(contactName))
                                                {
                                                    contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                }

                                                if (contactPM == null)
                                                {
                                                    contactPM = new ContactPM()
                                                    {
                                                        //Id = IdCounter.GetNumber("Contact", tenant),
                                                        Email = email,
                                                        EnglishName = contactEnglishName,
                                                        Tenant = tenant,
                                                        CardId = "newCard",
                                                        IsHybrid = true,
                                                        IsCreatedWithPartner = true,

                                                    };

                                                   // contactPM.CardId = "newCard";
                                                    customer.Contacts.Add(contactPM);

                                                    contacts.Add(contactPM);
                                                    //ContactService contactService = new ContactService(objectContext, tenant);
                                                    //contactService.Create(contactPM);
                                                }
                                            }

                                            customer.Addresses.Add(address);

                                            //CustomerService service = new CustomerService(objectContext, customer, systemContact.Id);
                                            //service.Create(customer);

                                            ICommonDataContext MyContext = CommonDataContext.GetContext(customer.Tenant);
                                            CustomerService service = new CustomerService(MyContext, customer, systemContact.Id);
                                            service.Create();

                                            customerCodesDect.Add(customer.Id, customer.EnglishName + address.Address1);
                                            objectContext.SaveChanges();

                                        }
                                        //update customer.
                                        else
                                        {
                                            CacheManager.CacheWrapper = new MockCacheWrapper();
                                            if (customers == null)
                                            {
                                                customers = customerQuery.GetCustomerPMsByTenant(tenant).ToList();
                                            }
                                            if (addresses == null)
                                            {
                                                addresses = addressQuery.GetAddressePMsByTenant(tenant).ToList();
                                            }
                                            string key = customerCodesDect.Where(d => d.Value == (name + address1)).FirstOrDefault().Key;
                                            CustomerPM updatedCustomer = customers.Where(d => d.Id == key).FirstOrDefault();
                                            if (updatedCustomer != null)
                                            {
                                                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                {
                                                    ContactService contactService = new ContactService(otherObjectContext, tenant);
                                                    string contactEnglishName = contactName;
                                                    if (string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactEnglishName = email.Split('@')[0];
                                                    }
                                                    ContactPM contactPM = null;
                                                    if (!string.IsNullOrEmpty(email))
                                                    {
                                                        contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                    }
                                                    else if (!string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                    }
                                                    if (contactPM == null)
                                                    {
                                                        contactPM = new ContactPM()
                                                        {
                                                            //Id = IdCounter.GetNumber("Contact", tenant),
                                                            Email = email,
                                                            EnglishName = contactEnglishName,
                                                            Tenant = tenant,
                                                            CardId = key,
                                                            IsHybrid = true,
                                                        };
                                                        contacts.Add(contactPM);
                                                        contactService.Create(contactPM);
                                                        objectContext.SaveChanges();
                                                        updatedCustomer.Contacts.Add(contactPM);
                                                    }
                                                    else
                                                    {
                                                        if (contactPM.Id.Length <= 15)
                                                        {
                                                            contactPM.EnglishName = contactName;
                                                            contactPM.Email = email;
                                                            contactPM.IsHybrid = true;
                                                            contactService.Update(contactPM);
                                                            objectContext.SaveChanges();
                                                        }
                                                    }

                                                    AddressPM updatedAddress = addresses.Where(d => d.CardId == updatedCustomer.Id && d.AddressTypeId == "M").FirstOrDefault();
                                                    updatedAddress.Name = name;
                                                    updatedAddress.IsHybrid = true;
                                                    updatedAddress.Address2 = address2;
                                                    updatedAddress.ZipCode = zip;
                                                    updatedAddress.StateId = state != null ? state.Id : null;
                                                    updatedAddress.CountryId = country != null ? country.Id : null;
                                                    updatedAddress.City = city;
                                                    updatedAddress.PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null;
                                                    updatedAddress.FaxNumber = fax;
                                                    addressService.Update(updatedAddress);

                                                    updatedCustomer.EnglishName = customerName;
                                                    updatedCustomer.VatNumber = vatNumber;
                                                    updatedCustomer.IsHybrid = true;
                                                    updatedCustomer.PayablesAccountingCard = payablesAccountingCard;
                                                    updatedCustomer.ReceivablesAccountingCard = receivablesAccountingCard;
                                                    //updatedCustomer.AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null;

                                                    //CustomerService service = new CustomerService(otherObjectContext, updatedCustomer, systemContact.Id);
                                                    //service.Update(updatedCustomer);

                                                    ICommonDataContext MyContext = CommonDataContext.GetContext(updatedCustomer.Tenant);
                                                    CustomerService service = new CustomerService(MyContext, updatedCustomer, "system@tenant" + tenant + ".com");
                                                    service.Update();

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        scope.Complete();
                    }
                    //}
                    //catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    //{
                    //    string Error = "";
                    //    foreach (var eve in e.EntityValidationErrors)
                    //    {
                    //        Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                    //        foreach (var ve in eve.ValidationErrors)
                    //        {

                    //            Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    //        }
                    //    }

                    //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                    //    {
                    //        file.WriteLine(Error);
                    //        file.Close();
                    //    }

                    //    status = "fail";

                    //}
                    //catch (Exception exception)
                    //{
                    //    status = "fail";
                    //    string ErrorMessage = "";

                    //    if (exception != null)
                    //    {



                    //        ErrorMessage += exception.Message;

                    //        if (exception.InnerException != null)
                    //        {
                    //            ErrorMessage += Environment.NewLine + exception.InnerException.Message;

                    //            if (exception.InnerException.InnerException != null)
                    //            {
                    //                ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                    //                if (exception.InnerException.InnerException.InnerException != null)
                    //                {
                    //                    ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                    //                }
                    //            }
                    //        }

                    //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                    //        {
                    //            file.WriteLine(ErrorMessage);
                    //            file.Close();
                    //        }

                    //    }

                    //}

                    if (counter == 10)
                    {
                        objectContext = null;
                        objectContext = CommonDataContext.GetContext(tenant);
                        counter = 0;
                    }

                }


            }
            return status;
        }

        public List<ContactPM> GetContactPMsByTenant(int tenant)
        {
            var context = CommonDataContext.GetContext(tenant);
            List<ContactPM> contacts = (from a in context.Contacts
                                        where a.Tenant == tenant && a.UserType == "R"
                                        select new ContactPM()
                                        {
                                            Anniversary = a.Anniversary,
                                            Birthday = a.Birthday,
                                            BusinessPhone = a.BusinessPhone,
                                            Email = a.Email,
                                            EnglishName = a.EnglishName,
                                            FacebookId = a.FacebookId,
                                            Fax = a.Fax,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            LocalName = a.LocalName,
                                            SearchFields = a.SearchFields,
                                            Mobile = a.Mobile,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            Signature = a.Signature,
                                            SignatureHtml = a.SignatureHtml,
                                            ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                            DisplayGettingStarted = a.DisplayGettingStarted,
                                            DontShowLocal = a.DontShowLocalLabels,
                                            BirthdayReminder = a.BirthdayReminder,
                                            AnniversaryReminder = a.AnniversaryReminder,
                                            ImageDetailId = a.ImageDetailId,
                                            DoneDate = a.DoneDate,
                                            BirthDayOfYear = a.BirthDayOfYear,
                                            ContactDoneMethodCode = a.ContactDoneMethod != null ? a.ContactDoneMethod.Code : null,
                                            Position = a.Position,
                                            ExternalId = a.ExternalId,
                                            IndexColor = a.IndexColor,
                                            CompanyName = a.CompanyName,
                                            CreateDate = a.CreateDate,
                                        }).ToList();

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            //    IGlobalContext globalContext = GlobalContext.GetContext();
            //    List<ContactPassword> contactPasswords = globalContext.ContactPasswords.Where(c => contacts.Any(ct => ct.Email == c.Email)).ToList();

            //    foreach (var c in contacts)
            //    {
            //        if (c.Email != null)
            //        {
            //            ContactPassword contactPassword = contactPasswords.Where(cn => cn.Email == c.Email.ToLower()).FirstOrDefault();
            //            if (contactPassword != null)
            //            {
            //                c.IsLocked = contactPassword.IsLocked;
            //                c.MustChangePassword = contactPassword.MustChangePassword;
            //                c.NumberOfRetries = contactPassword.NumberOfRetries;
            //            }
            //        }
            //    }
            //}

            return contacts;
        }

        public string LoadAgentsFromAFile(int tenant)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            ICommonDataContext otherObjectContext = CommonDataContext.GetContext(tenant);

            CountryRepository countryRep = new CountryRepository(otherObjectContext);
            StateRepository stateRep = new StateRepository(otherObjectContext);
            Dictionary<string, State> statesDictionary = stateRep.GetStates(tenant).ToDictionary(d => d.Code + ',' + d.CountryId, o => o);
            Dictionary<string, Country> countrieysDictionary = countryRep.GetCountries(tenant).ToDictionary(d => d.Code, o => o);

            ContactRepository contactRep = new ContactRepository(otherObjectContext);
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            Contact systemContact = contactRep.GetSingleContactByEmail(systemContactEmail, tenant);
            ContactQuery contactQuery = new ContactQuery(contactRep);
            List<ContactPM> contacts = contactQuery.GetContactPMsByTenant(tenant);
            ContactPM cont = contacts.Where(d => d.Email == "imardo@il.loreal.com").FirstOrDefault();

            string status = "success";

            AgentRepository agentRep = new AgentRepository(otherObjectContext);
            AgentQuery agentQuery = new AgentQuery(agentRep);
            List<AgentPM> agents = null;
            AddressRepository addressRep = new AddressRepository(otherObjectContext);
            AddressQuery addressQuery = new AddressQuery(addressRep);
            List<AddressPM> addresses = null;
            AddressService addressService = new AddressService(otherObjectContext, tenant);

            var query = (from card in otherObjectContext.Cards
                         join address in otherObjectContext.Addresses on card.Id equals address.CardId
                         where card.PartnerTypeId == "AG" && address.AddressTypeId == "M" && card.Tenant == tenant
                         select new { Id = card.Id, Code = card.Code, Name = card.EnglishName, Address1 = address.Address1 });
            Dictionary<string, string> agentCodesDect = new Dictionary<string, string>();

            foreach (var rec in query)
            {
                agentCodesDect.Add(rec.Id, (rec.Name + rec.Address1));
            }

            CardRepository cardRep = new CardRepository(tenant);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog.FileName);
                string customrsString = sr.ReadToEnd();
                sr.Close();


                string[] stringLineArray = customrsString.Split('\n');
                string[] readAgentsCodesData = null;
                

                int counter = 0;
                for (int i = 0; i < stringLineArray.Length; i = i + 10)
                {
                    counter++;
                    try
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(2, 0, 0) }))
                        {
                            for (int j = i; j < i + 10 && j < stringLineArray.Count(); j++)
                            {


                                if (!string.IsNullOrEmpty(stringLineArray[j]))
                                {
                                    stringLineArray[j] = stringLineArray[j].TrimEnd('\r');
                                    readAgentsCodesData = stringLineArray[j].Split('\t');

                                    if (readAgentsCodesData.Length >= 2)
                                    {
                                        if (readAgentsCodesData[1].Length > 59)
                                            readAgentsCodesData[1] = readAgentsCodesData[1].Substring(0, 59);

                                        if (readAgentsCodesData[1].Trim() != String.Empty)
                                        {
                                            string address1 = readAgentsCodesData[3].Trim() == "NULL" ? null : (readAgentsCodesData[3].Length > 64 ? readAgentsCodesData[3].Substring(0, 64) : readAgentsCodesData[3]);
                                            string name = readAgentsCodesData[1].Trim() == "NULL" ? null : (readAgentsCodesData[1].Length > 59 ? readAgentsCodesData[1].Substring(0, 59) : readAgentsCodesData[1]);


                                            // 0      1         2          3           4         5    6        7        8      9    10    11          12        13                              14
                                            //Type	Name	VAT Number	Address 1	Address 2	Zip	 City	State	Country	 Phone	Fax	 Email	Contact Name  ReceivablesAccountingCard   PayablesAccountingCard                   
                                            string type = readAgentsCodesData[0].Trim() == "NULL" ? null : (readAgentsCodesData[0]);
                                            string agentName = readAgentsCodesData[1].Trim() == "NULL" ? null : (readAgentsCodesData[1].Length > 59 ? readAgentsCodesData[1].Substring(0, 59) : readAgentsCodesData[1]);
                                            string vatNumber = readAgentsCodesData[2].Trim() == "NULL" ? null : (readAgentsCodesData[2].Length > 19 ? readAgentsCodesData[2].Substring(0, 19) : readAgentsCodesData[2]);
                                            string address2 = readAgentsCodesData[4].Trim() == "NULL" ? null : (readAgentsCodesData[4].Length > 64 ? readAgentsCodesData[4].Substring(0, 64) : readAgentsCodesData[4]);
                                            string zip = readAgentsCodesData[5].Trim() == "NULL" ? null : (readAgentsCodesData[5].Length > 14 ? readAgentsCodesData[5].Substring(0, 14) : readAgentsCodesData[5]);
                                            string city = readAgentsCodesData[6].Trim() == "NULL" ? null : (readAgentsCodesData[6].Length > 24 ? readAgentsCodesData[6].Substring(0, 24) : readAgentsCodesData[6]);
                                            string stateCode = readAgentsCodesData[7].Trim() == "NULL" ? null : readAgentsCodesData[7].Trim();
                                            string countryCode = readAgentsCodesData[8].Trim() == "NULL" ? null : readAgentsCodesData[8].Trim();
                                            string phone = readAgentsCodesData[9].Trim() == "NULL" ? null : readAgentsCodesData[9].Trim();
                                            string fax = readAgentsCodesData[10].Trim() == "NULL" ? null : readAgentsCodesData[10].Trim();
                                            string email = readAgentsCodesData[11].Trim() == "NULL" ? null : readAgentsCodesData[11].Trim();
                                            string contactName = readAgentsCodesData[12].Trim() == "NULL" ? null : readAgentsCodesData[12].Trim();

                                            string receivablesAccountingCard = null;
                                            if (readAgentsCodesData.Length >= 14)
                                            {
                                                receivablesAccountingCard = readAgentsCodesData[13].Trim() == "NULL" ? null : (readAgentsCodesData[13].Trim());
                                            }

                                            string payablesAccountingCard = null;
                                            if (readAgentsCodesData.Length >= 15)
                                            {
                                                payablesAccountingCard = readAgentsCodesData[14].Trim() == "NULL" ? null : (readAgentsCodesData[14].Trim());
                                            }

                                            Country country = null;
                                            if (countrieysDictionary.Keys.Contains(countryCode))
                                            {
                                                country = countrieysDictionary[countryCode];
                                            }

                                            State state = null;
                                            if (country != null)
                                            {

                                                if (statesDictionary.Keys.Contains(stateCode + ',' + country.Id))
                                                {
                                                    state = statesDictionary[stateCode + ',' + country.Id];
                                                }
                                            }

                                            //if doesn't exist add agent with address and contact.
                                            if (!agentCodesDect.Values.Contains((name + address1)))
                                            {

                                                AddressPM address = new AddressPM()
                                                {
                                                    Name = name,
                                                    Description = "Main Address",
                                                    Address1 = address1,
                                                    Address2 = address2,
                                                    ZipCode = zip,
                                                    StateId = state != null ? state.Id : null,
                                                    CountryId = country != null ? country.Id : null,
                                                    City = city,
                                                    PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null,
                                                    FaxNumber = fax,
                                                    AddressTypeId = "M",
                                                    Tenant = tenant,

                                                };


                                                AgentPM agent = new AgentPM()
                                                {
                                                    EnglishName = agentName,
                                                    VatNumber = vatNumber,
                                                    Tenant = tenant,
                                                    IsHybrid = true,
                                                    Code = CodeCounter.GetNumber("Agent", tenant).ToString(),
                                                    PartnerTypeId = "AG",
                                                    ReceivablesAccountingCard = receivablesAccountingCard,
                                                    PayablesAccountingCard = payablesAccountingCard,

                                                    //AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null,

                                                };

                                                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                {
                                                    string contactEnglishName = contactName;
                                                    if (string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactEnglishName = email.Split('@')[0];
                                                    }

                                                    ContactPM contactPM = null;
                                                    if (!string.IsNullOrEmpty(email))
                                                    {
                                                        contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                    }
                                                    else if (!string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                    }
                                                    if (contactPM == null)
                                                    {
                                                        contactPM = new ContactPM()
                                                        {
                                                            //Id = IdCounter.GetNumber("Contact", tenant),
                                                            Email = email,
                                                            EnglishName = contactEnglishName,
                                                            Tenant = tenant,
                                                            CardId = "newCard",
                                                            IsHybrid = true,
                                                            IsCreatedWithPartner = true,

                                                        };
                                                        contacts.Add(contactPM);
                                                        //ContactService contactService = new ContactService(objectContext, tenant);
                                                        //contactService.Create(contactPM);
                                                        objectContext.SaveChanges();
                                                    }
                                                    contactPM.CardId = "newCard";


                                                    agent.Contacts.Add(contactPM);

                                                }
                                                agent.Addresses.Add(address);

                                                AgentService service = new AgentService(objectContext, agent, systemContact.Id);

                                                service.Create(agent);
                                                agentCodesDect.Add(agent.Id, agent.EnglishName + address.Address1);
                                                objectContext.SaveChanges();

                                            }
                                            //update agent.
                                            else
                                            {
                                                CacheManager.CacheWrapper = new MockCacheWrapper();
                                                if (agents == null)
                                                {
                                                    agents = agentQuery.GetAgentPMsByTenant(tenant).ToList();
                                                }
                                                if (addresses == null)
                                                {
                                                    addresses = addressQuery.GetAddressePMsByTenant(tenant).ToList();
                                                }
                                                string key = agentCodesDect.Where(d => d.Value == (name + address1)).FirstOrDefault().Key;
                                                AgentPM updatedAgent = agents.Where(d => d.Id == key).FirstOrDefault();
                                                if (updatedAgent != null)
                                                {
                                                    if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                    {
                                                        ContactService contactService = new ContactService(otherObjectContext, tenant);
                                                        string contactEnglishName = contactName;
                                                        if (string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactEnglishName = email.Split('@')[0];
                                                        }
                                                        ContactPM contactPM = null;
                                                        if (!string.IsNullOrEmpty(email))
                                                        {
                                                            contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                        }
                                                        else if (!string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                        }
                                                        if (contactPM == null)
                                                        {
                                                            contactPM = new ContactPM()
                                                            {
                                                                //Id = IdCounter.GetNumber("Contact", tenant),
                                                                Email = email,
                                                                EnglishName = contactEnglishName,
                                                                Tenant = tenant,
                                                                CardId = key,
                                                                IsHybrid = true,
                                                            };
                                                            contacts.Add(contactPM);
                                                            contactService.Create(contactPM);
                                                            objectContext.SaveChanges();
                                                            updatedAgent.Contacts.Add(contactPM);
                                                        }
                                                        else
                                                        {
                                                            if (contactPM.Id.Length <= 15)
                                                            {
                                                                contactPM.EnglishName = contactName;
                                                                contactPM.Email = email;
                                                                contactPM.IsHybrid = true;
                                                                contactService.Update(contactPM);
                                                                objectContext.SaveChanges();
                                                            }
                                                        }

                                                        AddressPM updatedAddress = addresses.Where(d => d.CardId == updatedAgent.Id && d.AddressTypeId == "M").FirstOrDefault();
                                                        updatedAddress.Name = name;
                                                        updatedAddress.IsHybrid = true;
                                                        updatedAddress.Address2 = address2;
                                                        updatedAddress.ZipCode = zip;
                                                        updatedAddress.StateId = state != null ? state.Id : null;
                                                        updatedAddress.CountryId = country != null ? country.Id : null;
                                                        updatedAddress.City = city;
                                                        updatedAddress.PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null;
                                                        updatedAddress.FaxNumber = fax;
                                                        addressService.Update(updatedAddress);

                                                        updatedAgent.EnglishName = agentName;
                                                        updatedAgent.VatNumber = vatNumber;
                                                        updatedAgent.IsHybrid = true;
                                                        updatedAgent.ReceivablesAccountingCard = receivablesAccountingCard;
                                                        updatedAgent.PayablesAccountingCard = payablesAccountingCard;

                                                       // updatedAgent.AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null;

                                                        AgentService service = new AgentService(otherObjectContext, updatedAgent, systemContact.Id);
                                                        service.Update(updatedAgent);

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            scope.Complete();
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        string Error = "";
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                            foreach (var ve in eve.ValidationErrors)
                            {

                                Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                            }
                        }

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                        {
                            file.WriteLine(Error);
                            file.Close();
                        }

                        status = "fail";

                    }
                    catch (Exception exception)
                    {
                        status = "fail";
                        string ErrorMessage = "";

                        if (exception != null)
                        {



                            ErrorMessage += exception.Message;

                            if (exception.InnerException != null)
                            {
                                ErrorMessage += Environment.NewLine + exception.InnerException.Message;

                                if (exception.InnerException.InnerException != null)
                                {
                                    ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                                    if (exception.InnerException.InnerException.InnerException != null)
                                    {
                                        ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                                    }
                                }
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                            {
                                file.WriteLine(ErrorMessage);
                                file.Close();
                            }

                        }

                    }

                    if (counter == 10)
                    {
                        objectContext = null;
                        objectContext = CommonDataContext.GetContext(tenant);
                        counter = 0;
                    }

                }


            }
            return status;
        }

        public string LoadTruckersFromAFile(int tenant)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            ICommonDataContext otherObjectContext = CommonDataContext.GetContext(tenant);

            CountryRepository countryRep = new CountryRepository(otherObjectContext);
            StateRepository stateRep = new StateRepository(otherObjectContext);
            Dictionary<string, State> statesDictionary = stateRep.GetStates(tenant).ToDictionary(d => d.Code + ',' + d.CountryId, o => o);
            Dictionary<string, Country> countrieysDictionary = countryRep.GetCountries(tenant).ToDictionary(d => d.Code, o => o);

            ContactRepository contactRep = new ContactRepository(otherObjectContext);
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            Contact systemContact = contactRep.GetSingleContactByEmail(systemContactEmail, tenant);
            ContactQuery contactQuery = new ContactQuery(contactRep);
            List<ContactPM> contacts = contactQuery.GetContactPMsByTenant(tenant);
            ContactPM cont = contacts.Where(d => d.Email == "imardo@il.loreal.com").FirstOrDefault();

            string status = "success";

            TruckerRepository truckerRep = new TruckerRepository(otherObjectContext);
            TruckerQuery truckerQuery = new TruckerQuery(truckerRep);
            List<TruckerPM> truckers = null;
            AddressRepository addressRep = new AddressRepository(otherObjectContext);
            AddressQuery addressQuery = new AddressQuery(addressRep);
            List<AddressPM> addresses = null;
            AddressService addressService = new AddressService(otherObjectContext, tenant);

            var query = (from card in otherObjectContext.Cards
                         join address in otherObjectContext.Addresses on card.Id equals address.CardId
                         where card.PartnerTypeId == "TR" && address.AddressTypeId == "M" && card.Tenant == tenant
                         select new { Id = card.Id, Code = card.Code, Name = card.EnglishName, Address1 = address.Address1 });
            Dictionary<string, string> truckerCodesDect = new Dictionary<string, string>();

            foreach (var rec in query)
            {
                truckerCodesDect.Add(rec.Id, (rec.Name + rec.Address1));
            }

            CardRepository cardRep = new CardRepository(tenant);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog.FileName);
                string customrsString = sr.ReadToEnd();
                sr.Close();


                string[] stringLineArray = customrsString.Split('\n');
                string[] readTruckersCodesData = null;
               
                int counter = 0;
                for (int i = 0; i < stringLineArray.Length; i = i + 10)
                {
                    counter++;
                    try
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(2, 0, 0) }))
                        {
                            for (int j = i; j < i + 10 && j < stringLineArray.Count(); j++)
                            {


                                if (!string.IsNullOrEmpty(stringLineArray[j]))
                                {
                                    stringLineArray[j] = stringLineArray[j].TrimEnd('\r');
                                    readTruckersCodesData = stringLineArray[j].Split('\t');

                                    if (readTruckersCodesData.Length >= 2)
                                    {
                                        if (readTruckersCodesData[1].Length > 59)
                                            readTruckersCodesData[1] = readTruckersCodesData[1].Substring(0, 59);

                                        if (readTruckersCodesData[1].Trim() != String.Empty)
                                        {
                                            string address1 = readTruckersCodesData[3].Trim() == "NULL" ? null : (readTruckersCodesData[3].Length > 64 ? readTruckersCodesData[3].Substring(0, 64) : readTruckersCodesData[3]);
                                            string name = readTruckersCodesData[1].Trim() == "NULL" ? null : (readTruckersCodesData[1].Length > 59 ? readTruckersCodesData[1].Substring(0, 59) : readTruckersCodesData[1]);


                                            // 0      1         2          3           4         5    6        7        8      9    10    11          12        13
                                            //Type	Name	VAT Number	Address 1	Address 2	Zip	 City	State	Country	 Phone	Fax	 Email	Contact Name  code
                                            string type = readTruckersCodesData[0].Trim() == "NULL" ? null : (readTruckersCodesData[0]);
                                            string truckerName = readTruckersCodesData[1].Trim() == "NULL" ? null : (readTruckersCodesData[1].Length > 59 ? readTruckersCodesData[1].Substring(0, 59) : readTruckersCodesData[1]);
                                            string vatNumber = readTruckersCodesData[2].Trim() == "NULL" ? null : (readTruckersCodesData[2].Length > 19 ? readTruckersCodesData[2].Substring(0, 19) : readTruckersCodesData[2]);
                                            string address2 = readTruckersCodesData[4].Trim() == "NULL" ? null : (readTruckersCodesData[4].Length > 64 ? readTruckersCodesData[4].Substring(0, 64) : readTruckersCodesData[4]);
                                            string zip = readTruckersCodesData[5].Trim() == "NULL" ? null : (readTruckersCodesData[5].Length > 14 ? readTruckersCodesData[5].Substring(0, 14) : readTruckersCodesData[5]);
                                            string city = readTruckersCodesData[6].Trim() == "NULL" ? null : (readTruckersCodesData[6].Length > 24 ? readTruckersCodesData[6].Substring(0, 24) : readTruckersCodesData[6]);
                                            string stateCode = readTruckersCodesData[7].Trim() == "NULL" ? null : readTruckersCodesData[7].Trim();
                                            string countryCode = readTruckersCodesData[8].Trim() == "NULL" ? null : readTruckersCodesData[8].Trim();
                                            string phone = readTruckersCodesData[9].Trim() == "NULL" ? null : readTruckersCodesData[9].Trim();
                                            string fax = readTruckersCodesData[10].Trim() == "NULL" ? null : readTruckersCodesData[10].Trim();
                                            string email = readTruckersCodesData[11].Trim() == "NULL" ? null : readTruckersCodesData[11].Trim();
                                            string contactName = readTruckersCodesData[12].Trim() == "NULL" ? null : readTruckersCodesData[12].Trim();

                                            string code = null;
                                            if (readTruckersCodesData.Length >= 14)
                                            {
                                                code = readTruckersCodesData[13].Trim() == "NULL" ? null : (readTruckersCodesData[13].Trim());
                                            }

                                            string receivablesAccountingCard = null;
                                            if (readTruckersCodesData.Length >= 15)
                                            {
                                                receivablesAccountingCard = readTruckersCodesData[14].Trim() == "NULL" ? null : (readTruckersCodesData[14].Trim());
                                            }

                                            string payablesAccountingCard = null;
                                            if (readTruckersCodesData.Length >= 16)
                                            {
                                                payablesAccountingCard = readTruckersCodesData[15].Trim() == "NULL" ? null : (readTruckersCodesData[15].Trim());
                                            }

                                            Country country = null;
                                            if (countrieysDictionary.Keys.Contains(countryCode))
                                            {
                                                country = countrieysDictionary[countryCode];
                                            }

                                            State state = null;
                                            if (country != null)
                                            {

                                                if (statesDictionary.Keys.Contains(stateCode + ',' + country.Id))
                                                {
                                                    state = statesDictionary[stateCode + ',' + country.Id];
                                                }
                                            }

                                            //if doesn't exist add trucker with address and contact.
                                            if (!truckerCodesDect.Values.Contains((name + address1)))
                                            {

                                                AddressPM address = new AddressPM()
                                                {
                                                    Name = name,
                                                    Description = "Main Address",
                                                    Address1 = address1,
                                                    Address2 = address2,
                                                    ZipCode = zip,
                                                    StateId = state != null ? state.Id : null,
                                                    CountryId = country != null ? country.Id : null,
                                                    City = city,
                                                    PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null,
                                                    FaxNumber = fax,
                                                    AddressTypeId = "M",
                                                    Tenant = tenant,

                                                };


                                                TruckerPM trucker = new TruckerPM()
                                                {
                                                    EnglishName = truckerName,
                                                    VatNumber = vatNumber,
                                                    Tenant = tenant,
                                                    IsHybrid = true,
                                                    Code = code,//CodeCounter.GetNumber("Trucker", tenant).ToString(),
                                                    CarrierTypeId = "TR",
                                                    ReceivablesAccountingCard = receivablesAccountingCard,
                                                    PayablesAccountingCard = payablesAccountingCard,

                                                    //AccountingCard =accountingCards!=null?( accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards):null,

                                                };

                                                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                {
                                                    string contactEnglishName = contactName;
                                                    if (string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactEnglishName = email.Split('@')[0];
                                                    }

                                                    ContactPM contactPM = null;
                                                    if (!string.IsNullOrEmpty(email))
                                                    {
                                                        contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                    }
                                                    else if (!string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                    }
                                                    if (contactPM == null)
                                                    {
                                                        contactPM = new ContactPM()
                                                        {
                                                            //Id = IdCounter.GetNumber("Contact", tenant),
                                                            Email = email,
                                                            EnglishName = contactEnglishName,
                                                            Tenant = tenant,
                                                            CardId = "newCard",
                                                            IsCreatedWithPartner = true,
                                                            IsHybrid = true,

                                                        };
                                                        contacts.Add(contactPM);
                                                        ContactService contactService = new ContactService(objectContext, tenant);
                                                        contactService.Create(contactPM);
                                                        objectContext.SaveChanges();
                                                    }
                                                    contactPM.CardId = "newCard";


                                                    trucker.Contacts.Add(contactPM);

                                                }
                                                trucker.Addresses.Add(address);

                                                TruckerService service = new TruckerService(objectContext, trucker, systemContact.Id);

                                                service.Create(trucker);
                                                truckerCodesDect.Add(trucker.Id, trucker.EnglishName + address.Address1);
                                                objectContext.SaveChanges();

                                            }
                                            //update trucker.
                                            else
                                            {
                                                CacheManager.CacheWrapper = new MockCacheWrapper();
                                                if (truckers == null)
                                                {
                                                    truckers = truckerQuery.GetTruckerPMsByTenant(tenant).ToList();
                                                }
                                                if (addresses == null)
                                                {
                                                    addresses = addressQuery.GetAddressePMsByTenant(tenant).ToList();
                                                }
                                                string key = truckerCodesDect.Where(d => d.Value == (name + address1)).FirstOrDefault().Key;
                                                TruckerPM updatedTrucker = truckers.Where(d => d.Id == key).FirstOrDefault();
                                                if (updatedTrucker != null)
                                                {
                                                    if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                    {
                                                        ContactService contactService = new ContactService(otherObjectContext, tenant);
                                                        string contactEnglishName = contactName;
                                                        if (string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactEnglishName = email.Split('@')[0];
                                                        }
                                                        ContactPM contactPM = null;
                                                        if (!string.IsNullOrEmpty(email))
                                                        {
                                                            contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                        }
                                                        else if (!string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                        }
                                                        if (contactPM == null)
                                                        {
                                                            contactPM = new ContactPM()
                                                            {
                                                                //Id = IdCounter.GetNumber("Contact", tenant),
                                                                Email = email,
                                                                EnglishName = contactEnglishName,
                                                                Tenant = tenant,
                                                                CardId = key,
                                                                IsHybrid = true,
                                                            };
                                                            contacts.Add(contactPM);
                                                            contactService.Create(contactPM);
                                                            objectContext.SaveChanges();
                                                            updatedTrucker.Contacts.Add(contactPM);
                                                        }
                                                        else
                                                        {
                                                            if (contactPM.Id.Length <= 15)
                                                            {
                                                                contactPM.EnglishName = contactName;
                                                                contactPM.Email = email;
                                                                contactPM.IsHybrid = true;
                                                                contactService.Update(contactPM);
                                                                objectContext.SaveChanges();
                                                            }
                                                        }

                                                    }

                                                    AddressPM updatedAddress = addresses.Where(d => d.CardId == updatedTrucker.Id && d.AddressTypeId == "M").FirstOrDefault();
                                                    updatedAddress.Name = name;
                                                    updatedAddress.IsHybrid = true;
                                                    updatedAddress.Address2 = address2;
                                                    updatedAddress.ZipCode = zip;
                                                    updatedAddress.StateId = state != null ? state.Id : null;
                                                    updatedAddress.CountryId = country != null ? country.Id : null;
                                                    updatedAddress.City = city;
                                                    updatedAddress.PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null;
                                                    updatedAddress.FaxNumber = fax;
                                                    addressService.Update(updatedAddress);

                                                    updatedTrucker.EnglishName = truckerName;
                                                    updatedTrucker.VatNumber = vatNumber;
                                                    updatedTrucker.IsHybrid = true;
                                                    updatedTrucker.ReceivablesAccountingCard = receivablesAccountingCard;
                                                    updatedTrucker.PayablesAccountingCard = payablesAccountingCard;

                                                    //updatedTrucker.AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null;
                                                    updatedTrucker.Code = !string.IsNullOrEmpty(code) ? code : updatedTrucker.Code;
                                                    TruckerService service = new TruckerService(otherObjectContext, updatedTrucker, systemContact.Id);
                                                    service.Update(updatedTrucker);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            scope.Complete();
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        string Error = "";
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                            foreach (var ve in eve.ValidationErrors)
                            {

                                Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                            }
                        }

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                        {
                            file.WriteLine(Error);
                            file.Close();
                        }

                        status = "fail";

                    }
                    catch (Exception exception)
                    {
                        status = "fail";
                        string ErrorMessage = "";

                        if (exception != null)
                        {



                            ErrorMessage += exception.Message;

                            if (exception.InnerException != null)
                            {
                                ErrorMessage += Environment.NewLine + exception.InnerException.Message;

                                if (exception.InnerException.InnerException != null)
                                {
                                    ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                                    if (exception.InnerException.InnerException.InnerException != null)
                                    {
                                        ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                                    }
                                }
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                            {
                                file.WriteLine(ErrorMessage);
                                file.Close();
                            }

                        }

                    }

                    if (counter == 10)
                    {
                        objectContext = null;
                        objectContext = CommonDataContext.GetContext(tenant);
                        counter = 0;
                    }

                }


            }
            return status;
        }

        public string LoadWarehousesFromAFile(int tenant)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            ICommonDataContext otherObjectContext = CommonDataContext.GetContext(tenant);

            CountryRepository countryRep = new CountryRepository(otherObjectContext);
            StateRepository stateRep = new StateRepository(otherObjectContext);
            Dictionary<string, State> statesDictionary = stateRep.GetStates(tenant).ToDictionary(d => d.Code + ',' + d.CountryId, o => o);
            Dictionary<string, Country> countrieysDictionary = countryRep.GetCountries(tenant).ToDictionary(d => d.Code, o => o);

            ContactRepository contactRep = new ContactRepository(otherObjectContext);
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            Contact systemContact = contactRep.GetSingleContactByEmail(systemContactEmail, tenant);
            ContactQuery contactQuery = new ContactQuery(contactRep);
            List<ContactPM> contacts = contactQuery.GetContactPMsByTenant(tenant);
            string status = "success";

            WarehouseRepository warehouseRep = new WarehouseRepository(otherObjectContext);
            WarehouseQuery warehouseQuery = new WarehouseQuery(warehouseRep);
            List<WarehousePM> warehouses = null;
            AddressRepository addressRep = new AddressRepository(otherObjectContext);
            AddressQuery addressQuery = new AddressQuery(addressRep);
            List<AddressPM> addresses = null;
            AddressService addressService = new AddressService(otherObjectContext, tenant);

            var query = (from card in otherObjectContext.Cards
                         join address in otherObjectContext.Addresses on card.Id equals address.CardId
                         where card.PartnerTypeId == "WH" && address.AddressTypeId == "M" && card.Tenant == tenant
                         select new { Id = card.Id, Code = card.Code, Name = card.EnglishName, Address1 = address.Address1 });
            Dictionary<string, string> warehouseCodesDect = new Dictionary<string, string>();

            foreach (var rec in query)
            {
                warehouseCodesDect.Add(rec.Id, (rec.Name + rec.Address1));
            }

            CardRepository cardRep = new CardRepository(tenant);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog.FileName);
                string customrsString = sr.ReadToEnd();
                sr.Close();


                string[] stringLineArray = customrsString.Split('\n');
                string[] readWarehousesCodesData = null;
                

                int counter = 0;
                for (int i = 0; i < stringLineArray.Length; i = i + 10)
                {
                    counter++;
                    try
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(2, 0, 0) }))
                        {
                            for (int j = i; j < i + 10 && j < stringLineArray.Count(); j++)
                            {


                                if (!string.IsNullOrEmpty(stringLineArray[j]))
                                {
                                    stringLineArray[j] = stringLineArray[j].TrimEnd('\r');
                                    readWarehousesCodesData = stringLineArray[j].Split('\t');

                                    if (readWarehousesCodesData.Length >= 2)
                                    {
                                        if (readWarehousesCodesData[1].Length > 59)
                                            readWarehousesCodesData[1] = readWarehousesCodesData[1].Substring(0, 59);

                                        if (readWarehousesCodesData[1].Trim() != String.Empty)
                                        {
                                            // 0      1         2          3           4         5    6        7        8      9    10    11          12      13            14                           15
                                            //Type	Name	VAT Number	Address 1	Address 2	Zip	 City	State	Country	 Phone	Fax	 Email	Contact Name  code   ReceivablesAccountingCard   PayablesAccountingCard
                                            string type = readWarehousesCodesData[0].Trim() == "NULL" ? null : (readWarehousesCodesData[0]);
                                            string warehouseName = readWarehousesCodesData[1].Trim() == "NULL" ? null : (readWarehousesCodesData[1].Length > 59 ? readWarehousesCodesData[1].Substring(0, 59) : readWarehousesCodesData[1]);
                                            string name = readWarehousesCodesData[1].Trim() == "NULL" ? null : (readWarehousesCodesData[1].Length > 59 ? readWarehousesCodesData[1].Substring(0, 59) : readWarehousesCodesData[1]);
                                            string vatNumber = readWarehousesCodesData[2].Trim() == "NULL" ? null : (readWarehousesCodesData[2].Length > 19 ? readWarehousesCodesData[2].Substring(0, 19) : readWarehousesCodesData[2]);
                                            string address1 = readWarehousesCodesData[3].Trim() == "NULL" ? null : (readWarehousesCodesData[3].Length > 64 ? readWarehousesCodesData[3].Substring(0, 64) : readWarehousesCodesData[3]);
                                            string address2 = readWarehousesCodesData[4].Trim() == "NULL" ? null : (readWarehousesCodesData[4].Length > 64 ? readWarehousesCodesData[4].Substring(0, 64) : readWarehousesCodesData[4]);
                                            string zip = readWarehousesCodesData[5].Trim() == "NULL" ? null : (readWarehousesCodesData[5].Length > 14 ? readWarehousesCodesData[5].Substring(0, 14) : readWarehousesCodesData[5]);
                                            string city = readWarehousesCodesData[6].Trim() == "NULL" ? null : (readWarehousesCodesData[6].Length > 24 ? readWarehousesCodesData[6].Substring(0, 24) : readWarehousesCodesData[6]);
                                            string stateCode = readWarehousesCodesData[7].Trim() == "NULL" ? null : readWarehousesCodesData[7].Trim();
                                            string countryCode = readWarehousesCodesData[8].Trim() == "NULL" ? null : readWarehousesCodesData[8].Trim();
                                            string phone = readWarehousesCodesData[9].Trim() == "NULL" ? null : readWarehousesCodesData[9].Trim();
                                            string fax = readWarehousesCodesData[10].Trim() == "NULL" ? null : readWarehousesCodesData[10].Trim();
                                            string email = readWarehousesCodesData[11].Trim() == "NULL" ? null : readWarehousesCodesData[11].Trim();
                                            string contactName = readWarehousesCodesData[12].Trim() == "NULL" ? null : readWarehousesCodesData[12].Trim();

                                            string code = null;
                                            if (readWarehousesCodesData.Length >= 14)
                                            {
                                                code = readWarehousesCodesData[13].Trim() == "NULL" ? null : (readWarehousesCodesData[13].Trim());
                                            }

                                            string receivablesAccountingCard = null;
                                            if (readWarehousesCodesData.Length >= 15)
                                            {
                                                receivablesAccountingCard = readWarehousesCodesData[14].Trim() == "NULL" ? null : (readWarehousesCodesData[14].Trim());
                                            }

                                            string payablesAccountingCard = null;
                                            if (readWarehousesCodesData.Length >= 16)
                                            {
                                                payablesAccountingCard = readWarehousesCodesData[15].Trim() == "NULL" ? null : (readWarehousesCodesData[15].Trim());
                                            }

                                            Country country = null;
                                            if (countrieysDictionary.Keys.Contains(countryCode))
                                            {
                                                country = countrieysDictionary[countryCode];
                                            }

                                            State state = null;
                                            if (country != null)
                                            {

                                                if (statesDictionary.Keys.Contains(stateCode + ',' + country.Id))
                                                {
                                                    state = statesDictionary[stateCode + ',' + country.Id];
                                                }
                                            }

                                            //if doesn't exist add customer with address and contact.
                                            if (!warehouseCodesDect.Values.Contains((name + address1)))
                                            {

                                                AddressPM address = new AddressPM()
                                                {
                                                    Name = name,
                                                    Description = "Main Address",
                                                    Address1 = address1,
                                                    Address2 = address2,
                                                    ZipCode = zip,
                                                    StateId = state != null ? state.Id : null,
                                                    CountryId = country != null ? country.Id : null,
                                                    City = city,
                                                    PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null,
                                                    FaxNumber = fax,
                                                    AddressTypeId = "M",
                                                    Tenant = tenant,

                                                };

                                                WarehousePM warehouse = new WarehousePM()
                                                {
                                                    EnglishName = warehouseName,
                                                    VatNumber = vatNumber,
                                                    Tenant = tenant,
                                                    IsHybrid = true,
                                                    Code = code,//CodeCounter.GetNumber("Warehouse", tenant).ToString(),
                                                    PartnerTypeId = "WH",
                                                    ReceivablesAccountingCard = receivablesAccountingCard,
                                                    PayablesAccountingCard = payablesAccountingCard,
                                                   // AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null,
                                                };

                                                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                {
                                                    string contactEnglishName = contactName;
                                                    if (string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactEnglishName = email.Split('@')[0];
                                                    }

                                                    ContactPM contactPM = null;
                                                    if (!string.IsNullOrEmpty(email))
                                                    {
                                                        contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                    }
                                                    else if (!string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                    }
                                                    if (contactPM == null)
                                                    {
                                                        contactPM = new ContactPM()
                                                        {
                                                            //Id = IdCounter.GetNumber("Contact", tenant),
                                                            Email = email,
                                                            EnglishName = contactEnglishName,
                                                            Tenant = tenant,
                                                            CardId = "newCard",
                                                            IsCreatedWithPartner = true,
                                                            IsHybrid = true,

                                                        };
                                                        contacts.Add(contactPM);
                                                        ContactService contactService = new ContactService(objectContext, tenant);
                                                        contactService.Create(contactPM);
                                                        objectContext.SaveChanges();
                                                    }
                                                    contactPM.CardId = "newCard";


                                                    warehouse.Contacts.Add(contactPM);

                                                }
                                                warehouse.Addresses.Add(address);

                                                WarehouseService service = new WarehouseService(objectContext, warehouse, systemContact.Id);

                                                service.Create(warehouse);
                                                warehouseCodesDect.Add(warehouse.Id, warehouse.EnglishName + address.Address1);
                                                objectContext.SaveChanges();

                                            }
                                            //update customer.
                                            else
                                            {
                                                CacheManager.CacheWrapper = new MockCacheWrapper();
                                                if (warehouses == null)
                                                {
                                                    warehouses = warehouseQuery.GetWarehousePMsByTenant(tenant).ToList();
                                                }
                                                if (addresses == null)
                                                {
                                                    addresses = addressQuery.GetAddressePMsByTenant(tenant).ToList();
                                                }
                                                string key = warehouseCodesDect.Where(d => d.Value == (name + address1)).FirstOrDefault().Key;
                                                WarehousePM updatedWarehouse = warehouses.Where(d => d.Id == key).FirstOrDefault();
                                                if (updatedWarehouse != null)
                                                {
                                                    if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                    {
                                                        ContactService contactService = new ContactService(otherObjectContext, tenant);
                                                        string contactEnglishName = contactName;
                                                        if (string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactEnglishName = email.Split('@')[0];
                                                        }
                                                        ContactPM contactPM = null;
                                                        if (!string.IsNullOrEmpty(email))
                                                        {
                                                            contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                        }
                                                        else if (!string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                        }
                                                        if (contactPM == null)
                                                        {
                                                            contactPM = new ContactPM()
                                                            {
                                                                //Id = IdCounter.GetNumber("Contact", tenant),
                                                                Email = email,
                                                                EnglishName = contactEnglishName,
                                                                Tenant = tenant,
                                                                CardId = key,
                                                                IsHybrid = true,
                                                            };
                                                            contacts.Add(contactPM);
                                                            contactService.Create(contactPM);
                                                            objectContext.SaveChanges();
                                                            updatedWarehouse.Contacts.Add(contactPM);
                                                        }
                                                        else
                                                        {
                                                            if (contactPM.Id.Length <= 15)
                                                            {
                                                                contactPM.EnglishName = contactName;
                                                                contactPM.Email = email;
                                                                contactPM.IsHybrid = true;
                                                                contactService.Update(contactPM);
                                                                objectContext.SaveChanges();
                                                            }
                                                        }

                                                    }

                                                    AddressPM updatedAddress = addresses.Where(d => d.CardId == updatedWarehouse.Id && d.AddressTypeId == "M").FirstOrDefault();
                                                    updatedAddress.Name = name;
                                                    updatedAddress.IsHybrid = true;
                                                    updatedAddress.Address2 = address2;
                                                    updatedAddress.ZipCode = zip;
                                                    updatedAddress.StateId = state != null ? state.Id : null;
                                                    updatedAddress.CountryId = country != null ? country.Id : null;
                                                    updatedAddress.City = city;
                                                    updatedAddress.PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null;
                                                    updatedAddress.FaxNumber = fax;
                                                    addressService.Update(updatedAddress);

                                                    updatedWarehouse.EnglishName = warehouseName;
                                                    updatedWarehouse.VatNumber = vatNumber;
                                                    updatedWarehouse.IsHybrid = true;
                                                    //updatedWarehouse.AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null;
                                                    updatedWarehouse.PayablesAccountingCard = payablesAccountingCard;
                                                    updatedWarehouse.ReceivablesAccountingCard = receivablesAccountingCard;
                                                    updatedWarehouse.Code = !string.IsNullOrEmpty(code) ? code : updatedWarehouse.Code;
                                                    WarehouseService service = new WarehouseService(otherObjectContext, updatedWarehouse, systemContact.Id);
                                                    service.Update(updatedWarehouse);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            scope.Complete();
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        string Error = "";
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                            foreach (var ve in eve.ValidationErrors)
                            {

                                Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                            }
                        }

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                        {
                            file.WriteLine(Error);
                            file.Close();
                        }

                        status = "fail";

                    }
                    catch (Exception exception)
                    {
                        status = "fail";
                        string ErrorMessage = "";

                        if (exception != null)
                        {



                            ErrorMessage += exception.Message;

                            if (exception.InnerException != null)
                            {
                                ErrorMessage += Environment.NewLine + exception.InnerException.Message;

                                if (exception.InnerException.InnerException != null)
                                {
                                    ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                                    if (exception.InnerException.InnerException.InnerException != null)
                                    {
                                        ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                                    }
                                }
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                            {
                                file.WriteLine(ErrorMessage);
                                file.Close();
                            }

                        }

                    }

                    if (counter == 10)
                    {
                        objectContext = null;
                        objectContext = CommonDataContext.GetContext(tenant);
                        counter = 0;
                    }

                }


            }
            return status;
        }

        public string LoadVendorsFromAfile(int tenant)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            ICommonDataContext otherObjectContext = CommonDataContext.GetContext(tenant);

            CountryRepository countryRep = new CountryRepository(otherObjectContext);
            StateRepository stateRep = new StateRepository(otherObjectContext);
            Dictionary<string, State> statesDictionary = stateRep.GetStates(tenant).ToDictionary(d => d.Code + ',' + d.CountryId, o => o);
            Dictionary<string, Country> countrieysDictionary = countryRep.GetCountries(tenant).ToDictionary(d => d.Code, o => o);

            ContactRepository contactRep = new ContactRepository(otherObjectContext);
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            Contact systemContact = contactRep.GetSingleContactByEmail(systemContactEmail, tenant);
            ContactQuery contactQuery = new ContactQuery(contactRep);
            List<ContactPM> contacts = contactQuery.GetContactPMsByTenant(tenant);
            ContactPM cont = contacts.Where(d => d.Email == "imardo@il.loreal.com").FirstOrDefault();

            string status = "success";

            VendorRepository vendorRep = new VendorRepository(otherObjectContext);
            VendorQuery vendorQuery = new VendorQuery(vendorRep);
            List<VendorPM> vendors = null;
            AddressRepository addressRep = new AddressRepository(otherObjectContext);
            AddressQuery addressQuery = new AddressQuery(addressRep);
            List<AddressPM> addresses = null;
            AddressService addressService = new AddressService(otherObjectContext, tenant);

            var query = (from card in otherObjectContext.Cards
                         join address in otherObjectContext.Addresses on card.Id equals address.CardId
                         where card.PartnerTypeId == "VD" && address.AddressTypeId == "M" && card.Tenant == tenant
                         select new { Id = card.Id, Code = card.Code, Name = card.EnglishName, Address1 = address.Address1 });
            Dictionary<string, string> vendorCodesDect = new Dictionary<string, string>();

            foreach (var rec in query)
            {
                vendorCodesDect.Add(rec.Id, (rec.Name + rec.Address1));
            }

            CardRepository cardRep = new CardRepository(tenant);
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog.FileName);
                string customrsString = sr.ReadToEnd();
                sr.Close();


                string[] stringLineArray = customrsString.Split('\n');
                string[] readVendorsCodesData = null;

                int counter = 0;
                for (int i = 0; i < stringLineArray.Length; i = i + 10)
                {
                    counter++;
                    try
                    {
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(2, 0, 0) }))
                        {
                            for (int j = i; j < i + 10 && j < stringLineArray.Count(); j++)
                            {


                                if (!string.IsNullOrEmpty(stringLineArray[j]))
                                {
                                    stringLineArray[j] = stringLineArray[j].TrimEnd('\r');
                                    readVendorsCodesData = stringLineArray[j].Split('\t');

                                    if (readVendorsCodesData.Length >= 2)
                                    {
                                        if (readVendorsCodesData[1].Length > 59)
                                            readVendorsCodesData[1] = readVendorsCodesData[1].Substring(0, 59);

                                        if (readVendorsCodesData[1].Trim() != String.Empty)
                                        {
                                            string address1 = readVendorsCodesData[3].Trim() == "NULL" ? null : (readVendorsCodesData[3].Length > 64 ? readVendorsCodesData[3].Substring(0, 64) : readVendorsCodesData[3]);
                                            string name = readVendorsCodesData[1].Trim() == "NULL" ? null : (readVendorsCodesData[1].Length > 59 ? readVendorsCodesData[1].Substring(0, 59) : readVendorsCodesData[1]);


                                            // 0      1         2          3           4         5    6        7        8      9    10    11          12        13                              14
                                            //Type	Name	VAT Number	Address 1	Address 2	Zip	 City	State	Country	 Phone	Fax	 Email	Contact Name   ReceivablesAccountingCard   PayablesAccountingCard

                                            string type = readVendorsCodesData[0].Trim() == "NULL" ? null : (readVendorsCodesData[0]);
                                            string vendorName = readVendorsCodesData[1].Trim() == "NULL" ? null : (readVendorsCodesData[1].Length > 59 ? readVendorsCodesData[1].Substring(0, 59) : readVendorsCodesData[1]);
                                            string vatNumber = readVendorsCodesData[2].Trim() == "NULL" ? null : (readVendorsCodesData[2].Length > 19 ? readVendorsCodesData[2].Substring(0, 19) : readVendorsCodesData[2]);
                                            string address2 = readVendorsCodesData[4].Trim() == "NULL" ? null : (readVendorsCodesData[4].Length > 64 ? readVendorsCodesData[4].Substring(0, 64) : readVendorsCodesData[4]);
                                            string zip = readVendorsCodesData[5].Trim() == "NULL" ? null : (readVendorsCodesData[5].Length > 14 ? readVendorsCodesData[5].Substring(0, 14) : readVendorsCodesData[5]);
                                            string city = readVendorsCodesData[6].Trim() == "NULL" ? null : (readVendorsCodesData[6].Length > 24 ? readVendorsCodesData[6].Substring(0, 24) : readVendorsCodesData[6]);
                                            string stateCode = readVendorsCodesData[7].Trim() == "NULL" ? null : readVendorsCodesData[7].Trim();
                                            string countryCode = readVendorsCodesData[8].Trim() == "NULL" ? null : readVendorsCodesData[8].Trim();
                                            string phone = readVendorsCodesData[9].Trim() == "NULL" ? null : readVendorsCodesData[9].Trim();
                                            string fax = readVendorsCodesData[10].Trim() == "NULL" ? null : readVendorsCodesData[10].Trim();
                                            string email = readVendorsCodesData[11].Trim() == "NULL" ? null : readVendorsCodesData[11].Trim();
                                            string contactName = readVendorsCodesData[12].Trim() == "NULL" ? null : readVendorsCodesData[12].Trim();

                                            string receivablesAccountingCard = null;
                                            if (readVendorsCodesData.Length >= 14)
                                            {
                                                receivablesAccountingCard = readVendorsCodesData[13].Trim() == "NULL" ? null : (readVendorsCodesData[13].Trim());
                                            }

                                            string payablesAccountingCard = null;
                                            if (readVendorsCodesData.Length >= 15)
                                            {
                                                payablesAccountingCard = readVendorsCodesData[14].Trim() == "NULL" ? null : (readVendorsCodesData[14].Trim());
                                            }

                                            Country country = null;
                                            if (countrieysDictionary.Keys.Contains(countryCode))
                                            {
                                                country = countrieysDictionary[countryCode];
                                            }

                                            State state = null;
                                            if (country != null)
                                            {

                                                if (statesDictionary.Keys.Contains(stateCode + ',' + country.Id))
                                                {
                                                    state = statesDictionary[stateCode + ',' + country.Id];
                                                }
                                            }

                                            //if doesn't exist add Vendor with address and contact.
                                            if (!vendorCodesDect.Values.Contains((name + address1)))
                                            {

                                                AddressPM address = new AddressPM()
                                                {
                                                    Name = name,
                                                    Description = "Main Address",
                                                    Address1 = address1,
                                                    Address2 = address2,
                                                    ZipCode = zip,
                                                    StateId = state != null ? state.Id : null,
                                                    CountryId = country != null ? country.Id : null,
                                                    City = city,
                                                    PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null,
                                                    FaxNumber = fax,
                                                    AddressTypeId = "M",
                                                    Tenant = tenant,

                                                };


                                                VendorPM vendor = new VendorPM()
                                                {
                                                    EnglishName = vendorName,
                                                    VatNumber = vatNumber,
                                                    Tenant = tenant,
                                                    IsHybrid = true,
                                                    Code = CodeCounter.GetNumber("Vendor", tenant).ToString(),
                                                    PartnerTypeId = "VD",
                                                    ReceivablesAccountingCard = receivablesAccountingCard,
                                                    PayablesAccountingCard = payablesAccountingCard,
                                                    // AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null,


                                                };

                                                if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                {
                                                    string contactEnglishName = contactName;
                                                    if (string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactEnglishName = email.Split('@')[0];
                                                    }

                                                    ContactPM contactPM = null;
                                                    if (!string.IsNullOrEmpty(email))
                                                    {
                                                        contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                    }
                                                    else if (!string.IsNullOrEmpty(contactName))
                                                    {
                                                        contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                    }
                                                    if (contactPM == null)
                                                    {
                                                        contactPM = new ContactPM()
                                                        {
                                                            //Id = IdCounter.GetNumber("Contact", tenant),
                                                            Email = email,
                                                            EnglishName = contactEnglishName,
                                                            Tenant = tenant,
                                                            CardId = "newCard",
                                                            IsCreatedWithPartner = true,
                                                            IsHybrid = true,

                                                        };
                                                        contacts.Add(contactPM);
                                                        ContactService contactService = new ContactService(objectContext, tenant);
                                                        contactService.Create(contactPM);
                                                        objectContext.SaveChanges();
                                                    }
                                                    contactPM.CardId = "newCard";


                                                    vendor.Contacts.Add(contactPM);

                                                }
                                                vendor.Addresses.Add(address);

                                                VendorService service = new VendorService(objectContext, vendor, systemContact.Id);

                                                service.Create(vendor);
                                                vendorCodesDect.Add(vendor.Id, vendor.EnglishName + address.Address1);
                                                objectContext.SaveChanges();

                                            }
                                            //update customer.
                                            else
                                            {
                                                CacheManager.CacheWrapper = new MockCacheWrapper();
                                                if (vendors == null)
                                                {
                                                    vendors = vendorQuery.GetVendorPMsByTenant(tenant).ToList();
                                                }
                                                if (addresses == null)
                                                {
                                                    addresses = addressQuery.GetAddressePMsByTenant(tenant).ToList();
                                                }
                                                string key = vendorCodesDect.Where(d => d.Value == (name + address1)).FirstOrDefault().Key;
                                                VendorPM updatedVendor = vendors.Where(d => d.Id == key).FirstOrDefault();
                                                if (updatedVendor != null)
                                                {
                                                    if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(contactName))
                                                    {
                                                        ContactService contactService = new ContactService(otherObjectContext, tenant);
                                                        string contactEnglishName = contactName;
                                                        if (string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactEnglishName = email.Split('@')[0];
                                                        }
                                                        ContactPM contactPM = null;
                                                        if (!string.IsNullOrEmpty(email))
                                                        {
                                                            contactPM = contacts.Where(d => d.Email == email).FirstOrDefault();
                                                        }
                                                        else if (!string.IsNullOrEmpty(contactName))
                                                        {
                                                            contactPM = contacts.Where(d => d.EnglishName == contactName).FirstOrDefault();
                                                        }
                                                        if (contactPM == null)
                                                        {
                                                            contactPM = new ContactPM()
                                                            {
                                                                //Id = IdCounter.GetNumber("Contact", tenant),
                                                                Email = email,
                                                                EnglishName = contactEnglishName,
                                                                Tenant = tenant,
                                                                CardId = key,
                                                                IsHybrid = true,
                                                            };
                                                            contacts.Add(contactPM);
                                                            contactService.Create(contactPM);
                                                            objectContext.SaveChanges();
                                                            updatedVendor.Contacts.Add(contactPM);
                                                        }
                                                        else
                                                        {
                                                            if (contactPM.Id.Length <= 15)
                                                            {
                                                                contactPM.EnglishName = contactName;
                                                                contactPM.Email = email;
                                                                contactPM.IsHybrid = true;
                                                                contactService.Update(contactPM);
                                                                objectContext.SaveChanges();
                                                            }
                                                        }

                                                        AddressPM updatedAddress = addresses.Where(d => d.CardId == updatedVendor.Id && d.AddressTypeId == "M").FirstOrDefault();
                                                        updatedAddress.Name = name;
                                                        updatedAddress.IsHybrid = true;
                                                        updatedAddress.Address2 = address2;
                                                        updatedAddress.ZipCode = zip;
                                                        updatedAddress.StateId = state != null ? state.Id : null;
                                                        updatedAddress.CountryId = country != null ? country.Id : null;
                                                        updatedAddress.City = city;
                                                        updatedAddress.PhoneNumber = phone != null ? (phone.Length > 39 ? phone.Substring(0, 39) : phone) : null;
                                                        updatedAddress.FaxNumber = fax;
                                                        addressService.Update(updatedAddress);

                                                        updatedVendor.EnglishName = vendorName;
                                                        updatedVendor.VatNumber = vatNumber;
                                                        updatedVendor.IsHybrid = true;
                                                        updatedVendor.PayablesAccountingCard = payablesAccountingCard;
                                                        updatedVendor.ReceivablesAccountingCard = receivablesAccountingCard;
                                                        // updatedVendor.AccountingCard = accountingCards != null ? (accountingCards.Length > 24 ? accountingCards.Substring(0, 24) : accountingCards) : null;

                                                        VendorService service = new VendorService(otherObjectContext, updatedVendor, systemContact.Id);
                                                        service.Update(updatedVendor);

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            scope.Complete();
                        }
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        string Error = "";
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                            foreach (var ve in eve.ValidationErrors)
                            {

                                Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                            }
                        }

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                        {
                            file.WriteLine(Error);
                            file.Close();
                        }

                        status = "fail";

                    }
                    catch (Exception exception)
                    {
                        status = "fail";
                        string ErrorMessage = "";

                        if (exception != null)
                        {



                            ErrorMessage += exception.Message;

                            if (exception.InnerException != null)
                            {
                                ErrorMessage += Environment.NewLine + exception.InnerException.Message;

                                if (exception.InnerException.InnerException != null)
                                {
                                    ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                                    if (exception.InnerException.InnerException.InnerException != null)
                                    {
                                        ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                                    }
                                }
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@".\ErrorLogs.txt"))
                            {
                                file.WriteLine(ErrorMessage);
                                file.Close();
                            }

                        }

                    }

                    if (counter == 10)
                    {
                        objectContext = null;
                        objectContext = CommonDataContext.GetContext(tenant);
                        counter = 0;
                    }

                }


            }
            return status;
        }

    }
}
