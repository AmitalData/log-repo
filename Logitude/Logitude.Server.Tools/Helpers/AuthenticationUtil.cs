using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Server.Tools.Helpers
{
    public class AuthenticationUtil
    {
        

        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = HttpContext.Current.Request.UserHostAddress;
            }
            foreach (IPAddress IPA in Dns.GetHostAddresses(currentIP))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }
        
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
        
            return IP4Address;
        }
        


        public static string GetAuthenticatedUser()
        {
            if (HttpContext.Current != null)
            {
              
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken != null)
                            return authToken.Email;
                    }

                    throw new Exception("Sorry! this user is not authorized!");
                }
            }
            throw new Exception("Sorry! this user is not authorized!");
        }
        private string ResolveLoggingUserId1(int Tenant)
        {
            string systemEmail = SystemIdentityName(Tenant);
            //var contactQuery = new ContactQuery(tenant);
            var loggedContact = "";
            //contactQuery.GetContactByNameAndTenant(serviceContextUser, tenant, true);
            return loggedContact;
        }
        public static string SystemIdentityName(int Tenant)
        {
            string systemEmail = "system@tenant" + Tenant.ToString() + ".com";
            return systemEmail;
        }

        public static string ResolveSystemUserId(int Tenant)
        {
            string systemEmail = SystemIdentityName(Tenant);
            if (String.IsNullOrWhiteSpace(systemEmail))
            {
                return null;
            }

            ContactRepository contactRep = new ContactRepository(Tenant);
            var contact = contactRep.GetSingleContactByEmailAndTenant(systemEmail, Tenant);
            if (contact == null)
            {
                throw new BusinessErrorException("could not ResolveUserId from  Tenant");
            }
            return contact.Id;
        }

        public static void test()
        {
            var i = System.Threading.Thread.CurrentPrincipal.Identity;
            GenericIdentity identity = new GenericIdentity("M.Brown", "gg");
            //identity.IsAuthenticated = true;

            // ...

            System.Threading.Thread.CurrentPrincipal =
                new GenericPrincipal(
                    identity,
                    new string[] { "Role1", "Roll2" }
                    );

            //http://msdn.microsoft.com/en-us/library/ff647404.aspx
            var name = WindowsIdentity.GetCurrent().Name;

            //WindowsIdentity.Impersonate 
            //IClaimsPrincipal claimsPrincipal = HttpContext.Current.User as IClaimsPrincipal;
            HttpContext.Current.User = new GenericPrincipal(
                    identity,
                    new string[] { "Role1", "Roll2" }
                    ); ;
        }
        
        public static string ResolveUserIdentityName(int Tenant)
        {
            string systemEmail = SystemIdentityName(Tenant);
            string defaultName = "";
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    defaultName = HttpContext.Current.User.Identity.Name;
                }
                
            }

            if (String.IsNullOrWhiteSpace(defaultName))
            {
                defaultName = GetThreadCurrentPrincipalIdentityName(); ;// Thread.CurrentPrincipal.Identity.Name;
            }
            if (String.IsNullOrWhiteSpace(defaultName))
            {
                defaultName = systemEmail;
            }
            return defaultName;
        }
        public static string ResolveUserId(int Tenant)
        {
            ContactRepository contactRep = new ContactRepository(Tenant);

            string resolveUserIdentityName = AuthenticationUtil.ResolveUserIdentityName(Tenant);


            var contact = contactRep.GetSingleContactByEmail(resolveUserIdentityName, Tenant, false);
            if (contact == null)
            {
                throw new BusinessErrorException("could not ResolveUserId from  Tenant");
            }
            return contact.Id;
        }
        public static string ResolveUnifreightUserId(int Tenant)
        {
            //<--- Yuval Chalup 16.06.2015 TASK-13974 - CHANGED (client to user) FROM:
            //ContactRepository contactRep = new ContactRepository(Tenant);

            //string resolveUserIdentityName = AuthenticationUtil.ResolveUserIdentityName(Tenant);

            //var contact = contactRep.GetSingleContactByEmail(resolveUserIdentityName, Tenant, true);
            //if (contact == null)
            //{
            //    return null;
            //}
            //if (!String.IsNullOrWhiteSpace(contact.ExternalId))
            //{
            //    return contact.ExternalId;
            //}
            //return null;//system
            //TO:
            string resolveUserIdentityName = AuthenticationUtil.ResolveUserIdentityName(Tenant);

            UserRepository userRep = new UserRepository(Tenant);
            User user = userRep.GetSingleUserByEmail(resolveUserIdentityName, Tenant, true);
            if (user == null)
            {
                return null;
            }
            if (!String.IsNullOrWhiteSpace(user.Code))
            {
                return user.Code;
            }
            return null;//system
            //Yuval Chalup 16.06.2015 TASK-13974 --->
        }

        public static string ResolveUserIdOfUnifreightUser(int Tenant)//Eitan H task 35737
        {
            string resolveUserIdentityName = AuthenticationUtil.ResolveUserIdentityName(Tenant);

            UserRepository userRep = new UserRepository(Tenant);
            User user = userRep.GetSingleUserByEmail(resolveUserIdentityName, Tenant, true);
            if (user == null)
            {
                return null;
            }
            if (!String.IsNullOrWhiteSpace(user.Id))
            {
                return user.Id;
            }
            return null;//system
        }
        public static string ResolveUnifreightUserById(string Id, int Tenant)
        {
            ContactRepository contactRep = new ContactRepository(Tenant);
            var contact = contactRep.GetSingleContact(Id, Tenant);
            if (contact == null)
            {
                return null;
            }
            if (!String.IsNullOrWhiteSpace(contact.ExternalId))
            {
                return contact.ExternalId;
            }
            return null;//system
        }
        
        //[PrincipalPermission(SecurityAction.Demand,Role = "Role1")]
        public static bool UnifreightImpersonateOld(string MoreParams ,out int iTenanat ,out string contactEmail )
        {
            iTenanat = -999;
            contactEmail = "";
            try
            {
                //test();
                var unifreightListsParams = UnifreightListsUtil.Deserialize(MoreParams);
                var tenant = UnifreightListsUtil.GetValue(ref unifreightListsParams, "tenant");
                var UniUser = UnifreightListsUtil.GetValue(ref unifreightListsParams, "UNIFREIGHT_USER_ID");
                if (String.IsNullOrWhiteSpace(tenant) || string.IsNullOrWhiteSpace(UniUser))
                {
                    return false;
                }
                
                if (!int.TryParse(tenant, out iTenanat))
                {
                    return false;
                }
                

                ContactRepository contactRep = new ContactRepository(iTenanat);
                
                var contact = contactRep.GetSingleContactByExternalId(UniUser, iTenanat);
                
                if (contact == null)
                {
                    //yaron say int system !!!   ///throw new BusinessErrorException("UniUser, iTenanat not exist ");
                    //yaron say int system !!!   return false;
                    contactEmail = SystemIdentityName(iTenanat);

                }
                else
                {
                    contactEmail= contact.Email;
                }



                GenericIdentity identity = new GenericIdentity(contactEmail, "UnifreightImpersonate");

                
                

                var myPrincipal = new GenericPrincipal(
                    identity,
                    new string[] { "UnifreightRole", "TenantRole=" + tenant }
                    );
                
                

                
                AppDomain.CurrentDomain.SetThreadPrincipal(myPrincipal);
                


                System.Threading.Thread.CurrentPrincipal = myPrincipal; //myPrincipal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = myPrincipal;
                        
                }
                
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public static bool UnifreightImpersonate(string MoreParams, out int iTenanat, out string contactEmail)
        {
            iTenanat = -999;
            contactEmail = "";
            try
            {
                //test();
                var unifreightListsParams = UnifreightListsUtil.Deserialize(MoreParams);
                var tenant = UnifreightListsUtil.GetValue(ref unifreightListsParams, "tenant");
                var UniUser = UnifreightListsUtil.GetValue(ref unifreightListsParams, "UNIFREIGHT_USER_ID");
                if (String.IsNullOrWhiteSpace(tenant) || string.IsNullOrWhiteSpace(UniUser))
                {
                    return false;
                }

                if (!int.TryParse(tenant, out iTenanat))
                {
                    return false;
                }


                ///ContactRepository contactRep = new ContactRepository(iTenanat);
                var userRepository = new UserRepository(iTenanat);
                var //contact =
                    user =
                    //contactRep.GetSingleContactByExternalId(UniUser, iTenanat);
                    userRepository.GetSingleUserByCode(UniUser, iTenanat, false);

                if (user == null)
                {
                    //yaron say int system !!!   ///throw new BusinessErrorException("UniUser, iTenanat not exist ");
                    //yaron say int system !!!   return false;
                    contactEmail = SystemIdentityName(iTenanat);

                }
                else
                {
                    if (user.Contact != null)
                    {
                        contactEmail = user.Contact.Email;
                    }
                    else
                    {
                        contactEmail = SystemIdentityName(iTenanat);
                    }
                    
                }


                return Impersonate(iTenanat, contactEmail, UniUser);
            }
            catch (Exception)
            {

                return false;
            }

        }

        public  static bool Impersonate(int iTenanat, string contactEmail, string UniUser)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var claims = new List<Claim>
                {
                    new Claim("UniUser", UniUser),
                    new Claim("Tenant", iTenanat.ToString()  ),
                    new Claim(ClaimTypes.Name, contactEmail),
                    new Claim(ClaimTypes.Email, contactEmail),
                    
                };
            var claimIdentity = new ClaimsIdentity(claims);

            claimsPrincipal.AddIdentity(claimIdentity);
            Thread.CurrentPrincipal = claimsPrincipal;







            AppDomain.CurrentDomain.SetThreadPrincipal(claimsPrincipal);



            System.Threading.Thread.CurrentPrincipal = claimsPrincipal; //myPrincipal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = claimsPrincipal;

            }

            return true;
        }
        public static int? GetThreadCurrentPrincipalTenant()
        {

            if (Thread.CurrentPrincipal.Identity == null)
            {
                return null;
            }
            Claim tenant = ClaimsPrincipal.Current.FindFirst("Tenant");
            if (tenant == null)
            {
                return null;
            }
            var sTenant = tenant.Value;
            var i=int.Parse(sTenant);
            return i;
        }

        public static string GetThreadCurrentPrincipalIdentityName()
        {

            if (Thread.CurrentPrincipal.Identity == null)
            {
                return null;
            }
            return Thread.CurrentPrincipal.Identity.Name;
        }

        public static void DebugUsers()
        {

            var principal = ClaimsPrincipal.Current;

            //Claim userName = principal.FirstOrDefault("UserName");
            
            var claimsEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            bool hasEmailClaim = principal.HasClaim(c => c.Type == ClaimTypes.Email);
            if (hasEmailClaim)
            {
                Debug.WriteLine("hasEmailClaim:" + claimsEmail.Value);
            }

            Debug.WriteLine("GetCurrentPrincipalTenant():" );
            Debug.Write(GetThreadCurrentPrincipalTenant());


            Debug.WriteLine("UserName:" +Environment.UserName);
            Debug.WriteLine("Thread.CurrentPrincipal.Identity.Name:" + Thread.CurrentPrincipal.Identity.Name);
            if (HttpContext.Current != null)
            {
                Debug.WriteLine("HttpContext.Current.User.Identity.Name:" + HttpContext.Current.User.Identity.Name);
            }
            
            Debug.WriteLine("WindowsIdentity.GetCurrent().Name:" + WindowsIdentity.GetCurrent().Name);
        }

        public static bool IsResolveUserIdentityNameEqualSystem(int tenant)
        {
            var resolveUserIdentityName = ResolveUserIdentityName(tenant);
            var systemIdentityName = SystemIdentityName(tenant);
            return resolveUserIdentityName.Equals(systemIdentityName, StringComparison.OrdinalIgnoreCase);
        }

        public static string ResolveLoggingUserId(int Tenant)
        {
            string systemEmail = "system@tenant" + Tenant.ToString() + ".com";
            string defaultName = "";
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    defaultName = HttpContext.Current.User.Identity.Name;
                }
            }
            if (String.IsNullOrWhiteSpace(defaultName))
            {
                defaultName = systemEmail;
            }
            return defaultName;
        }

        public static bool IsAuthenticatedUserExists()
        {
            bool exists = false;
            if (HttpContext.Current != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    exists = true;
                }
            }

            return exists;
        }

        public static ContactPassword VerifyContactPassword(string email, string password, IGlobalContext globalContext , bool isHashPassword = false)
        {
            ContactPassword contactPassword = null;
            if (globalContext != null && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                email = email.ToLower();
                string hashedOldPassword = !isHashPassword ? PasswordGenerator.GetHashedPassword(email, password) : password;
                contactPassword = globalContext.ContactPasswords.Where(c => c.Email == email && c.Password == hashedOldPassword && !c.IsBCrypt).FirstOrDefault();

                if (contactPassword == null)
                {
                    contactPassword = globalContext.ContactPasswords.Where(c => c.Email.ToLower() == email && c.IsBCrypt).FirstOrDefault();
                    if (contactPassword != null)
                    {
                        var isValid = false;
                        if (!isHashPassword)
                        {
                            isValid = PasswordGenerator.VerifyBCryptHashedPassword(contactPassword.Email, password, contactPassword.Password);
                        }

                        else
                        {
                            isValid = contactPassword.Password == password ? true : false;
                        }
                        if (!isValid)
                        {
                            contactPassword = null;
                        }
                    }
                }
            }

            return contactPassword;
        }

        public static string GenerateToken()
        {
            int length = 26;
            string token = "";
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            byte[] rndArray = new byte[length];
            generator.GetBytes(rndArray);
            token = Convert.ToBase64String(rndArray);

            return token;
        }
    }
}
