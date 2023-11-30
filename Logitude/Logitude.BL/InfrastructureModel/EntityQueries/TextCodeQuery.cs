using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class TextCodeQuery
    {
        TextCodeRepository repository;
        public TextCodeQuery()
        {
            repository = new TextCodeRepository(); 
        }

        public TextCodeQuery(int tenant)
        {
            repository = new TextCodeRepository(tenant);
        }

        public TextCodeQuery(TextCodeRepository textCodeRepository)
        {
            repository = textCodeRepository;
        }



        public TextCodePM GetByCode(string code , int tenant)
        {
            return (from a in repository.context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser.Contact")
                   where a.Code == code && a.Tenant == tenant
                    select new TextCodePM()
                   {
                       Code = a.Code,
                       DefaultText = a.DefaultText,
                       DefaultTextPlural = a.DefaultTextPlural,
                       Id = a.Id,
                       ObjectTableId = a.ObjectTableId,
                       ObjectTableName = a.ObjectTable.Name,
                       Tenant = a.Tenant,
                       TextCodeTypeCode = a.TextCodeTypeCode,
                       IsSpellChecked = a.IsSpellChecked,
                       SpellCheckDate = a.SpellCheckDate,
                       SpellCheckedByUserId = a.SpellCheckedByUserId,
                       InActive = a.InActive,
                       LocalDefaultText = a.LocalDefaultText,
                       SpellCheckedByUserName = a.SpellCheckedByUser == null ? null : a.SpellCheckedByUser.Contact.EnglishName,
                   }).FirstOrDefault();
        }
        public IQueryable<TextCodePM> GetTextCodePMsByTenant(int tenant)
        {
            IQueryable<TextCodePM> textcodes = from a in repository.context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser.Contact")
                                               where a.Tenant == tenant && !a.InActive
                                               select new TextCodePM()
                                               {
                                                   Code = a.Code,
                                                   DefaultText = a.DefaultText,
                                                   DefaultTextPlural = a.DefaultTextPlural,
                                                   Id = a.Id,
                                                   ObjectTableId = a.ObjectTableId,
                                                   ObjectTableName = a.ObjectTable.Name,
                                                   Tenant = a.Tenant,
                                                   TextCodeTypeCode = a.TextCodeTypeCode,
                                                   IsSpellChecked = a.IsSpellChecked,
                                                   SpellCheckDate = a.SpellCheckDate,
                                                   SpellCheckedByUserId = a.SpellCheckedByUserId,
                                                   InActive = a.InActive,
                                                    LocalDefaultText=a.LocalDefaultText,
                                                   SpellCheckedByUserName = a.SpellCheckedByUser == null ? null : a.SpellCheckedByUser.Contact.EnglishName,
                                               };

            return textcodes;
        }

        public IQueryable<TextCodePM> GetTenantZeroTextCodePMs()
        {
            IQueryable<TextCodePM> textcodes = from a in repository.context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser.Contact")
                                               where a.Tenant == 0 && !a.InActive
                                               select new TextCodePM()
                                               {
                                                   Code = a.Code,
                                                   DefaultText = a.DefaultText,
                                                   DefaultTextPlural = a.DefaultTextPlural,
                                                   Id = a.Id,
                                                   ObjectTableId = a.ObjectTableId,
                                                   ObjectTableName = a.ObjectTable.Name,
                                                   Tenant = a.Tenant,
                                                   TextCodeTypeCode = a.TextCodeTypeCode,
                                                   IsSpellChecked = a.IsSpellChecked,
                                                   SpellCheckDate = a.SpellCheckDate,
                                                   SpellCheckedByUserId = a.SpellCheckedByUserId,
                                                   InActive = a.InActive,
                                                   LocalDefaultText = a.LocalDefaultText,
                                                   SpellCheckedByUserName = a.SpellCheckedByUser == null ? null : a.SpellCheckedByUser.Contact.EnglishName,
                                               };

            return textcodes;
        }


        public List<TextCodePM> GetFilteredTextCodesForDefaultTranslation(int tenant, string objectTableId, string isSpellCheckedCode, string textCodeTypeCode, DateTime? selectedCheckDate, string checkDateFiler, string searchText, int skipDigit, int takeDigit)
        {
            IQueryable<TextCodePM> textCodes = (from a in repository.context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser.Contact")
                                                where a.Tenant == tenant
                                                && !a.InActive
                                                select new TextCodePM()
                                                {
                                                    Code = a.Code,
                                                    DefaultText = a.DefaultText,
                                                    DefaultTextPlural = a.DefaultTextPlural,
                                                    Id = a.Id,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Tenant = a.Tenant,
                                                    TextCodeTypeCode = a.TextCodeTypeCode,
                                                    IsSpellChecked = a.IsSpellChecked,
                                                    SpellCheckDate = a.SpellCheckDate,
                                                    SpellCheckedByUserId = a.SpellCheckedByUserId,
                                                    InActive = a.InActive,
                                                    LocalDefaultText = a.LocalDefaultText,
                                                    SpellCheckedByUserName = a.SpellCheckedByUser == null ? null : a.SpellCheckedByUser.Contact.EnglishName,
                                                });

            if (!string.IsNullOrEmpty(objectTableId))
            {
                textCodes = textCodes.Where(d => d.ObjectTableId == objectTableId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                textCodes = textCodes.Where(d => d.Code.ToUpper().Contains(searchText.ToUpper()) || (!string.IsNullOrEmpty(d.DefaultText) && d.DefaultText.ToUpper().Contains(searchText.ToUpper())) || (!string.IsNullOrEmpty(d.LocalDefaultText) && d.LocalDefaultText.ToUpper().Contains(searchText.ToUpper())));
            }
            if (textCodeTypeCode != "All")
            {
                textCodes = textCodes.Where(d => d.TextCodeTypeCode == textCodeTypeCode);
            }

            if (selectedCheckDate != null)
            {
                if (checkDateFiler == "Equals") { textCodes = textCodes.Where(d => d.SpellCheckDate == selectedCheckDate); }
                else if (checkDateFiler == "Bigger") { textCodes = textCodes.Where(d => d.SpellCheckDate > selectedCheckDate); }
                else if (checkDateFiler == "Less") { textCodes = textCodes.Where(d => d.SpellCheckDate < selectedCheckDate); }
            }

            if (isSpellCheckedCode != "None")
            {
                if (isSpellCheckedCode == "True")
                {
                    textCodes = textCodes.Where(d => d.IsSpellChecked == true);
                }

                else
                {
                    textCodes = textCodes.Where(d => d.IsSpellChecked == false);
                }
            }

            return textCodes.OrderBy(d => d.Code).Skip(skipDigit).Take(takeDigit).ToList();

        }



    }
}