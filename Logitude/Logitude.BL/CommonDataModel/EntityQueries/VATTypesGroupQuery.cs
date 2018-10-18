using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class VATTypesGroupQuery
    {
        VATTypesGroupRepository repository;

        public VATTypesGroupQuery(int tenant)
        {
            repository = new VATTypesGroupRepository(tenant);
        }

        public VATTypesGroupQuery(VATTypesGroupRepository agentRepository)
        {
            repository = agentRepository;
        }

        public List<VATTypesGroupPM> GetVATTypesGroups(int tenant)
        {
            List<VATTypesGroupPM> myResult = (from d in repository.GetVATTypesGroup(tenant)
                                              select new VATTypesGroupPM()
                                              {
                                                  Tenant = d.Tenant,
                                                  GroupVATTypeId = d.GroupVATTypeId,
                                                  SingleVATTypeId = d.SingleVATTypeId,
                                                  SingleVATTypeName = d.SingleVATType == null ? null:d.SingleVATType.EnglishName,
                                              }).ToList();

            return myResult;
        }

        public List<VATTypesGroupPM> GetVATTypesGroupsByVatId(int tenant, string vatTypeId)
        {
            List<VATTypesGroupPM> myResult = (from d in repository.GetVATTypesGroup(vatTypeId, tenant)
                                              select new VATTypesGroupPM()
                                              {
                                                  Tenant = d.Tenant,
                                                  GroupVATTypeId = d.GroupVATTypeId,
                                                  SingleVATTypeId = d.SingleVATTypeId,
                                                  SingleVATTypeName = d.SingleVATType == null ? null : d.SingleVATType.EnglishName,
                                              }).ToList();

            return myResult;
        }
    }
}
