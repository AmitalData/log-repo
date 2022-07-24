

using Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.BL.Validators;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Customs.BL.TraceEvents;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Simplog.Data.CommonDataModel;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class ExportCloudSSOService : UnifreightGenericService
    {
        private ICustomContext _context;

        
        private Stopwatch _Stopwatch;

        public ExportCloudSSOService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string DataIn1,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyGenericResponseObj.Stage = "Deserialize";

            MyCommunicationsParams.Subject = "ExportCloudSSOService ";
            var dic = UnifreightListsUtil.Deserialize(DataIn1);

            MustAdmin();

            
            string email=UnifreightListsUtil.GetValue(ref dic, "EMAIL");
            //UnifreightListsUtil.GetValue(ref dic, "tenant");
            MyGenericResponseObj.Stage = "check";
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("email is must");
            }

            ContactRepository contactRep = new ContactRepository();


            var contact = contactRep.GetSingleContactByEmail(email, ResolvedTenant(), true);
            if (contact?.Id == null)
            {
                throw new Exception($"contact {email} is missing in tenant  {ResolvedTenant()}");
            }
            if (contact.InActive)
            {
                throw new Exception($"contact {email} InActive in tenant  {ResolvedTenant()}");
            }


            MyGenericResponseObj.Stage = "Just Do IT";


            
            
                
            string token = AuthenticationUtil.GenerateToken();
            AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
            AuthenticationToken authentication = new AuthenticationToken() { CreateDate = DateTime.Now, Email = email, Password = email, Token = token };
            authenticationTokenRepository.Add(authentication);
                
            authenticationTokenRepository.SubmitChanges();
            MyGenericResponseObj.ApplicationId = token;

                
                
            

            AppendLogLine("send request:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "Done All ";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

        }

        protected override int ResolvedTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

            return authToken.Tenant;
        }
        private static void MustAdmin()
        {
            //string token = HttpContext.Current.Request.Headers["Token"];
            //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            //ecurityUtility.CheckContactFeature("Customs.Declaration", "READ", authToken.Tenant);
        }

  
     


        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            return "";
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }


    }

}

