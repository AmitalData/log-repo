	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class FullAccountingSettingListQueryService
    {
        private IQueryable<FullAccountingSettingList> GetIqueryableList(IQueryable<FullAccountingSetting> iQueryable)
        {
            IQueryable<FullAccountingSettingList> query = (from a in iQueryable
                                                           select new FullAccountingSettingList()
                                                           {
                                                               Id = a.Id,
                                                               NumberOfAgingMonths = a.NumberOfAgingMonths,
                                                               AutomaticReconcileMethodId = a.AutomaticReconcileMethodId,
                                                               ConsolidationVAT = a.ConsolidationVAT,
                                                               DeductionFileNumber = a.DeductionFileNumber,
                                                               DefaultVATTypeId = a.DefaultVATTypeId,
                                                               ExchangeRateDiffGLAccountId = a.ExchangeRateDiffGLAccountId,
                                                               VATInputsGLAccountId = a.VATInputsGLAccountId,
                                                               Tenant = a.Tenant,

                                                               CustomerControlAccountId = a.CustomerControlAccountId,
                                                               CustomerControlAccountNumber = a.CustomerControlAccount != null ? a.CustomerControlAccount.DisplayNumber : null,
                                                               CustomerControlAccountName = a.CustomerControlAccount != null ? a.CustomerControlAccount.EnglishName : null,

                                                               VendorControlAccountId = a.VendorControlAccountId,
                                                               VendorControlAccountNumber = a.VendorControlAccount != null ? a.VendorControlAccount.DisplayNumber : null,
                                                               VendorControlAccountName = a.VendorControlAccount != null ? a.VendorControlAccount.EnglishName : null,

                                                               FileControlAccountId = a.FileControlAccountId,
                                                               FileControlAccountNumber = a.FileControlAccount != null ? a.FileControlAccount.DisplayNumber : null,
                                                               FileControlAccountName = a.FileControlAccount != null ? a.FileControlAccount.EnglishName : null,

                                                               OceanExportJobControlAccountId = a.OceanExportJobControlAccountId,
                                                               OceanExportJobControlAccountNumber = a.OceanExportJobControlAccount != null ? a.OceanExportJobControlAccount.DisplayNumber : null,
                                                               OceanExportJobControlAccountName = a.OceanExportJobControlAccount != null ? a.OceanExportJobControlAccount.EnglishName : null,

                                                               AirExportJobControlAccountId = a.AirExportJobControlAccountId,
                                                               AirExportJobControlAccountNumber = a.AirExportJobControlAccount != null ? a.AirExportJobControlAccount.DisplayNumber : null,
                                                               AirExportJobControlAccountName = a.AirExportJobControlAccount != null ? a.AirExportJobControlAccount.EnglishName : null,

                                                               OceanImportJobControlAccountId = a.OceanImportJobControlAccountId,
                                                               OceanImportJobControlAccountNumber = a.OceanImportJobControlAccount != null ? a.OceanImportJobControlAccount.DisplayNumber : null,
                                                               OceanImportJobControlAccountName = a.OceanImportJobControlAccount != null ? a.OceanImportJobControlAccount.EnglishName : null,

                                                               AirImportJobControlAccountId = a.AirImportJobControlAccountId,
                                                               AirImportJobControlAccountNumber = a.AirImportJobControlAccount != null ? a.AirImportJobControlAccount.DisplayNumber : null,
                                                               AirImportJobControlAccountName = a.AirImportJobControlAccount != null ? a.AirImportJobControlAccount.EnglishName : null,

                                                               ExternalReconciliationDefault = a.ExternalReconciliationDefault,
                                                               DefaultDifferencesGLAccountId = a.DefaultDifferencesGLAccountId,
                                                               DefaultExternalDiffGLAccountId= a.DefaultExternalDiffGLAccountId,
                                                               DefaultTaxWithholdPercentage = a.DefaultTaxWithholdPercentage,
                                                               GLAccounterCounterLength = a.GLAccounterCounterLength,
                                                               IsSecurityLevelActivated = a.IsSecurityLevelActivated
                                                           });
            return query;
        }

        private IQueryable<FullAccountingSetting> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<FullAccountingSetting> iQueryable, int tenant)
        {
            return iQueryable;
            //throw new NotImplementedException();
        }
        private IQueryable<FullAccountingSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<FullAccountingSetting> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	