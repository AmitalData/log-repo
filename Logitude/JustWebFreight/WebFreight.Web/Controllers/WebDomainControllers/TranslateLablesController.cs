using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TranslateLablesController : ApiController
    {
        public HttpResponseMessage Post(TranslateLabelsAPIHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    this.GetTranslationsByParam(tenant, args);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private void GetTranslationsByParam(int translationTenant, TranslateLabelsAPIHelper args)
        {
            int tenant = translationTenant;
            IWebFreightContext myContext = WebFreightContext.GetContext(tenant);
            TextCodeRepository TextCodeRepository = new TextCodeRepository(myContext);
            TranslationRepository TranslationRepository = new TranslationRepository(myContext);

            #region TextCodes
            IQueryable<TextCode> iQueryable_TextCodes = null;

            if (args.SpellCheckedFilterCode == "True")
            {
                iQueryable_TextCodes = (from a in myContext.Translations.Include("TextCode")
                                        where a.Tenant == tenant || a.Tenant == 0
                                        select a.TextCode);
            }

            else
            {
                iQueryable_TextCodes = TextCodeRepository.GetTextCodesByTenant(tenant);
            }

            iQueryable_TextCodes = iQueryable_TextCodes.Where(d => d.TextCodeTypeCode != "T" && d.InActive == false);

            if (!string.IsNullOrEmpty(args.TextCodeTypeCode) && args.TextCodeTypeCode != "All")
            {
                iQueryable_TextCodes = iQueryable_TextCodes.Where(d => d.TextCodeTypeCode == args.TextCodeTypeCode);
            }

            if (!string.IsNullOrEmpty(args.ObjectTableId))
            {
                iQueryable_TextCodes = iQueryable_TextCodes.Where(d => d.ObjectTableId == args.ObjectTableId);
            }

            if (!string.IsNullOrEmpty(args.SearchText))
            {
                iQueryable_TextCodes = from d in iQueryable_TextCodes
                                       where
                                       (!string.IsNullOrEmpty(d.Code) && d.Code.ToUpper().Contains(args.SearchText.ToUpper()))
                                       ||
                                       (!string.IsNullOrEmpty(d.DefaultText) && d.DefaultText.ToUpper().Contains(args.SearchText.ToUpper()))
                                       select d;
            }
            #endregion

            #region Translations
            IQueryable<Translation> iQueryable_Translation = (from a in myContext.Translations
                                                              where a.TranslationHeaderCode == args.Language
                                                              && (a.Tenant == 0 || a.Tenant == tenant)
                                                              select a);

            if (args.SelectedCheckDate != null && args.CheckDateFilerCode != "None")
            {
                if (args.CheckDateFilerCode == "Equals")
                {
                    iQueryable_Translation = iQueryable_Translation.Where(d => d.TranslateDate == args.SelectedCheckDate);
                }

                else if (args.CheckDateFilerCode == "Bigger")
                {
                    iQueryable_Translation = iQueryable_Translation.Where(d => d.TranslateDate > args.SelectedCheckDate);
                }

                else if (args.CheckDateFilerCode == "Less")
                {
                    iQueryable_Translation = iQueryable_Translation.Where(d => d.TranslateDate < args.SelectedCheckDate);
                }
            }
            #endregion


            #region FieldsTranslations
            List<FieldsTranslations> myResult = new List<FieldsTranslations>();

            List<Translation> list_Translation = iQueryable_Translation.ToList();
            List<TextCode> list_TextCodes = iQueryable_TextCodes.OrderBy(d => d.Code).Skip(args.SkipDigit).Take(args.TakeDigit).ToList();

            foreach (TextCode item in list_TextCodes)
            {
                FieldsTranslations Record = new FieldsTranslations();
                Record.Tenant = item.Tenant;
                Record.DefaultText = item.DefaultText;
                Record.DefaultTextPlural = item.DefaultTextPlural;
                Record.Code = item.Code;
                Record.TextCodeId = item.Id;
                Record.TypeCode = item.TextCodeTypeCode;
                Record.ObjectTableID = item.ObjectTableId;
                Record.TranslationTenent = translationTenant;
                Record.TranslationLanguageCode = args.Language;
                Record.ObjectTableName = item.ObjectTable.Name;
                Record.ObjectTableTypeCode = item.ObjectTable.ObjectTableTypeCode;
                Record.TranslatedText = item.DefaultText;
                Record.TranslatedTextPlural = item.DefaultTextPlural;

                Translation translaion = list_Translation.Where(d => d.TextCodeId == item.Id && d.Tenant == tenant).FirstOrDefault();
                if (translaion == null)
                {
                    list_Translation.Where(d => d.TextCodeId == item.Id && d.Tenant == 0).FirstOrDefault();
                }

                if (translaion != null)
                {
                    Record.IsTranslated = true;
                    Record.TranslateDate = translaion.TranslateDate;
                    Record.TranslatedText = translaion.TranslatedText;
                    Record.TranslatedByUserId = translaion.TranslatedByUserId;
                    Record.TranslatedTextPlural = translaion.TranslatedTextPlural;
                }

                myResult.Add(Record);
            }
            #endregion

            args.CountAll = iQueryable_TextCodes.Count();
            args.Translations = myResult.OrderBy(d => d.Code).Skip(args.SkipDigit).Take(args.TakeDigit).ToList();
        }

        private void GetTranslationsByParam_ERR(int translationTenant, TranslateLabelsAPIHelper args)
        {
            int tenant = translationTenant;
            IWebFreightContext myContext = WebFreightContext.GetContext(tenant);
            TextCodeRepository TextCodeRepository = new TextCodeRepository(myContext);
            TranslationRepository TranslationRepository = new TranslationRepository(myContext);

            #region TextCodes
            IQueryable<TextCode> iQueryable_TextCodes = null;

            if (args.SpellCheckedFilterCode == "True")
            {
                iQueryable_TextCodes = (from a in myContext.Translations.Include("TextCode")
                                        where a.Tenant == tenant || a.Tenant == 0
                                        select a.TextCode);
            }

            else
            {
                iQueryable_TextCodes = TextCodeRepository.GetTextCodesByTenant(tenant);
            }

            iQueryable_TextCodes = iQueryable_TextCodes.Where(d => d.TextCodeTypeCode != "T" && d.InActive == false);

            if (!string.IsNullOrEmpty(args.TextCodeTypeCode) && args.TextCodeTypeCode != "All")
            {
                iQueryable_TextCodes = iQueryable_TextCodes.Where(d => d.TextCodeTypeCode == args.TextCodeTypeCode);
            }

            if (!string.IsNullOrEmpty(args.ObjectTableId))
            {
                iQueryable_TextCodes = iQueryable_TextCodes.Where(d => d.ObjectTableId == args.ObjectTableId);
            }

            if (!string.IsNullOrEmpty(args.SearchText))
            {
                iQueryable_TextCodes = from d in iQueryable_TextCodes
                                       where
                                       (!string.IsNullOrEmpty(d.Code) && d.Code.ToUpper().Contains(args.SearchText.ToUpper()))
                                       ||
                                       (!string.IsNullOrEmpty(d.DefaultText) && d.DefaultText.ToUpper().Contains(args.SearchText.ToUpper()))                                       
                                       select d;
            }
            #endregion

            #region Translations
            IQueryable<Translation> iQueryable_Translation0 = (from a in myContext.Translations.Include("TextCode") where a.Tenant == 0 select a);
            IQueryable<Translation> iQueryable_Translation1 = (from a in myContext.Translations.Include("TextCode") where a.Tenant == tenant select a);

            if (args.SelectedCheckDate != null && args.CheckDateFilerCode != "None")
            {
                if (args.CheckDateFilerCode == "Equals")
                {
                    iQueryable_Translation0 = iQueryable_Translation0.Where(d => d.TranslateDate == args.SelectedCheckDate);
                    iQueryable_Translation1 = iQueryable_Translation1.Where(d => d.TranslateDate == args.SelectedCheckDate);
                }

                else if (args.CheckDateFilerCode == "Bigger")
                {
                    iQueryable_Translation0 = iQueryable_Translation0.Where(d => d.TranslateDate > args.SelectedCheckDate);
                    iQueryable_Translation1 = iQueryable_Translation1.Where(d => d.TranslateDate > args.SelectedCheckDate);
                }

                else if (args.CheckDateFilerCode == "Less")
                {
                    iQueryable_Translation0 = iQueryable_Translation0.Where(d => d.TranslateDate < args.SelectedCheckDate);
                    iQueryable_Translation1 = iQueryable_Translation1.Where(d => d.TranslateDate < args.SelectedCheckDate);
                }
            }

            if (!string.IsNullOrEmpty(args.SearchText))
            {
                iQueryable_Translation0 = iQueryable_Translation0.Where(d => !string.IsNullOrEmpty(d.TranslatedText) && d.TranslatedText.ToUpper().Contains(args.SearchText.ToUpper()));
                iQueryable_Translation1 = iQueryable_Translation1.Where(d => !string.IsNullOrEmpty(d.TranslatedText) && d.TranslatedText.ToUpper().Contains(args.SearchText.ToUpper()));
            }
            #endregion

            #region FieldsTranslations
            List<FieldsTranslations> myResult = new List<FieldsTranslations>();

            List<Translation> list_Translation0 = iQueryable_Translation0.ToList();
            List<Translation> list_Translation1 = iQueryable_Translation1.ToList();
            List<TextCode> list_TextCodes = iQueryable_TextCodes.OrderBy(d => d.Code).Skip(args.SkipDigit).Take(args.TakeDigit).ToList();

            foreach (TextCode item in list_TextCodes)
            {
                FieldsTranslations Record = new FieldsTranslations();
                Record.Tenant = item.Tenant;
                Record.DefaultText = item.DefaultText;
                Record.DefaultTextPlural = item.DefaultTextPlural;
                Record.Code = item.Code;
                Record.TextCodeId = item.Id;
                Record.TypeCode = item.TextCodeTypeCode;
                Record.ObjectTableID = item.ObjectTableId;
                Record.TranslationTenent = translationTenant;
                Record.TranslationLanguageCode = args.Language;
                Record.ObjectTableName = item.ObjectTable.Name;
                Record.ObjectTableTypeCode = item.ObjectTable.ObjectTableTypeCode;
                Record.TranslatedText = item.DefaultText;
                Record.TranslatedTextPlural = item.DefaultTextPlural;

                Translation translaion = list_Translation1.Where(t => t.TextCodeId == item.Id).FirstOrDefault();
                if (translaion == null)
                {
                    translaion = list_Translation0.Where(t => t.TextCode.Code == item.Code).FirstOrDefault();
                }

                if (translaion != null)
                {
                    Record.IsTranslated = true;
                    Record.TranslateDate = translaion.TranslateDate;
                    Record.TranslatedText = translaion.TranslatedText;
                    Record.TranslatedByUserId = translaion.TranslatedByUserId;
                    Record.TranslatedTextPlural = translaion.TranslatedTextPlural;
                }

                myResult.Add(Record);
            }
            #endregion

            args.CountAll = iQueryable_TextCodes.Count();
            args.Translations = myResult.OrderBy(d => d.Code).Skip(args.SkipDigit).Take(args.TakeDigit).ToList();
        }
    }

    public class TranslateLabelsAPIHelper
    {
        public int Id { get; set; }
        public int CountAll { get; set; }
        public string Language { get; set; }
        public string ObjectTableId { get; set; }
        public string TextCodeTypeCode { get; set; }

        public string SpellCheckedFilterCode { get; set; }
        public string CheckDateFilerCode { get; set; }
        public DateTime? SelectedCheckDate { get; set; }
        public string SearchText { get; set; }

        public int SkipDigit { get; set; }
        public int TakeDigit { get; set; }

        public List<FieldsTranslations> Translations { get; set; }
        public List<FieldsTranslations> UpdatedTranslations { get; set; }
    }
}