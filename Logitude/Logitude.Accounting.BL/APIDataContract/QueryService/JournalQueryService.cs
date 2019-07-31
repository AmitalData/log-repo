using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
   public partial class JournalQueryService
    {


        public Journal JournalDataMappingAndValidatin(JournalPM MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new Journal();
                //if (!string.IsNullOrEmpty(MyEntity.Id))
                //{
                //    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                //}

                //if (temp == null)
                //{
                //    throw new ApplicationException("GLAccount with Id " + MyEntity.Id + " doesn't exist");
                //}
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                temp.Tenant = MyEntity.Tenant;

                
               temp.JournalNumber = MyEntity.JournalNumber;
                temp.CreateDate = MyEntity.CreateDate;
                temp.AccountingDate = MyEntity.AccountingDate;
                temp.ExternalNo = MyEntity.ExternalNo;
                temp.UpdateDate = MyEntity.UpdateDate;
                temp.ApproveDate = MyEntity.ApproveDate;
                temp.AccountingEntityReference = MyEntity.AccountingEntityReference;
             
                UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
                if (MyEntity.UpdatedByUserId != null)
                {
                    var myUpdatedByUserPM = UpdatedByUserUserService.GetUserById(MyEntity.UpdatedByUserId, Tenant);
                    if (myUpdatedByUserPM != null)
                    {
                        temp.UpdatedByUser = new User();
                        temp.UpdatedByUser.EnglishName = myUpdatedByUserPM.EnglishName;
                        temp.UpdatedByUser.LocalName = myUpdatedByUserPM.LocalName;
                        temp.UpdatedByUser.PartnerCode = myUpdatedByUserPM.PartnerCode;
                        temp.UpdatedByUser.ExternalCode = myUpdatedByUserPM.ExternalCode;


                    }

                }


                temp.ExternalSystem = MyEntity.ExternalSystem;
            
              


                Logitude.Accounting.BL.EntityQueryServices.JournalQueryService OriginalJournalJournalService = new Logitude.Accounting.BL.EntityQueryServices.JournalQueryService(Tenant);
                if (MyEntity.OriginalJournalId != null)
                {
                    var myOriginalJournalPM = OriginalJournalJournalService.GetSinglePM(MyEntity.OriginalJournalId, Tenant);
                    if (myOriginalJournalPM != null)
                    {
                        temp.OriginalJournalNumber = myOriginalJournalPM.JournalNumber;
                     
                    }

                }


                UserQueryService ApprovedByUserUserService = new UserQueryService(Tenant);
                if (MyEntity.ApprovedByUserId != null)
                {
                    var myApprovedByUserPM = ApprovedByUserUserService.GetUserById(MyEntity.ApprovedByUserId, Tenant);
                    if (myApprovedByUserPM != null)
                    {
                        temp.ApprovedByUser = new User();
                        temp.ApprovedByUser.EnglishName = myApprovedByUserPM.EnglishName;
                        temp.ApprovedByUser.LocalName = myApprovedByUserPM.LocalName;
                        temp.ApprovedByUser.PartnerCode = myApprovedByUserPM.PartnerCode;
                        temp.ApprovedByUser.ExternalCode = myApprovedByUserPM.ExternalCode;
                    }

                }





                if (MyEntity.CreatedByUserId != null)
                {
                    var myCreatedByUserPM = ApprovedByUserUserService.GetUserById(MyEntity.CreatedByUserId, Tenant);
                    if (myCreatedByUserPM != null)
                    {
                        temp.CreatedByUser = new User();
                        temp.CreatedByUser.EnglishName = myCreatedByUserPM.EnglishName;
                        temp.CreatedByUser.LocalName = myCreatedByUserPM.LocalName;
                        temp.CreatedByUser.PartnerCode = myCreatedByUserPM.PartnerCode;
                        temp.CreatedByUser.ExternalCode = myCreatedByUserPM.ExternalCode;
                    }

                }


                EntityQueryServices.JournalTypeQueryService TypeCodeJournalTypeService = new EntityQueryServices.JournalTypeQueryService(Tenant);
                if (MyEntity.TypeCode != null)
                {
                    var myTypeCodePM = TypeCodeJournalTypeService.GetSinglePM(MyEntity.TypeCode, Tenant);
                    if (myTypeCodePM != null)
                    {
                        temp.JournalType = myTypeCodePM.JournalTypeID;
                        //temp.JournalType.Code = myTypeCodePM.JournalTypeID;
                        //temp.JournalType.EnglishName = myTypeCodePM.EnglishName;
                        //temp.JournalType.LocalName = myTypeCodePM.LocalName;
                      
                    }

                }


                EntityQueryServices.JournalStatusTypeQueryService StatusCodeJournalStatusTypeService = new EntityQueryServices.JournalStatusTypeQueryService(Tenant);
                if (MyEntity.StatusCode != null)
                {
                    var myStatusCodePM = StatusCodeJournalStatusTypeService.GetSinglePM(MyEntity.StatusCode, Tenant);
                    if (myStatusCodePM != null)
                    {
                        temp.JournalStatusType = myStatusCodePM.JournalStatusID;
                    //    temp.JournalStatusType.Code = myStatusCodePM.JournalStatusID;
                    //    temp.JournalStatusType.EnglishName = myStatusCodePM.EnglishName;
                    //    temp.JournalStatusType.LocalName = myStatusCodePM.LocalName;
                    }

                }


                EntityQueryServices.AccountingEntityQueryService AccountingEntityCodeAccountingEntityService = new EntityQueryServices.AccountingEntityQueryService(Tenant);
                if (MyEntity.AccountingEntityCode != null)
                {
                    var myAccountingEntityCodePM = AccountingEntityCodeAccountingEntityService.GetSinglePM(MyEntity.AccountingEntityCode, Tenant);
                    if (myAccountingEntityCodePM != null)
                    {
                        temp.AccountingEntity = new AccountingEntity();
                        temp.AccountingEntity.Code = myAccountingEntityCodePM.Code;
                        temp.AccountingEntity.EnglishName = myAccountingEntityCodePM.EnglishName;
                        temp.AccountingEntity.LocalName = myAccountingEntityCodePM.LocalName;


                    }

                }

                if (MyEntity.JournalLines != null && MyEntity.JournalLines.Count > 0)
                {
                    JournalLineQueryService JournalLineService9 = new JournalLineQueryService(Tenant);
                    temp.JournalLines = JournalLineService9.JournalLineCustomDataMapping(MyEntity, MyEntity.JournalLines, Tenant);
                }


                temp.AccountingEntityId = MyEntity.JournalNumber;
             


                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public string SetVoidedByJournal(Journal MyEntity, int Tenant)
        //{
        //    string voidedBy = null;
        //    Logitude.Accounting.BL.EntityQueryServices.JournalQueryService JournalService = new Logitude.Accounting.BL.EntityQueryServices.JournalQueryService(Tenant);
        //    if (MyEntity.VoidedByJournal != null)
        //    {
        //        var myVoidedByJournalIdPM =JournalService.GetSingleJournalByNumber(MyEntity.VoidedByJournal, Tenant);
        //        if (myVoidedByJournalIdPM != null)
        //        {

        //            voidedBy = myVoidedByJournalIdPM.Id;
        //        }

        //    }

        //    return voidedBy;

        //}

     

        public string SetOriginalJournal(Journal MyEntity, int Tenant)
        {
            string original = null;
            Logitude.Accounting.BL.EntityQueryServices.JournalQueryService JournalService = new Logitude.Accounting.BL.EntityQueryServices.JournalQueryService(Tenant);
            if (MyEntity.OriginalJournalNumber != null)
            {
                var myVoidedByJournalIdPM = JournalService.GetSingleJournalByNumber(MyEntity.OriginalJournalNumber, Tenant);
                if (myVoidedByJournalIdPM != null)
                {

                    original = myVoidedByJournalIdPM.Id;
                }

            }

            return original;

        }

        public Journal GetJournalByNumber(string number, int Tenant)
        {
            try
            {


                var temp = query.GetSingleJournalByNumber(number, Tenant);
                if (temp == null)
                    throw new ApplicationException("Journal with number " + number + " doesn't exist");

                return JournalDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Journal GetSingleJournalByExternalNoAndExternalSystem(string externalNo, string externalSysem, int Tenant)
        {
            try
            {


                var temp = query.GetSingleJournalByExternalNoAndExternalSystem(externalNo, externalSysem, Tenant);
                if (temp == null)
                    throw new ApplicationException("Journal with external NO. " + externalNo + " and external system "+ externalSysem+" doesn't exist");

                return JournalDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public JournalPM GetJournalPMById(string id, int Tenant)
        {
           

                var temp = query.GetSinglePM(id, Tenant);
            return temp;
        }
    }
}
