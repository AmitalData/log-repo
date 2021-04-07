using System;
using System.Linq;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateTermsofUseList(TermsofUseList currentEntity)
        {
        }

        public IQueryable<TermsofUsePM> GetTermsofUses(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            termsofUseQuery = new TermsofUseQuery(tenant);
            return termsofUseQuery.GetTermsofUsePMsByVersion();
        }

        public IQueryable<TermsofUseList> GetTermsofUseLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IQueryable<TermsofUse> iQueryable = termsofUseRepository.GetTermsofUses();
            var query2 = from entity in iQueryable
                         select new TermsofUseList()
                         {
                             VersionNumber = entity.VersionNumber,
                             Date = entity.Date,
                         };
            return query2;
        }

        public TermsofUsePM GetSingleTermsofUse(DateTime toudate, int version)
        {
            termsofUseQuery = new TermsofUseQuery(version);
            return termsofUseQuery.GetSinglePM(toudate, version);
        }

        public TermsofUseList GetSingleTermsofUseList(DateTime toudate, int version)
        {
            termsofUseQuery = new TermsofUseQuery(version);
            TermsofUsePM entityPm = termsofUseQuery.GetSinglePM(toudate, version);
            TermsofUseList entityList = new TermsofUseList()
            {
                VersionNumber = entityPm.VersionNumber,
                Date = entityPm.Date,
            };
            return entityList;
        }

        public TermsofUsePM GetTermsofUseByVersion(int version)
        {
            termsofUseQuery = new TermsofUseQuery(version);
            TermsofUsePM termsofUse = termsofUseQuery.GetSinglePMByVersion(version);
            return termsofUse;
        }

        public void InsertTermsofUse(TermsofUsePM termsofUse)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(termsofUse.VersionNumber);
            }
            termsofUseRepository = new TermsofUseRepository(objectContext);
            termsofUseQuery = new TermsofUseQuery(termsofUseRepository);

            bool exist = termsofUseQuery.GetSinglePMByVersion(termsofUse.VersionNumber) != null ? true : false;
            if (!exist)
            {
                TermsofUse newTermsofUse = new TermsofUse();
                newTermsofUse.VersionNumber = termsofUse.VersionNumber;
                MapTermsofUseTermsofUsePM(termsofUse, newTermsofUse);
                termsofUseRepository.Add(newTermsofUse);
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", termsofUse.VersionNumber);
                msg = msg.Replace("%Entity", "TermsofUse");
                throw new Exception(msg);
            }
        }

        public void MapTermsofUseTermsofUsePM(TermsofUsePM termsofUsePm, TermsofUse termsofUse)
        {
            termsofUse.Date = termsofUsePm.Date;
        }

        public void UpdateTermsofUse(TermsofUsePM currentTermsofUse)
        {
           
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentTermsofUse.VersionNumber);
            }
            termsofUseRepository = new TermsofUseRepository(objectContext);

            bool exist = (from a in termsofUseRepository.GetTermsofUsesByVersion(currentTermsofUse.VersionNumber)
                          where
                          a.Date == currentTermsofUse.Date
                          && a.VersionNumber == currentTermsofUse.VersionNumber
                          select a).Any();

            if (!exist)
            {
                TermsofUse entity = termsofUseRepository.GetSingleTermsofUse(currentTermsofUse.Date, currentTermsofUse.VersionNumber);
                MapTermsofUseTermsofUsePM(currentTermsofUse, entity);
                termsofUseRepository.Update(entity);
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentTermsofUse.VersionNumber);
                msg = msg.Replace("%Entity", "TermsofUse");
                throw new Exception(msg);
            }
        }

        public void DeleteTermsofUse(TermsofUsePM termsofUse)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(termsofUse.VersionNumber);
            }
            termsofUseRepository = new TermsofUseRepository(objectContext);
            TermsofUse entity = termsofUseRepository.GetSingleTermsofUse(termsofUse.Date, termsofUse.VersionNumber);
            termsofUseRepository.Remove(entity);
        }
    }
}