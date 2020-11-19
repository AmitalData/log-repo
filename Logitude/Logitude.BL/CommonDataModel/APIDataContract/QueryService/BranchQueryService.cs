using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class BranchQueryService
    {
        public BranchPM BranchCustomDataMappingAndValidatin(Branch MyEntity, int Tenant, string ComputingPartnerName = "")
        {

            try
            {
                var temp = new BranchPM();

                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                else
                {
                    temp = query.GetSinglePMByCode(MyEntity.Code, Tenant, false);
                }


                if (temp == null)
                {
                    throw new ApplicationException("Branch with Id " + MyEntity.Id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                if (string.IsNullOrEmpty(temp.EnglishName))
                {
                    temp.EnglishName = MyEntity.EnglishName;
                }
                if (string.IsNullOrEmpty(temp.LocalName))
                {
                    temp.LocalName = MyEntity.LocalName;
                }
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public Branch BranchCustomDataMapping(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                BranchQueryService BranchService0 = new BranchQueryService(Tenant);
                var ChargeType = BranchService0.GetBranchById(Id, Tenant,ComputingPartnerName);
                return ChargeType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    }
