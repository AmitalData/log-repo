using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.Web;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;

using WebFreight.Web.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.Security
{
    [EnableClientAccess]
    public class SimplogAuthinticationService : FormsAuthinticationService<UserData>
    {
        private static readonly string SimplogGuid = Guid.NewGuid().ToString("N");



        protected override UserData GetCurrentUser(string name, string userData)
        {
            string[] userDataParts = userData.Split(':');

            return new UserData()
            {
                UserName = name,
                Id = userDataParts[2]

            };
        }

        protected override UserData GetDefaultUser()
        {

            return new UserData() { Id = "default", Name = "default" };
            //return base.GetDefaultUser();
        }
        protected override UserData ValidateCredentials(string name, string password, string customData, out string userData)
        {

            UserData user = null;
            userData = null;
            if (name.Contains("system@"))
            {
                return user;
            }


            GlobalContact member = null;
            Card card = null;
            //CardContact cardContact = null;
            int tenant = -1;

            string computerId = null;
            if (customData != null)
            {
                string[] customdataArray = customData.Split(',');

                int.TryParse(customdataArray[0], out tenant);
                computerId = customdataArray[1];


            }
            else
            {
            }


            //ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);

            string hashedPassword = PasswordGenerator.GetHashedPassword(name, password);

            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            GlobalContact contact = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.InActive == false && m.GlobalTenantId == tenant).FirstOrDefault();
            if (contact != null)
            {
                bool isIpAuthenticated = true;
                if (contact.Email == "customercare@logitudeworld.com")
                {
                    string ipstring = LogitudeSettings.CustomerCareIP;//System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
                    string[] authenticatedIPs = ipstring.Split(',');
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    if (!authenticatedIPs.Contains(currentIP))
                    {
                        isIpAuthenticated = false;
                    }

                }

                    if (isIpAuthenticated)
                    {
                        ContactPassword contactPassword =AuthenticationUtil.VerifyContactPassword(name, password, globalObjectContext);

                    if (contactPassword != null)
                        {
                            if (!contactPassword.IsLocked && !contactPassword.MustChangePassword)
                            {
                                IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);
                                member = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.InActive == false && (m.IsUser == true) && m.GlobalTenantId == tenant).FirstOrDefault();//|| m.InternetAccess == true




                            if (member != null)
                            {
                                //cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == member.Id).FirstOrDefault();
                                //if (cardContact != null)
                                //{
                                //    card = commonDataContext.Cards.Where(d => d.Id == cardContact.CardId).FirstOrDefault();
                                //}

                                if (contactPassword.NumberOfRetries > 0)
                                {
                                    contactPassword.NumberOfRetries = 0;

                                    globalObjectContext.SaveChanges();
                                }

                                //RoleRepository rolerep = new RoleRepository(tenant);


                                RoleQuery roleQuery = new RoleQuery(tenant);
                                List<RolePM> allRoles = roleQuery.GetRolesForContact(contact.Id, contact.GlobalTenantId).ToList();
                                List<RolePM> allCustomRoles = allRoles.Where(d => d.IsCustomRole == true).ToList();

                                List<string> allRolesIds = allRoles.Select(s => s.Id).ToList();

                                foreach (RolePM item in allCustomRoles)
                                {
                                    if (allRolesIds.Contains(item.ParentRoleId))
                                    {
                                        allRolesIds.Remove(item.ParentRoleId);
                                    }
                                }

                                ContactInfo myContactInfo = new ContactInfo()
                                {
                                    Tenant = contact.GlobalTenantId,
                                    ContactEmail = contact.Email,
                                    RolesIds = allRolesIds,
                                };

                                CacheManager.CacheWrapper.Insert(contact.Email + contact.GlobalTenantId, myContactInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }

                            else
                            {



                                //GlobalContact globalContact = globalObjectContext.GlobalContacts.Where(m => m.Email == name && m.InActive == false && (m.IsUser == true) && m.GlobalTenantId == tenant).FirstOrDefault();//|| m.InternetAccess == true

                                //if (globalContact != null)
                                //{
                                contactPassword.NumberOfRetries++;
                                if (contactPassword.NumberOfRetries == 5)
                                {
                                    contactPassword.IsLocked = true;
                                }

                                globalObjectContext.SaveChanges();

                                //}

                            }
                        }
                    }
                }
                else
                {
                    //user = new UserData()
                    //{
                    //    IsLockedUser = true,
                    //    Id = "",
                    //    Name = "",
                    //    UserName = ""


                    //};
                }
            }


            if (member != null)
            {
                if (card != null)
                {
                    user = new UserData()
                    {
                        UserName = name,
                        Id = member.Id,
                        Name = name,
                        //Roles = member.Roles,
                        CardId = card.Id,
                        CardType = card.PartnerTypeId,
                        CurrentTenant = member.GlobalTenantId,
                        //IsUser = member.IsUser,


                    };
                }
                else
                {
                    user = new UserData()
                    {
                        UserName = name,
                        Id = member.Id,
                        Name = name,
                        //  Roles = member.Roles,
                        CurrentTenant = member.GlobalTenantId,
                        //IsUser = member.IsUser,


                    };

                }
            }

            if (user != null)
            {


                string userAgent = HttpContext.Current.Request.UserAgent.Length <= 500 ? HttpContext.Current.Request.UserAgent : HttpContext.Current.Request.UserAgent.Substring(0, 500);
                //if (user.IsUser)
                //{


                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(contact.GlobalTenantId);
                    UserLastLogin lastLogin = (from a in commonDataContext.UserLastLogins
                                               where a.Id == user.Id
                                               select a).FirstOrDefault();
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    UserLoginLog userLog = new UserLoginLog()
                    {
                        Id = IdCounter.GetNumber("UserLoginLog", tenant).ToString(),
                        Tenant = tenant,
                        Browser = HttpContext.Current.Request.Browser.Type,
                        IP = currentIP,
                        UserId = user.Id,
                        GMTDateTime = DateTime.Now,
                        LocalDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                        UserAgent = userAgent,
                        ComputerId = computerId,
                    };

                    if (lastLogin == null)
                    {
                        lastLogin = new UserLastLogin()
                        {
                            Id = user.Id,
                            Tenant = tenant,
                        };

                        commonDataContext.UserLastLogins.Add(lastLogin);
                    }

                    lastLogin.ComputerId = computerId;
                    lastLogin.WorkEnvironment = LogitudeSettingConfigration.GetWorkEnvironment();
                    lastLogin.LoginDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);


                    commonDataContext.UserLoginLogs.Add(userLog);
                    commonDataContext.SaveChanges();
                    scope.Complete();
                }


                //}

                if (card != null)
                {
                    userData = member.Email + ":" + member.Id + ":" + card.Id + ":" + card.PartnerTypeId + SimplogGuid;
                }
                else
                {
                    if (member != null)
                    {
                        userData = member.Email + ":" + member.Id + ":" + SimplogGuid;
                    }
                    //else
                    //{
                    //    userData = "" + ":" + "" + ":" + BookClubGuid;
                    //}
                }
            }
            return user;
        }
        // To enable Forms/Windows Authentication for the Web Application, 
        // edit the appropriate section of web.config file.




    }



}
