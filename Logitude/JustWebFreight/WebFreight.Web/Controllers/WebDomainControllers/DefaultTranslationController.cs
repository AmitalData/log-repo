using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class DefaultTranslationController : ApiController
    {
        public HttpResponseMessage Post(DefaultTranslationAPIHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    //testing 
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if(args.UpdatedTextCodes == null)
                    {
                        args.UpdatedTextCodes = new List<TextCodePM>();
                    }

                    if (args.UpdatedTextCodes.Count > 0)
                    {
                        string tenantCodesListName = "tenanttextcodes" + tenant;
                        string zeroCodeslistName = "tenantzerotextcodes";

                        if (HttpContext.Current != null)
                        {
                            if (CacheManager.CacheWrapper.Get(tenantCodesListName) != null)
                            {
                                CacheManager.CacheWrapper.Invalidate(tenantCodesListName);
                            }

                            if (CacheManager.CacheWrapper.Get(zeroCodeslistName) != null)
                            {
                                CacheManager.CacheWrapper.Invalidate(zeroCodeslistName);
                            }
                        }

                        IWebFreightContext ObjectContext = WebFreightContext.GetContext(tenant);
                        TextCodeService service = new TextCodeService(ObjectContext, tenant);

                        foreach (TextCodePM item in args.UpdatedTextCodes)
                        {
                            service.Update(item);
                        }

                        args.UpdatedTextCodes = new List<TextCodePM>();
                    }

                    if (!args.IsUpdatingOnly)
                    {
                        TextCodeRepository myTextCodeRepository = new TextCodeRepository(tenant);
                        TextCodeQuery myTextCodeQuery = new TextCodeQuery(myTextCodeRepository);

                        if (args.IsNewSearching)
                        {
                            args.Count = myTextCodeRepository.GetTextCodesCountForDefaultTranslation(tenant, args.ObjectTableId, args.SpellCheckedFilterCode, args.TextCodeTypeCode, args.SelectedCheckDate, args.CheckDateFilerCode, args.SearchText);
                        }

                        args.TextCodes = myTextCodeQuery.GetFilteredTextCodesForDefaultTranslation(tenant, args.ObjectTableId, args.SpellCheckedFilterCode, args.TextCodeTypeCode, args.SelectedCheckDate, args.CheckDateFilerCode, args.SearchText, args.SkipDigit, args.TakeDigit);
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //[HttpGet]
        //public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        //{

        //}
    }

    public class DefaultTranslationAPIHelper
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public List<TextCodePM> TextCodes { get; set; }
        public List<TextCodePM> UpdatedTextCodes { get; set; }
        public string ObjectTableId { get; set; }
        public string SpellCheckedFilterCode { get; set; }
        public string CheckDateFilerCode { get; set; }
        public string TextCodeTypeCode { get; set; }
        public DateTime? SelectedCheckDate { get; set; }
        public string SearchText { get; set; }
        public int SkipDigit { get; set; }
        public int TakeDigit { get; set; }
        public bool IsUpdatingOnly { get; set; }
        public bool IsNewSearching { get; set; }
    }
}