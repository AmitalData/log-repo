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


        public List<TextCodePM> GetFilteredTextCodesForDefaultTranslation(int tenant, string objectTableId, string isSpellCheckedCode, string textCodeTypeCode, DateTime? selectedCheckDate, string checkDateFiler, string searchText, int skipDigit, int takeDigit)
        {
            List<TextCodePM> data = (from a in repository.context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser.Contact")
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
                                     }).ToList();

            List<TextCodePM> filteredListTable = string.IsNullOrEmpty(objectTableId) ? data : data.Where(d => d.ObjectTableId == objectTableId).ToList();
            List<TextCodePM> filteredListSearch = string.IsNullOrEmpty(searchText) ? filteredListTable : filteredListTable.Where(d => d.Code.ToUpper().Contains(searchText.ToUpper()) || (!string.IsNullOrEmpty(d.DefaultText) && d.DefaultText.ToUpper().Contains(searchText.ToUpper())) || (!string.IsNullOrEmpty(d.LocalDefaultText) && d.LocalDefaultText.ToUpper().Contains(searchText.ToUpper()))).ToList();
            List<TextCodePM> filteredListType = (textCodeTypeCode == "All") ? filteredListSearch : filteredListSearch.Where(d => d.TextCodeTypeCode == textCodeTypeCode).ToList();
            List<TextCodePM> filteredListDate = new List<TextCodePM>();
            List<TextCodePM> filteredListChecked = new List<TextCodePM>();

            if (selectedCheckDate == null || checkDateFiler == "None")
            {
                filteredListDate = filteredListType;
            }

            if (selectedCheckDate != null)
            {
                if (checkDateFiler == "Equals") { filteredListDate = filteredListType.Where(d => d.SpellCheckDate == selectedCheckDate).ToList(); }
                else if (checkDateFiler == "Bigger") { filteredListDate = filteredListType.Where(d => d.SpellCheckDate > selectedCheckDate).ToList(); }
                else if (checkDateFiler == "Less") { filteredListDate = filteredListType.Where(d => d.SpellCheckDate < selectedCheckDate).ToList(); }
            }

            if (isSpellCheckedCode == "None")
            {
                filteredListChecked = filteredListDate;
            }

            else if (isSpellCheckedCode != "None")
            {
                if (isSpellCheckedCode == "True")
                {
                    filteredListChecked = filteredListDate.Where(d => d.IsSpellChecked == true).ToList();
                }

                else
                {
                    filteredListChecked = filteredListDate.Where(d => d.IsSpellChecked == false).ToList();
                }
            }

            return filteredListChecked.OrderBy(d => d.Code).Skip(skipDigit).Take(takeDigit).ToList();
        }



    }
}