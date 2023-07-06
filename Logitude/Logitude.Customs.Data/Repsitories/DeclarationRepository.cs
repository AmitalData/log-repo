
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using Simplog.Data.CommonDataModel;
using System.Data.SqlClient;
using System.Data;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class DeclarationRepository : IRepository<Declaration>
    {
        partial void onUpdate()
        {

        }



        public List<Declaration> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public Declaration GetSingleDeclarationByNumber(string number, int tenant)
        {

            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where a.DeclarationNumber == number && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public Declaration GetDeclarationByConsignment(string cargoTypeCode, string manifestNumber, string secondCargoID, string thirdCargoID)
        {
            var query = (
                   from d in context.Declarations
                   join a in context.Consignments
                    on d.Id equals a.DeclarationId
                   where a.CargoTypeCode.ToLower() == cargoTypeCode.ToLower() & a.ManifestNumber.ToLower() == manifestNumber.ToLower()
                & a.SecondCargoID.ToLower() == secondCargoID.ToLower() & a.ThirdCargoID.ToLower() == thirdCargoID.ToLower()
                   select d
                       ).ToList().FirstOrDefault();
            return query;
        }

        public string GetLastAmendmentIdByCustomFileNo(string customFileNo, int tenant)
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            var res = (from a in context.Declarations
                       where a.CustomFileNo == customFileNo
                       && a.AmendmentDontDisplayInList == false
                       && a.Tenant == tenant
                       select a.Id);

            return res.FirstOrDefault();
        }

        public Declaration GetDeclarationNotAmendmentDontDisplayInList(string id, string amendmentOriginalDeclartation, int tenant)
        {

            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where ((a.Id == id && a.AmendmentDontDisplayInList == false) || (a.Id == amendmentOriginalDeclartation && a.AmendmentDontDisplayInList == false))
                    && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public Declaration GetAcceptDeclarationAmendment(string id, int tenant)
        {


            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 


            bool newBL = true;
            if (newBL)// TRING  FILENO=60255210
            {
                List<Declaration> allDecSameFile = null;
                bool ship2uSlow_KIS = true;
                if (ship2uSlow_KIS)
                {
                    var qCustomFileNo = context.Declarations
        .Where(r => r.Id == id)
        .Where(r => r.Tenant == tenant)
        .Select(r => r.CustomFileNo);
                    string customFileNo = qCustomFileNo.FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(customFileNo))
                    {
                        throw new Exception($"CustomFileNo is missing (Declaration  id ={id})");
                    }
                    var qAllCustomFileNo = context.Declarations.Where(r => r.Tenant == tenant && r.CustomFileNo == customFileNo);
                    allDecSameFile = qAllCustomFileNo.ToList();
                }
                else
                {
                    var qCustomFileNo = context.Declarations
        .Where(r => r.Id == id)
        .Where(r => r.Tenant == tenant)
        .Select(r => r.CustomFileNo);
                    var qAllCustomFileNo =
                        (
                    from c in qCustomFileNo
                    join d in context.Declarations
                    on c equals d.CustomFileNo
                    select d
                        );

                    //var qGetAcceptDeclarationAmendment =
                    //    (
                    //from dec in qAllCustomFileNo.Where(r => r.Tenant == tenant)
                    //where
                    //(
                    //(dec.Id == id && dec.AmendmentDontDisplayInList == false && dec.DeclarationNumber != null) ||
                    //(dec.AmendmentOriginalDeclartation == id && dec.DeclarationNumber != null && dec.AmendmentDontDisplayInList == false)
                    //)
                    //select dec
                    //);

                    allDecSameFile = qAllCustomFileNo.ToList();
                }
                var qGetAcceptDeclarationAmendment = (from a in allDecSameFile
                                                      where ((a.Id == id && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null) ||
                            (a.AmendmentOriginalDeclartation == id && a.DeclarationNumber != null && a.AmendmentDontDisplayInList == false))
                            && a.Tenant == tenant
                                                      select a
                                                );

                return qGetAcceptDeclarationAmendment.FirstOrDefault();
            }
            else
            {
                return (from a in context.Declarations
                        where ((a.Id == id && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null) ||
                        (a.AmendmentOriginalDeclartation == id && a.DeclarationNumber != null && a.AmendmentDontDisplayInList == false))
                        && a.Tenant == tenant
                        select a).FirstOrDefault();
            }




        }
        public string GetAcceptDeclarationIdAmendment(string id, int tenant)
        {


            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 


            bool newBL = true;
            if (newBL)// TRING  FILENO=60255210
            {
                List<Declaration> allDecSameFile = null;
                bool ship2uSlow_KIS = true;
                if (ship2uSlow_KIS)
                {
                    var qCustomFileNo = context.Declarations
        .Where(r => r.Id == id)
        .Where(r => r.Tenant == tenant)
        .Select(r => r.CustomFileNo);
                    string customFileNo = qCustomFileNo.FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(customFileNo))
                    {
                        throw new Exception($"CustomFileNo is missing (Declaration  id ={id})");
                    }
                    var qAllCustomFileNo = context.Declarations.Where(r => r.Tenant == tenant && r.CustomFileNo == customFileNo);
                    allDecSameFile = qAllCustomFileNo.ToList();
                }
                else
                {
                    var qCustomFileNo = context.Declarations
        .Where(r => r.Id == id)
        .Where(r => r.Tenant == tenant)
        .Select(r => r.CustomFileNo);
                    var qAllCustomFileNo =
                        (
                    from c in qCustomFileNo
                    join d in context.Declarations
                    on c equals d.CustomFileNo
                    select d
                        );

                    //var qGetAcceptDeclarationAmendment =
                    //    (
                    //from dec in qAllCustomFileNo.Where(r => r.Tenant == tenant)
                    //where
                    //(
                    //(dec.Id == id && dec.AmendmentDontDisplayInList == false && dec.DeclarationNumber != null) ||
                    //(dec.AmendmentOriginalDeclartation == id && dec.DeclarationNumber != null && dec.AmendmentDontDisplayInList == false)
                    //)
                    //select dec
                    //);

                    allDecSameFile = qAllCustomFileNo.ToList();
                }
                var qGetAcceptDeclarationAmendment = (from a in allDecSameFile
                                                      where ((a.Id == id && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null) ||
                            (a.AmendmentOriginalDeclartation == id && a.DeclarationNumber != null && a.AmendmentDontDisplayInList == false))
                            && a.Tenant == tenant
                                                      select a.Id
                                                );

                return qGetAcceptDeclarationAmendment.FirstOrDefault();
            }
            else
            {
                return (from a in context.Declarations
                        where ((a.Id == id && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null) ||
                        (a.AmendmentOriginalDeclartation == id && a.DeclarationNumber != null && a.AmendmentDontDisplayInList == false))
                        && a.Tenant == tenant
                        select a.Id).FirstOrDefault();
            }




        }


        public string GetCourierhawbFromId(string declarationId, int tenant)
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
            return (from a in context.Declarations
                    where a.Id == declarationId
                    where a.Tenant == tenant
                    select a.CourierHAWB).FirstOrDefault();

        }

        public Declaration GetAcceptDeclarationAmendmentByCustomsFile(string customFileNo, int tenant)
        {

            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where (a.CustomFileNo == customFileNo && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null)
                    && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public Declaration GetWaitingDeclarationAmendmentByCustomsFile(string customFileNo, int tenant)
        {

            return (from a in context.Declarations
                    where (a.CustomFileNo == customFileNo && a.IsAmendment == true && new string[] { "6", "7", "8", "10" }.Contains(a.AmendmentStatus))
                    && a.Tenant == tenant && a.IsCancelled == false
                    select a).FirstOrDefault();
        }


        public int GetDeclarationMaxCancelRequestNumber(int tenant)
        {
            // && a.Id==id

            var list = (from a in context.Declarations
                        where a.Tenant == tenant && a.CancelRequestNumber != null
                        select a.CancelRequestNumber).ToList();

            int? max = 0;

            if (list.Count() != 0)
                max = list.Max();

            return Convert.ToInt32(max);
        }

        public int GetDeclarationMaxAmendmentAndCancelRequestNumber(int tenant)
        {
            //var x=  (from a in context.Declarations
            //         where a.Tenant == tenant
            //         select Convert.ToInt32(a.AmendmentRequestNumber)).Max();


            //  return (from a in context.Declarations
            //          where  a.Tenant == tenant
            //          select a).Max(rec => Convert.ToInt32( rec.AmendmentRequestNumber));

            var list = (from a in context.Declarations
                        where a.Tenant == tenant && (a.AmendmentRequestNumber != null || a.CancelRequestNumber != null)
                        select new { a.AmendmentRequestNumber, a.CancelRequestNumber }).ToList();

            int max = 0;

            if (list.Count() != 0)
            {
                var maxAmendmentRequestNumber = list.Select(r => (int.TryParse(r.AmendmentRequestNumber, out var a)) ? int.Parse(r.AmendmentRequestNumber) : 0).ToList().Max();
                var maxCancelRequestNumber = list.Select(r => ((r.CancelRequestNumber).HasValue) ? r.CancelRequestNumber.Value : 0).ToList().Max();
                max = (maxAmendmentRequestNumber > maxCancelRequestNumber) ? maxAmendmentRequestNumber : maxCancelRequestNumber;
            }

            return max;
        }

        public void GetDailyStatistic(int tenant,
            out int TotDec,
            out int TotDecPay,
            out int TotDecOpen2Date,

            out int TotLastMonthCreatedByUserId,
            out int TotLastMonthReferentUserId,



            out int LastMonthTotDecStandAlone,
            out int LastMonthTotdecIsConnectedToUnf,

            out DateTime LastPaidDeclarationDate,
            out int TotWithHATARA
            )
        {
            TotLastMonthCreatedByUserId =
                TotLastMonthReferentUserId =
            TotWithHATARA = TotDecOpen2Date = TotDecPay = TotDec = -1;
            LastMonthTotDecStandAlone = LastMonthTotdecIsConnectedToUnf = -1;
            LastPaidDeclarationDate = DateTime.MinValue;

            var weekAgo = DateTime.Now.Date;//.AddDays(-7);
            var monthAgo = DateTime.Now.Date;//.AddMonths(-1);

            var qOpenLastWeek =
                (from a in context.Declarations
                 where
                 a.Tenant == tenant &&
                 a.CreateDateTime >= weekAgo
                 select a);

            var qOpenLastMonth =
                (from a in context.Declarations
                 where
                 a.Tenant == tenant &&
                 a.CreateDateTime >= monthAgo
                 select a);

            //var qOpenFrom =
            //    (
            //    from a in qOpenLastMonth
            //    group a by a.IsConnectedToUnifreight into g
            //    select new { g.Key, tot = g.Count() }
            //    );


            var qLastMonthTotDecStandAlone =
                (
                from a in qOpenLastMonth
                where a.IsConnectedToUnifreight == false
                select a
                );
            var qLastMonthTotdecIsConnectedToUnf =
                (
                from a in qOpenLastMonth
                where a.IsConnectedToUnifreight == true
                select a
                );

            var qOpenLastMonthCreatedByUserId =
                (
                from a in qOpenLastMonth
                select a.CreatedByUserId
                ).Distinct();
            var qOpenLastMonthReferentUserId =
                (
                from a in qOpenLastMonth
                select a.ReferentUserId
                ).Distinct();
            //var qOpenLastMonthCreatedByUserIdCount = qOpenLastMonthCreatedByUserId.




            var qLastPaidDeclarationDate = (from a in qOpenLastMonth
                                            where a.PaymentDate.HasValue &&
                                            //!string.IsNullOrWhiteSpace(a.PaymentOrderNumber)
                                            (a.PaymentOrderNumber != null || a.PaymentOrderNumber.Trim() != string.Empty)

                                            orderby a.PaymentDate descending
                                            select a.PaymentDate
                     );
            var qAllDecWithHATARA =
                (
                from a in context.Declarations
                where a.HatraDate.HasValue
                select a.Id
                ).Distinct(); ;
            var qq = (
                 from a in qOpenLastWeek
                 group a by 1 into gOpenLastWeek
                 select new
                 {
                     Tot = 0,
                     TotHATARA = 0,
                     TotPayment = 0,
                     TotOpenLastWeek = gOpenLastWeek.Count(),

                     TotLastMonthCreatedByUserId = 0,
                     TotLastMonthReferentUserId = 0,
                     LastMonthTotDecStandAlone = 0,
                     LastMonthTotdecIsConnectedToUnf = 0,
                     LastPaidDeclarationDate = DateTime.MinValue,
                 }
                 ).Concat(

                 from b in context.Declarations
                .Where(r => r.Tenant == tenant)
                .Where(r => r.PaymentDate.HasValue)
                 group b by 1 into gPaymentDate
                 select new //TotM()
                 {
                     Tot = 0,
                     TotHATARA = 0,
                     TotPayment = gPaymentDate.Count(),
                     TotOpenLastWeek = 0,
                     TotLastMonthCreatedByUserId = 0,
                     TotLastMonthReferentUserId = 0,
                     LastMonthTotDecStandAlone = 0,
                     LastMonthTotdecIsConnectedToUnf = 0,
                     LastPaidDeclarationDate = DateTime.MinValue,
                 }
                 ).Concat(
                 from c in context.Declarations
                 group c by 1 into gTot
                 select new //TotM()
                 {
                     Tot = gTot.Count(),
                     TotHATARA = qAllDecWithHATARA.Count(),
                     TotPayment = 0,
                     TotOpenLastWeek = 0,
                     TotLastMonthCreatedByUserId = qOpenLastMonthCreatedByUserId.Count(),
                     TotLastMonthReferentUserId = qOpenLastMonthReferentUserId.Count(),

                     LastMonthTotDecStandAlone = qLastMonthTotDecStandAlone.Count(),
                     LastMonthTotdecIsConnectedToUnf = qLastMonthTotdecIsConnectedToUnf.Count(),
                     LastPaidDeclarationDate = DateTime.MinValue,
                 }
                 )

                 ;

            var list = qq.ToList();
            if (list.Count > 0)
            {
                TotDec = list.Max(a => a.Tot);
                TotWithHATARA = list.Max(a => a.TotHATARA);
                TotDecPay = list.Max(a => a.TotPayment);
                TotDecOpen2Date = list.Max(a => a.TotOpenLastWeek);
                TotLastMonthCreatedByUserId = list.Max(a => a.TotLastMonthCreatedByUserId);
                TotLastMonthReferentUserId = list.Max(a => a.TotLastMonthReferentUserId);
                LastMonthTotDecStandAlone = list.Max(a => a.LastMonthTotDecStandAlone);
                LastMonthTotdecIsConnectedToUnf = list.Max(a => a.LastMonthTotdecIsConnectedToUnf);
                LastPaidDeclarationDate = qLastPaidDeclarationDate.FirstOrDefault().GetValueOrDefault();//  list.Max(A => A.LastPaidDeclarationDate); 
            }

            //return null;
        }
        //<--- Yuval Chalup 19.11.2015 TASK-17450
        public IQueryable<Declaration> GetSingleDeclarationPMByNumber(string number, int tenant)
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where a.DeclarationNumber == number && a.Tenant == tenant
                    select a);
        }
        //Yuval Chalup 19.11.2015 TASK-17450 --->

        public IQueryable<Declaration> GetQSingle(EntityKeyFields entityKeys)
        {
            DeclarationKeys keys = entityKeys as DeclarationKeys;
            return (from a in context.Declarations
                    where a.Id == keys.Id
                    select a);
        }
        public Declaration GetByCustomFileNo(string customFileNo, int tenant)
        {

            if (String.IsNullOrWhiteSpace(customFileNo)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }
        public string GetIdByCustomFileNo(string customFileNo, int tenant)
        {

            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public string GetIdByCustomFileNoAndAmendmentDontDisplayInList(string customFileNo, int tenant, bool AmendmentDontDisplayInList)
        {

            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant && rec.AmendmentDontDisplayInList == AmendmentDontDisplayInList
                  select rec.Id
                  )
                  .FirstOrDefault();
        }
        public List<string> GetListByCourierHAWB(string CourierHAWB, int tenant)
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 



            var q =
                  (
                  from rec in context.Declarations
                  where rec.CourierHAWB == CourierHAWB && rec.Tenant == tenant && rec.AmendmentDontDisplayInList == false

                  select rec.Id
                  );
            return q.ToList(); ;
        }
        public string GetConcurrencyGUIDByCustomFileNo(string customFileNo, int tenant)
        {

            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec.ConcurrencyGUID
                  )
                  .FirstOrDefault();
        }


        public string GetIdByExternalDeclarationNumber(string externalDeclarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(externalDeclarationNumber)) return "";
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.ExternalDeclarationNumber == externalDeclarationNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public string GetIdByDeclarationNumber(string declarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationNumber)) return "";
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.DeclarationNumber == declarationNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public (string id, string direction, string declarationTypeCode) GetMinDeclarationByDeclarationNumber(string declarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationNumber)) return (id: "", direction: "", declarationTypeCode: "1");// tuple literal
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            var res =
                  (
                  from rec in context.Declarations
                  where rec.DeclarationNumber == declarationNumber && rec.Tenant == tenant
                  select new { rec.Id, rec.Direction, rec.DeclarationTypeCode }
                  )
                  .FirstOrDefault();
            return (id: res.Id, direction: res.Direction, declarationTypeCode: res.DeclarationTypeCode);// tuple literal
        }

        public List<Declaration> GetDeclarationsById(List<string> declarationIds)
        {
            List<Declaration> declarations = (from a in context.Declarations
                                              where declarationIds.Contains(a.Id)
                                              select a).ToList();

            return declarations;

        }




        public List<Declaration> GetDeclarationsByIdAndClientID(List<string> declarationIds, string clientID)
        {
            DateTime month3ago = DateTime.Now.AddDays(-90);
            List<Declaration> declarations = (from a in context.Declarations
                                              where a.CustomerId == clientID && a.CreateDateTime > month3ago && declarationIds.Contains(a.Id)
                                              select a).ToList();

            return declarations;

        }
        public IQueryable<Declaration> GetByConsigmentExportContainerizationID(string exportContainerizationID, int tenant)
        {
            var query = (from b in context.Consignments
                         where b.ExportContainerizationID == exportContainerizationID && b.Tenant == tenant
                         select b).Select(c => c.DeclarationId).ToList();


            return (from a in context.Declarations
                    where query.Contains(a.Id) && a.Tenant == tenant && a.AmendmentDontDisplayInList != true
                    select a);
        }



        public Declaration GetDeclarationByFunctionalReferenceIDagentFileReferenceID(string functionalReferenceID, string agentFileReferenceID, int tenant, bool isExportClose = false)

        {
            //Declaration declarationParent = (from a in context.Declarations
            //                           where declarationNumber == a.DeclarationNumber
            //                           select a).FirstOrDefault();

            if (isExportClose)
            {
                var declaration = (from a in context.Declarations
                                   where functionalReferenceID == a.ExportCloseAmendRequestNumber && a.Tenant == tenant && a.CustomFileNo == agentFileReferenceID
                                   select a).FirstOrDefault();
                return declaration;

            }
            else
            {
                Declaration declaration = (from a in context.Declarations
                                           where functionalReferenceID == a.AmendmentRequestNumber && a.Tenant == tenant && a.CustomFileNo == agentFileReferenceID


                                           select a).FirstOrDefault();
                return declaration;
            }

        }
        public List<Declaration> GetDeclarationByFunctionalReferenceID(string functionalReferenceID, int tenant, bool isExportClose = true)

        {
            //Declaration declarationParent = (from a in context.Declarations
            //                           where declarationNumber == a.DeclarationNumber
            //                           select a).FirstOrDefault();

            if (isExportClose)
            {
                var declaration = (from a in context.Declarations
                                           where functionalReferenceID == a.ExportCloseAmendRequestNumber && a.Tenant == tenant
                                           select a).ToList();
                return declaration;

            }
            else
             {
                  List<Declaration> declaration = (from a in context.Declarations
                                        where functionalReferenceID == a.AmendmentRequestNumber && a.Tenant == tenant 
                                        select a).ToList();

             return declaration;

         }
        }




        public List<Declaration> GetDeclarationsThatCanResend(int tenant, int take)
        {
            DateTime month2Ago = DateTime.Now.AddDays(-60);
            var statuss = new List<string>() { "12", "13" };
            var myQ = (from a in context.Declarations
                       where a.CreateDateTime > month2Ago
                       where statuss.Contains(a.DeclarationStatusTypeCode)
                       select a);
            myQ = myQ.Take(take);
            var list = myQ.ToList();
            return list;
        }
        public Dictionary<string, string> GetCustomersByDeclarationIds(List<string> declarationIds, int tenant)
        {
            Dictionary<string, string> declarationCustomers = new Dictionary<string, string>();
            Dictionary<string, string> DeclarationCustomers = new Dictionary<string, string>();

            Dictionary<string, string> result = (from a in context.Declarations
                                                 where declarationIds.Contains(a.Id) && a.Tenant == tenant
                                                 select a).ToDictionary(d => d.Id, f => f.CustomerId);


            CardRepository cardRep = new CardRepository(tenant);
            declarationCustomers = cardRep.GetCustomerNamesById(result.Values.ToList());




            foreach (var item in result)
            {
                if (item.Value != null)
                {
                    string customerName = declarationCustomers[item.Value];
                    DeclarationCustomers.Add(item.Key, customerName);
                }

            }

            //declarationCustomers = result.Select(t => new { t.Id, t.CustomerId })
            //        .ToDictionary(t => t.Id, t => t.CustomerId);

            //foreach (var item in declarationCustomers)
            // {
            //  Card customerCard = CardRepository.GetSingleCard(item.Value, tenant, true);
            //  DeclarationCustomers.Add(item.Key,customerCard.LocalName);

            // }

            return DeclarationCustomers;

        }


        public List<SupplierInvoice> GetSupplierInvoicesByDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.InvoiceCurrencyTypeCode != null
                    select a).ToList();
        }

        public string GetCustomFileNoByDeclarationId(string declarationId, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationId)) return "";
            return
                  (
                  from rec in context.Declarations
                  where rec.Id == declarationId && rec.Tenant == tenant
                  select rec.CustomFileNo
                  )
                  .FirstOrDefault();
        }

        public List<string> GetCustomsFileNumbersByDeclaraionIds(List<string> declarationIds, int tenant)
        {
            List<string> declarations = (from a in context.Declarations
                                         where declarationIds.Contains(a.Id) && a.Tenant == tenant
                                         select a.CustomFileNo).ToList();

            return declarations;
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<Declaration>(rec => rec.Id == entityKeyFields.Id);
        }

        public Declaration GetSingleDeclarationById(string id, int tenant)
        {
            Declaration declaration = (from a in context.Declarations
                                       where a.Id == id && a.Tenant == tenant
                                       select a).FirstOrDefault();

            return declaration;
        }

        public int GetInvoiceItemsWithTradeAgreementCount(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.TradeAgreementCode != null
                    select a).Count();
        }
        public List<string> GetIdsThatIsChanged(int tenant, List<string> DecIds)
        {

            var declarations = (from a in context.Declarations
                                where a.Tenant == tenant
                                where DecIds.Contains(a.Id)
                                where a.IsChanged
                                select a.Id
                                                    );

            return declarations.ToList();
        }

        public IQueryable<Declaration> GetCourierConnectedDeclaratins(string CourierMasterId, int tenant)
        {
            var /*List<string>*/ declarations = (from a in context.CourierDeclarations
                                                 join d in context.Declarations
                                                 on a.DeclarationId equals d.Id
                                                 where a.CourierMasterId == CourierMasterId && a.Tenant == tenant &&
                                                 !d.AmendmentDontDisplayInList
                                                 select d)/*.ToList()*/;

            //var /*List<string>*/ courierDeclarations = (from a in context.CourierDeclarations
            ///              where a.CourierMasterId == CourierMasterId && a.Tenant == tenant
            //                select a.DeclarationId)/*.ToList()*/;

            //IQueryable<Declaration> declarations = (from a in context.Declarations
            //                       where courierDeclarations.Contains(a.Id) && !a.AmendmentDontDisplayInList && a.Tenant == tenant
            //                             select a);

            return declarations;
        }

        public IQueryable<Declaration> GetNotConnectedDeclarations(int tenant)
        {
            bool joinIt = false;

#if (false)
            {
                List<string> courierDeclarations = (from a in context.CourierDeclarations
                                                    where a.CourierMasterId != null && a.Tenant == tenant
                                                    select a.DeclarationId).ToList();

                IQueryable<Declaration> declarations = (from a in context.Declarations
                                                        where !courierDeclarations.Contains(a.Id) && a.IsCourierDeclaration == true
                                                        select a);
            }
#endif
            IQueryable<Declaration> declarations = null;
            if (!joinIt)
            {
                var courierDeclarations = (from a in context.CourierDeclarations
                                           where a.CourierMasterId != null && a.Tenant == tenant
                                           select a.DeclarationId);

                declarations = (from a in context.Declarations
                                where !courierDeclarations.Contains(a.Id) && a.IsCourierDeclaration == true && !a.IsCancelled && !a.AmendmentDontDisplayInList && a.Tenant == tenant
                                select a);
            }
            else
            {
                declarations = (from a in context.Declarations

                                where /*!courierDeclarations.Contains(a.Id) */
                                !context.CourierDeclarations.Any(cd => cd.DeclarationId == a.Id)
                                && a.IsCourierDeclaration == true && !a.IsCancelled && !a.AmendmentDontDisplayInList && a.Tenant == tenant
                                select a
                 );

            }
            bool testIt = false;
            if (testIt)
            {
                var res = declarations.ToList();
            }
            return declarations;
        }


        public void SetIsChangedAndSubmitChanges(Declaration declaration)
        {
            if (!declaration.PaymentDate.HasValue)
            {
                declaration.IsChanged = true;
                this.Update(declaration);
                this.SubmitChanges();
            }
        }

        public bool GetIsValueForCustomsOnlyFromDeclaration(string declarationId, int tenant)
        {
            bool? IsValueForCustomsOnly = (from a in context.Declarations
                                           where a.Id == declarationId && a.Tenant == tenant
                                           select a.IsValueForCustomsOnly).FirstOrDefault();
            return IsValueForCustomsOnly ?? IsValueForCustomsOnly.Value;
        }

        public string GetCusomFileNoForDeclaration(string declarationId, int tenant)
        {
            Declaration dec = (from a in context.Declarations
                               where a.Id == declarationId && a.Tenant == tenant
                               select a).FirstOrDefault();

            return dec != null ? dec.CustomFileNo : null;
        }

        public Declaration GetDeclarationByDecNoAndVersion(string decNo, string version, int tenant)
        {
            if (String.IsNullOrWhiteSpace(decNo)) return null;
            if (String.IsNullOrWhiteSpace(version)) return null;

            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.DeclarationNumber == decNo && rec.VersionId == version && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }

        public Declaration GetDeclarationByCustomFileNo(string customFileNo, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }
        public Declaration GetLastDeclarationByDeclarationId(string id, int tenant, bool isExport = false)
        {
            if (String.IsNullOrWhiteSpace(id)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
            if (isExport)
            {
                return
                 (
                 from rec in context.Declarations
                 where rec.AmendmentOriginalDeclartation == id && rec.Tenant == tenant && (rec.AmendmentStatus == null || rec.AmendmentStatus == "6")
                 select rec
                 ).OrderByDescending(x => x.CreateDateTime)
                 .FirstOrDefault();
            }
            return
                  (
                  from rec in context.Declarations
                  where rec.AmendmentOriginalDeclartation == id && rec.Tenant == tenant
                  select rec
                  ).OrderByDescending(x => x.CreateDateTime)
                  .FirstOrDefault();
        }


        public List<Declaration> GetDeclarationById(int tenant, string id)
        {
            var myQ = (from a in context.Declarations
                       where a.Id == id && a.Tenant == tenant
                       select a);
            return myQ.ToList();

        }
        public List<Declaration> GetDeclarationAmendmentsById(int tenant, string id)
        {

            if (String.IsNullOrWhiteSpace(id)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            Declaration declaration = GetSingleDeclarationById(id, tenant);

            if (declaration.IsAmendment == true)
            {
                Declaration declarationOrg = GetSingleDeclarationById(declaration.AmendmentOriginalDeclartation, tenant);

                var myQ = (from a in context.Declarations
                           where (a.AmendmentOriginalDeclartation == declarationOrg.Id || a.Id == declarationOrg.Id) && a.Id != id
                           select a);
                return myQ.ToList();
            }

            else
            {
                var myQ = (from a in context.Declarations
                           where a.AmendmentOriginalDeclartation == id
                           select a);
                return myQ.ToList();
            }
        }

        public List<DeclarationPendingBulkFeed> GetforPendingBulkFeed(string courierMasterId, string goodsDescription, string weightFrom, string weightTo, string incotermCode, string SearchFilter, string totalInvoice, string fastIndividualProcess, int? skip = null, int? take = null, string sortingCol = null, string sortingDir = null)
        {
            int weightFromInt = 0;
            int weightToInt = 0;

            var q1 = (from cd in context.CourierDeclarations.Where(cd => cd.CourierMasterId == courierMasterId)

                      join d in context.Declarations on cd.DeclarationId equals d.Id

                      join dcs in context.DeclarationCourierStatuses on d.Id equals dcs.DeclarationId

                      join cp in context.ConsignmentPackages on d.Id equals cp.DeclarationId into cpjoin
                      from cpj in cpjoin.Where(cp => cp.PackageMeasureQualifierCode == "2").DefaultIfEmpty()

                      join c in context.Clients on d.ImporterId equals c.Id into cjoin
                      from cj in cjoin.DefaultIfEmpty()

                      join s in (from temp in context.SupplierInvoices
                                 group temp by temp.DeclarationId into temp2
                                 where temp2.Count() > 0
                                 select new { DeclarationId = temp2.Key, SupplierInvoices = temp2.FirstOrDefault() })
                               on d.Id equals s.DeclarationId into sjoin
                      from sj in sjoin.DefaultIfEmpty()

                          //join s in context.SupplierInvoices on d.Id equals s.DeclarationId into sjoin
                          //let sj = context.SupplierInvoices.Where(s=> d.Id == s.DeclarationId).FirstOrDefault()
                      orderby d.Id

                      select new
                      {
                          DeclarationId = d.Id,
                          CourierHAWB = d.CourierHAWB,
                          Importername = d.ImporterName,
                          Cargodescription = d.CargoDescription,
                          Casualimporteraddress1 = d.CasualImporterAddress1,
                          Casualimporteraddress2 = d.CasualImporterAddress2,
                          Casualimportercity = d.CasualImporterCity,
                          TotalInvoiceAmountInUSD = dcs.TotalInvoiceAmountInUSD,
                          PackageMeasureQualifierCode1 = cpj != null ? cpj.PackageMeasureQualifierCode : "0",// t.AsEnumerable().Sum(cpj => int.Parse(cpj.PackageMeasureQualifierCode)),
                          Code = cj != null ? cj.Code : d.ImporterCode,
                          IncotermCode = sj != null ? sj.SupplierInvoices.IncotermCode : "",
                          CourierSearchFields = d.CourierSearchFields,
                          FastIndividualProcessCode = dcs.FastIndividualProcessCode
                      });

            if (skip.HasValue)
                q1 = q1.OrderBy(x => x.CourierHAWB).Skip(skip.Value);

            if (take.HasValue)
                q1 = q1.Take(take.Value);

            if (!string.IsNullOrEmpty(incotermCode))
                q1 = q1.Where(s => s.IncotermCode == incotermCode);

            if (!string.IsNullOrEmpty(goodsDescription))
                q1 = q1.Where(s => s.Cargodescription.Contains(goodsDescription));

            if (!string.IsNullOrEmpty(SearchFilter))
                q1 = q1.Where(s => s.CourierSearchFields.Contains(SearchFilter));


            switch (totalInvoice)
            {
                case "75":
                    {
                        q1 = q1.Where(r => r.TotalInvoiceAmountInUSD <= 75);

                        break;
                    }
                case "500":
                    {
                        q1 = q1.Where(r => r.TotalInvoiceAmountInUSD > 75 && r.TotalInvoiceAmountInUSD <= 500);
                        break;
                    }
                case "1000":
                    {
                        q1 = q1.Where(r => r.TotalInvoiceAmountInUSD > 500 && r.TotalInvoiceAmountInUSD <= 1000);

                        break;
                    }
            }

            switch (fastIndividualProcess)
            {
                case "F":
                case "I":
                    {
                        q1 = q1.Where(r => r.FastIndividualProcessCode == fastIndividualProcess);
                        break;
                    }
            }


            var res = q1.ToList();


            var q2 = res.GroupBy(d =>
                new
                {
                    d.DeclarationId,
                    d.CourierHAWB,
                    d.Importername,
                    d.Cargodescription,
                    d.Casualimporteraddress1,
                    d.Casualimporteraddress2,
                    d.Casualimportercity,
                    d.TotalInvoiceAmountInUSD,
                    d.PackageMeasureQualifierCode1,
                    d.Code,
                    d.IncotermCode
                }).Select(t => new DeclarationPendingBulkFeed()
                {
                    DeclarationId = t.Key.DeclarationId,
                    CourierHAWB = t.Key.CourierHAWB,
                    Importername = t.Key.Importername,
                    Cargodescription = t.Key.Cargodescription,
                    Casualimporteraddress = t.Key.Casualimporteraddress1 + " " + t.Key.Casualimporteraddress2,
                    Casualimportercity = t.Key.Casualimportercity,
                    TotalInvoiceAmountInUSD = t.Key.TotalInvoiceAmountInUSD,
                    PackageMeasureQualifierCode = t.Sum(cpj => Convert.ToInt32(cpj.PackageMeasureQualifierCode1)),
                    Code = t.Key.Code,
                    IncotermCode = t.Key.IncotermCode
                });

            if (int.TryParse(weightFrom, out weightFromInt))
                q2 = q2.Where(s => s.PackageMeasureQualifierCode >= weightFromInt);

            if (int.TryParse(weightTo, out weightToInt))
                q2 = q2.Where(s => s.PackageMeasureQualifierCode <= weightToInt);

            if (!string.IsNullOrEmpty(sortingCol))
            {
                var sortBy = new Dictionary<string, Func<IEnumerable<DeclarationPendingBulkFeed>, IEnumerable<DeclarationPendingBulkFeed>>>()
                {
                    { "CourierHAWB", lus => lus.OrderBy(lu => lu.CourierHAWB) },
                    { "Importername", lus => lus.OrderBy(lu => lu.Importername) },
                    { "Code", lus => lus.OrderBy(lu => lu.Code) },
                    { "Cargodescription", lus => lus.OrderBy(lu => lu.Cargodescription) },
                    { "IncotermCode", lus => lus.OrderBy(lu => lu.IncotermCode) },
                    { "TotalInvoiceAmountInUSD", lus => lus.OrderBy(lu => lu.TotalInvoiceAmountInUSD) },
                    { "PackageMeasureQualifierCode", lus => lus.OrderBy(lu => lu.PackageMeasureQualifierCode) },
                    { "Casualimportercity", lus => lus.OrderBy(lu => lu.Casualimportercity) },
                };
                q2 = sortBy[sortingCol](q2);

                if (sortingDir == "Descending")
                    q2 = q2.Reverse();
            };

            var res2 = q2.ToList();

            return res2;
        }

        public string GetHatraDateForDecId(string decId, int tenant)
        {
            var HatraDateQuery = (from a in context.Declarations
                                  where a.Id == decId && a.Tenant == tenant
                                  select a.HatraDate);
            return HatraDateQuery.FirstOrDefault().ToString();
        }

        public List<string> GetDeclarationsByCourierHAWBsExpectDecWithHatraDate(List<string> courierHAWBs, int tenant)
        {

            var q = (from a in context.Declarations
                     where courierHAWBs.Contains(a.CourierHAWB)
                     where a.Tenant == tenant && a.HatraDate == null
                     select a.Id);
            return q.ToList();
        }


        public DeclarationConsignments GetDeclarationConsignment(string exportFile)
        {
            var declarationsQ = (from d in context.Declarations
                                 where d.ExportFile == exportFile && !d.AmendmentDontDisplayInList
                                 select d
                       );
            List<Declaration> declarations = declarationsQ.ToList();

            var consignmentsQ = (from d in context.Declarations

                                 join c in context.Consignments
                                 on d.Id equals c.DeclarationId into cjoin
                                 from cj in cjoin.DefaultIfEmpty()

                                 where d.ExportFile == exportFile && !d.AmendmentDontDisplayInList
                                 select cj
                              ); 
            List<Consignment> consignments = consignmentsQ.ToList();

            var myQ2 = (from d in context.Declarations.Where(d => d.ExportFile == exportFile && !d.AmendmentDontDisplayInList).Take(1)

                        join c in context.ConsignmentPackages.Select(x => new ConsignmentPackagesShort { DeclarationId = x.DeclarationId, PackageTypeCode = x.PackageTypeCode, Quantity = x.PackageQuantity.Value }) on d.Id equals c.DeclarationId into cjoin
                        from cj in cjoin.DefaultIfEmpty()

                        select new ConsignmentPackagesShort { DeclarationId = cj.DeclarationId, PackageTypeCode = cj.PackageTypeCode, Quantity = cj.Quantity }
                    );
            List<ConsignmentPackagesShort> ConsignmentPackages = myQ2.ToList();

            return new DeclarationConsignments { ConsignmentPackages = ConsignmentPackages, Declarations = declarations, Consignments = consignments };
        }

        public DeclarationId GetDeclarationId(string exportFileNo, string exporterNumber, string transportmodeId, string cargoIdentifierType, string cargoIdentifierKey1, string cargoIdentifierKey2, string cargoIdentifierKey3)
        {
            var myQ = (from d in context.Declarations
                       join c in context.Consignments on d.Id equals c.DeclarationId into cjoin
                       from cj in cjoin.DefaultIfEmpty()

                       where d.ExportFile == exportFileNo
                       && d.ImporterCode == exporterNumber
                       && d.TransportModeId == transportmodeId
                       && cj.CargoTypeCode == cargoIdentifierType
                       && cj.ManifestNumber == cargoIdentifierKey1
                       && cj.SecondCargoID == cargoIdentifierKey2
                       && cj.ThirdCargoID == cargoIdentifierKey3
                       select new DeclarationId { Id = d.Id }
                       );
            DeclarationId res = myQ.Take(1).ToList().FirstOrDefault();
            return res;
        }
        public ExportStorageConnectToDeclaration GetExportStorageConnectToDeclaration(string declarationId, int tenant)
        {
            var actionCodes = new List<string> { "4", "6", "8" };

            ExportStorageConnectToDeclaration res = new ExportStorageConnectToDeclaration();

            var q = (
                from d in context.Declarations.Where(x => x.Id == declarationId && x.Tenant == tenant)

                join e in context.ExportStorages.Where(x => x.Tenant == tenant) on d.ExportFile equals e.ExportFileNo into ejoin
                from ej in ejoin

                select new
                {
                    DeclarationId = ej.DeclarationId,
                    CustomsStatus = ej.CustomsStatus,
                    ActionCode = ej.ActionCode
                })
                .GroupBy(x => true)
                .Select(g => new ExportStorageConnectToDeclaration
                {
                    NotConnect = g.Sum(x => string.IsNullOrEmpty(x.DeclarationId) ? 1 : 0),
                    Connect = g.Sum(x => x.DeclarationId == declarationId ? 1 : 0),
                    CustomsStatus = g.Sum(x => x.CustomsStatus == "1" || x.CustomsStatus == "6" ? 1 : 0),
                    ActionCode = g.Sum(x => actionCodes.Contains(x.ActionCode) ? 1 : 0)
                });
            if (q.ToList().FirstOrDefault() != null)
            {
                res = q.ToList().FirstOrDefault();
            }

            return res;
        }

        public List<ContainerizationUniqueConsignment> GetContainerizationUniqueConsignment(List<string> declarationList)
        {
            try
            {
                var query = (from a in context.Declarations
                             where declarationList.Contains(a.Id)
                             select a);
                var query1 = (from a in context.Consignments
                              where declarationList.Contains(a.DeclarationId) && a.ExportContainerizationID == null && !string.IsNullOrEmpty(a.CargoTypeCode) && !string.IsNullOrEmpty(a.ManifestNumber)
                              select a);
                var query2 = (from b in context.Containerizations
                              select b).Select(t => new ContainerizationKey
                              {
                                  Id = t.Id,
                                  Key = t.CargoTypeCode + t.ManifestNumber + t.SecondCargoID + t.ThirdCargoID,
                              });

                List<ContainerizationKey> ck1 = new List<ContainerizationKey>();
                ck1 = query2.Where(t => t.Key != null).ToList();

                var q1 = query1.GroupBy(cont => new { ManifestNumber = cont.ManifestNumber, CargoTypeCode = cont.CargoTypeCode, SecondCargoID = cont.SecondCargoID, ThirdCargoID = cont.ThirdCargoID })
                 .ToList();
                //òãëåï äîëìä
                var q2 = q1.Where(x => x.Count() >= 1 && x.Any(u => (ck1.Any(g => g.Key.Contains(u.CargoTypeCode?.ToLower() + u.ManifestNumber?.ToLower() + u.SecondCargoID?.ToLower() + u.ThirdCargoID?.ToLower())))));

                //äåñôú äîëìä çãùä
                var q3 = q1.Where(x => x.Count() >= 1 && !(x.Any(u => (ck1.Any(g => g.Key.Contains(u.CargoTypeCode?.ToLower() + u.ManifestNumber?.ToLower() + u.SecondCargoID?.ToLower() + u.ThirdCargoID?.ToLower()))))));

                var q4 = q2.SelectMany(x => x.Select(cont => new ContainerizationUniqueConsignment
                {
                    DeclarationId = cont.DeclarationId,
                    CargoTypeCode = cont.CargoTypeCode,
                    ManifestNumber = cont.ManifestNumber,
                    SecondCargoId = cont.SecondCargoID,
                    ThirdCargoId = cont.ThirdCargoID,
                    IsNew = false,
                    Id = ck1.Where(f => f.Key.Contains(cont.CargoTypeCode?.ToLower() + cont.ManifestNumber?.ToLower() + cont.SecondCargoID?.ToLower() + cont.ThirdCargoID?.ToLower())).Select(y => y.Id).FirstOrDefault().ToString(),

                })).ToList();

                var q5 = q3.SelectMany(x => x.Select(cont => new ContainerizationUniqueConsignment
                {
                    DeclarationId = cont.DeclarationId,
                    CargoTypeCode = cont.CargoTypeCode,
                    ManifestNumber = cont.ManifestNumber,
                    SecondCargoId = cont.SecondCargoID,
                    ThirdCargoId = cont.ThirdCargoID,
                    IsNew = true,
                    TransportModeId = query.Where(t => t.Id == cont.DeclarationId).Select(y => y.TransportModeId).FirstOrDefault().ToString(),
                })).ToList();

                return q4.Union(q5).ToList();
            }
            catch (Exception ex)
            {
                var mess = ex.Message.ToString();
                return null;
            }


        }
        public Declaration GetDeclarationByDeclarationNum(string decNumber, int tenant)
        {
            var query = (from a
                        in context.Declarations
                         where a.DeclarationNumber == decNumber && a.Tenant == tenant
                         select a).FirstOrDefault();
            return query;
        }



        public List<ExportReport1> GetReportDeclarationForExportReport1(DateTime? ExportFrom, DateTime? ExportTo)
        {
            List<ExportReport1> ExportReports = new List<ExportReport1>();
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {


                SqlParameter pFrom = new SqlParameter("@pFrom", SqlDbType.Date);
                SqlParameter pTo = new SqlParameter("@pTo", SqlDbType.Date);

                pFrom.Direction = ParameterDirection.Input;
                pTo.Direction = ParameterDirection.Input;

                pFrom.Value = ExportFrom;
                pTo.Value = ExportTo.Value.AddDays(1);

                SqlCommand cmd = new SqlCommand("dbo.usp_ExportReport", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(pFrom);
                cmd.Parameters.Add(pTo);

                cn.Open();


                SqlDataReader reader = cmd.ExecuteReader();


                ExportReport1 exportReport = null;

                while (reader.Read())
                {
                    exportReport = new ExportReport1();
                    exportReport.company = reader["company"].ToString();
                    exportReport.submit = int.Parse(reader["submit"].ToString());
                    exportReport.release = int.Parse(reader["release"].ToString());
                    exportReport.closed = int.Parse(reader["closed"].ToString());
                    exportReport.notSubmit = int.Parse(reader["notSubmit"].ToString());
                    ExportReports.Add(exportReport);
                }


                cn.Close();
            }

            return ExportReports;

        }


        public List<ExportReport2> GetReportDeclarationForExportReport2(DateTime? ExportFrom, DateTime? ExportTo)
        {

            List<ExportReport2> ExportReports2 = new List<ExportReport2>();
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            using (SqlConnection cn = new SqlConnection(strConnString))
            {

                SqlParameter pFrom = new SqlParameter("@pFrom", SqlDbType.Date);
                SqlParameter pTo = new SqlParameter("@pTo", SqlDbType.Date);

                pFrom.Direction = ParameterDirection.Input;
                pTo.Direction = ParameterDirection.Input;

                pFrom.Value = ExportFrom;
                pTo.Value = ExportTo.Value.AddDays(1);

                SqlCommand cmd = new SqlCommand("dbo.usp_ExportReport2", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(pFrom);
                cmd.Parameters.Add(pTo);


                cn.Open();


                SqlDataReader reader = cmd.ExecuteReader();


                ExportReport2 exportReport = null;

                while (reader.Read())
                {
                    exportReport = new ExportReport2();
                    exportReport.company = reader["company"].ToString();
                    exportReport.localname = reader["localname"].ToString();
                    exportReport.count = int.Parse(reader["count"].ToString());

                    ExportReports2.Add(exportReport);
                }


                cn.Close();
            }

            return ExportReports2;

        }    

        public List<string> GetDisplayOnly(string[] declarationsId, int tenant, string[] sheetStatusInProcessId)
        {
            var q = from d in context.Declarations
                    join c in context.CustomsRequestsSheets on d.CustomFileNo equals c.CustomFileNo into cJoin
                    from cd in cJoin.DefaultIfEmpty()

                    where d.Tenant == tenant &&                        
                        declarationsId.Contains(d.Id) &&
                        cd.Tenant == tenant &&
                    (
                        d.PaymentDate.HasValue == true
                        || d.IsConvertedDeclaration == true
                        || new string[] { "10", "11" }.Contains(d.DeclarationStatusTypeCode)
                        || 
                        (
                            sheetStatusInProcessId.Contains(cd.RequestStatusCode)
                            && new string[] { "2750", "2754", "2755", "8211", "8212", "8214", "8215", "8216", "8227", "US2L01" }.Contains(cd.InterfaceTypeCode)
                        )
                    )
                    select d.Id;

            return q.Distinct().ToList();
        }

        public string GetDeclaratNumberByCustomFileNo(int tenant, string customFileNo, string direction)
        {
            var decNum = (from a in context.Declarations
                       where a.CustomFileNo == customFileNo && a.DeclarationNumber != null && a.Direction == direction && a.Tenant == tenant
                       select a.DeclarationNumber).FirstOrDefault();
            return decNum;

        }
    }


    public class ExportReport1
    {
        public string company { get; set; }
        public int submit { get; set; }
        public int release { get; set; }

        public int closed { get; set; }
        public int notSubmit { get; set; }


    }

    public class ExportReport2
    {
        public string company { get; set; }
        public string localname { get; set; }
        public int count { get; set; }


    }


    public class ContainerizationKey
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string TransportModeId { get; set; }

    }

    
    public class DeclarationId
    {
        public string Id { get; set; }
    }

    public class ConsignmentPackagesShort
    {
        public string PackageTypeCode { get; set; }
        public string DeclarationId { get; set; }
        public int Quantity { get; set; }
    }

    public class DeclarationConsignments
    {
        public List<ConsignmentPackagesShort> ConsignmentPackages { get; set; }
        public List<Declaration> Declarations { get; set; }
        public List<Consignment> Consignments { get; set; }
    }



    public class DeclarationPendingBulkFeed
    {
        public string DeclarationId { get; set; }
        public string CourierHAWB { get; set; }
        public string Importername { get; set; }
        public string Cargodescription { get; set; }
        public string Casualimporteraddress { get; set; }

        public string Casualimportercity { get; set; }
        public decimal? TotalInvoiceAmountInUSD { get; set; }
        public int PackageMeasureQualifierCode { get; set; }
        public string Code { get; set; }
        public string IncotermCode { get; set; }

        public DeclarationPendingBulkFeed() { }

        public DeclarationPendingBulkFeed(string declarationId, string courierHAWB, string importername, string cargodescription, string casualimporteraddress, string casualimportercity, decimal? totalInvoiceAmountInUSD, int packageMeasureQualifierCode, string code, string incotermCode)
        {
            DeclarationId = declarationId;
            CourierHAWB = courierHAWB;
            Importername = importername;
            Cargodescription = cargodescription;
            Casualimporteraddress = casualimporteraddress;
            Casualimportercity = casualimportercity;
            TotalInvoiceAmountInUSD = totalInvoiceAmountInUSD;
            PackageMeasureQualifierCode = packageMeasureQualifierCode;
            Code = code;
            IncotermCode = incotermCode;
        }
    }

    public class TempBulkFeedPending
    {
        public string Id { get; set; }
        public string Importername { get; set; }
        public string Cargodescription { get; set; }
        public string Casualimporteraddress1 { get; set; }
        public string Casualimporteraddress2 { get; set; }
        public string Casualimportercity { get; set; }
        public decimal? Totalinvoiceamountinus { get; set; }
        public string PackageMeasureQualifierCode1 { get; set; }
        public string Code { get; set; }
        public string IncotermCode { get; set; }

        public TempBulkFeedPending() { }

        public TempBulkFeedPending(string id, string importername, string cargodescription, string casualimporteraddress1, string casualimporteraddress2, string casualimportercity, decimal? totalinvoiceamountinus, string packageMeasureQualifierCode1, string code, string incotermCode)
        {
            Id = id;
            Importername = importername;
            Cargodescription = cargodescription;
            Casualimporteraddress1 = casualimporteraddress1;
            Casualimporteraddress2 = casualimporteraddress2;
            Casualimportercity = casualimportercity;
            Totalinvoiceamountinus = totalinvoiceamountinus;
            PackageMeasureQualifierCode1 = packageMeasureQualifierCode1;
            Code = code;
            IncotermCode = incotermCode;
        }
    }


    public class ExportStorageConnectToDeclaration
    {
        public int NotConnect { get; set; }
        public int Connect { get; set; }
        public int CustomsStatus { get; set; }
        public int ActionCode { get; set; }


        public ExportStorageConnectToDeclaration()
        {
            NotConnect = 0;
            Connect = 0;
            CustomsStatus = 0;
            ActionCode = 0;

        }
    }

    public class ContainerizationUniqueConsignment
    {
        public string CargoTypeCode { get; set; }
        public string ManifestNumber { get; set; }
        public string SecondCargoId { get; set; }
        public string ThirdCargoId { get; set; }
        public string DeclarationId { get; set; }
        public bool IsNew { get; set; }
        public string Id { get; set; }
        public string TransportModeId { get; set; }

    }


}


