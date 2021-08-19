using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class SATInterfaceSettingQuery
    {
        SATInterfaceSettingRepository repository;
        public SATInterfaceSettingQuery()
        {
            repository = new SATInterfaceSettingRepository();
        }


        public SATInterfaceSettingQuery(SATInterfaceSettingRepository SATInterfaceSettingRepository)
        {
            repository = SATInterfaceSettingRepository;
        }

        public SATInterfaceSettingQuery(int tenant)
        {
            repository = new SATInterfaceSettingRepository(tenant);
        }

        public SATInterfaceSettingPM GetSinglePM(int tenantId, int tenant)
        {
            return (from a in repository.context.SATInterfaceSettings.Include("SATInterface")
                    where a.Tenant == tenantId
                    select new SATInterfaceSettingPM()
                    {

                        Tenant = a.Tenant,
                        SATInterfaceCode = a.SATInterfaceCode,
                        Token = a.Token,
                        SATInterfaceName = a.SATInterface.Name,
                        ActivationDate = a.ActivationDate,
                        MetodoPagoCode = a.MetodoPagoCode,
                        IsARInvoiceTransferEnabled = a.IsARInvoiceTransferEnabled,
                        IsCartaPorteTransferEnabled = a.IsCartaPorteTransferEnabled,
                    }).FirstOrDefault();
        }


        public SATInterfaceSettingPM GetSATInterfaceSettingPMByTenant(int tenant)
        {
            return (from a in repository.context.SATInterfaceSettings.Include("SATInterface")
                    where a.Tenant == tenant
                    select new SATInterfaceSettingPM()
                    {
                        
                        Tenant = a.Tenant,
                        SATInterfaceCode = a.SATInterfaceCode,
                        Token = a.Token,
                        SATInterfaceName = a.SATInterface.Name,
                        ActivationDate = a.ActivationDate,
                        MetodoPagoCode = a.MetodoPagoCode,
                        IsARInvoiceTransferEnabled = a.IsARInvoiceTransferEnabled,
                        IsCartaPorteTransferEnabled = a.IsCartaPorteTransferEnabled,
                    }).FirstOrDefault();
        }

        public IQueryable<SATInterfaceSettingPM> GetSATInterfaceSettingPMs()
        {
            return from a in repository.context.SATInterfaceSettings.Include("SATInterface")
                   select new SATInterfaceSettingPM()
                   {
                       Tenant = a.Tenant,
                       SATInterfaceCode = a.SATInterfaceCode,
                       Token = a.Token,
                       SATInterfaceName = a.SATInterface.Name,
                       ActivationDate = a.ActivationDate,
                       MetodoPagoCode = a.MetodoPagoCode,
                       IsARInvoiceTransferEnabled = a.IsARInvoiceTransferEnabled,
                       IsCartaPorteTransferEnabled = a.IsCartaPorteTransferEnabled,
                   };
        }

        public IQueryable<SATInterfaceSettingList> GetIQueryableEntityList(IQueryable<SATInterfaceSetting> iQueryable)
        {
            IQueryable<SATInterfaceSettingList> result = from entity in iQueryable
                                                         select new SATInterfaceSettingList()
                                                         {
                                                           
                                                             Tenant = entity.Tenant,
                                                             SATInterfaceCode = entity.SATInterfaceCode,
                                                             Token = entity.Token,
                                                             ActivationDate = entity.ActivationDate,
                                                             MetodoPagoCode = entity.MetodoPagoCode,
                                                             IsARInvoiceTransferEnabled = entity.IsARInvoiceTransferEnabled,
                                                             IsCartaPorteTransferEnabled = entity.IsCartaPorteTransferEnabled,
                                                         };
            return result;
        }


    }
}
