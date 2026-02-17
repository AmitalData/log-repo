
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityPMs.UGenerated;
using System.Diagnostics;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {


        public GTBITEMPM GetSingleGTBITEMPM(string code, int tenant)
        {
            //GTBITEMQuery = new GTBITEMQueryService(GetAmitalContext(tenant));
            //GTBITEMPM GTBITEM = GTBITEMQuery.GetSingle(code, false, false);
            //return GTBITEM;
            return null;
        }


        public List<CustomsPartnersItemList> GetGTBITEMPartnersItemList(string vendorId, string customerCode, int tenant, string search, int top)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return null;
                }

                SecurityUtility.AuthenticationOnTenant(tenant);

                var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
                var itemRepo = new GTBITEMRepository(GetAmitalContext(tenant));

                string partner = GetDefault("ISRAEL", "CIM_SIVUG_103", "NON", customerCode, tenant); // S=Supplier I=Client
                    Debug.WriteLine("GetDefault" + sw.ElapsedMilliseconds);
                if (partner == "S") // If Supplier get Unifreight card
                {
                    customerCode = GetDefaultAccountNumber("ISRAEL", "CEX_CUS_SUP", "NON", customerCode, tenant);
                        Debug.WriteLine("GetDefaultAccountNumber" + sw.ElapsedMilliseconds);
                }

                if (string.IsNullOrWhiteSpace(customerCode))
                {
                    return null;
                }


                var cardDetails = cardRepo.GetAll().Where(rec => rec.CARDID == customerCode).FirstOrDefault();

        //    var q =
        //    from itm in itemRepo
        //        .GetAll()
        //        .Select( rec=> new CustomsPartnersItemList(){
        //            Id = rec.PRATID,
        //            ClassificationCode = rec.PRATID,
        //            ItemCode = rec.ITEMID,
        //            CustomerId = rec.PARTNERID,
        //            CustomerName = rec.PARTNERID,
        //            Name = rec.DESCRIPTION,
        //            VendorId = rec.PARTNERID,
        //            SearchFields =rec.SEARCHENG 
        //    })
        //    join card in cardRepo.GetAll() on itm.CustomerId equals card.CARDID into itmCard
        //from ic in itmCard.DefaultIfEmpty()
        //select new { itm, ic };
        //    if (!string.IsNullOrWhiteSpace(customerCode))
        //    {
        //            q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
        //    }
        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        search = search.ToUpper();
        //        //var searchById = q.Where(rec => rec.itm.ITEMID.Contains(search));
        //        //var searchByName = q.Where(rec => rec.itm.SEARCHENG.Contains(search));
        //            q = q.Where(rec => rec.itm.ItemCode.Contains(search) | rec.itm.SearchFields.Contains(search));
        //    }
        //    //GetCustomsPartnersItemList(itm, ic, tenant);
        //    q = q
        //        //.OrderBy(rec => rec.itm.ITEMID)
        //         .Take(top);

        //        var aynList = q.ToList();
        //        Debug.WriteLine("ToList():" + sw.ElapsedMilliseconds);
        //    var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, rec.ic, tenant)).ToList();
        //    return l;
        //    //var listService = new GTBITEMQueryService(GetAmitalContext(tenant));
        //    //var listPM= listService.GetListBy(vendorId, customerId,search, top);
        //    //return listPM.Select(pm => GetCustomsPartnersItemList(pm, tenant)).ToList(); 


                var q =
                from itm in itemRepo
                    .GetAll()
                    .Select(rec => new CustomsPartnersItemList()
                    {
                        Id = rec.ITEMID,
                        ClassificationCode = rec.PRATID,
                        ItemCode = rec.ITEMID,
                        CustomerId = rec.PARTNERID,
                        CustomerName = rec.PARTNERID,
                        Name = rec.DESCRIPTION,
                        VendorId = rec.PARTNERID,
                        SearchFields = rec.SEARCHENG
                    })
                select new { itm };
                if (!string.IsNullOrWhiteSpace(customerCode))
                {
                    q = q.Where(rec => rec.itm.CustomerId.Equals(customerCode));
                }
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToUpper();
                    q = q.Where(rec => rec.itm.ItemCode.Contains(search) | rec.itm.SearchFields.Contains(search));
                }
                q = q.Distinct();
                q = q.Take(top);

                var aynList = q.ToList();
                Debug.WriteLine("ToList():" + sw.ElapsedMilliseconds);
                var l = aynList.Select(rec => GetCustomsPartnersItemList(rec.itm, cardDetails, tenant)).ToList();
                return l;

            }
            finally
            {
                Debug.WriteLine("GTBITEMS" + sw.ElapsedMilliseconds);
            }
        }

        private CustomsPartnersItemList GetCustomsPartnersItemList(CustomsPartnersItemList item, GNDCARD card, int tenant)
        {
            var crd = card ?? new GNDCARD();
            item.VendorName = crd.NAMEENG;
            return item;

        }

        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            var myGDFDATAQueryService = new GDFDATAQueryService(GetAmitalContext(tenant));

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }

        private string GetDefaultAccountNumber(string DISTRID, string DEFID, string BRANCHID, string SHORTDEFDATA, int tenant)
        {
            var myGDFDATAQueryService = new GDFDATAQueryService(GetAmitalContext(tenant));

            if (DISTRID == null || DEFID == null || BRANCHID == null || SHORTDEFDATA == null)
            {
                return ("");
            }

            string accountNumber = myGDFDATAQueryService.GetCardIdByDefaultValue(DISTRID, DEFID, BRANCHID, SHORTDEFDATA);

            return (accountNumber);
        }

        public CustomsPartnersItemPM GetGTBITEMDetailsByPartnerAndItem(string customerCode, string itemId, int tenant, string search)
        {
            if (string.IsNullOrWhiteSpace(customerCode) || string.IsNullOrWhiteSpace(itemId))
            {
                return null;
            }

            SecurityUtility.AuthenticationOnTenant(tenant);

            var cardRepo = new GNDCARDRepository(GetAmitalContext(tenant));
            var itemRepo = new GTBITEMRepository(GetAmitalContext(tenant));

            string partner = GetDefault("ISRAEL", "CIM_SIVUG_103", "NON", customerCode, tenant); // S=Supplier I=Client
            if (partner == "S") // If Supplier get Unifreight card
            {
                customerCode = GetDefaultAccountNumber("ISRAEL", "CEX_CUS_SUP", "NON", customerCode, tenant);
            }

            if (string.IsNullOrWhiteSpace(customerCode))
            {
                return null;
            }

            var myGTBITEMQueryService = new GTBITEMQueryService(GetAmitalContext(tenant));
            GTBITEMPM myGTBITEMPM = myGTBITEMQueryService.GetSingle(customerCode, itemId, false); // Not From Cache
            if (myGTBITEMPM == null)
            {
                return null;
            }

            return GetCustomsPartnersItem(myGTBITEMPM, tenant);

        }

        private CustomsPartnersItemPM GetCustomsPartnersItem(GTBITEMPM item, int tenant)
        {
            if (item == null)
            {
                return null;
            }
            var mustKey=true;
            if (mustKey)
            {
                if (String.IsNullOrEmpty(item.PRATID))
                {
                    item.PRATID = item.ITEMID;
                }
            }
            return new CustomsPartnersItemPM()
            {
                Id = item.PRATID,
                ClassificationCode = item.PRATID,
                ItemCode = item.ITEMID,
                CustomerId = "item.CustomerId",
                CustomerName = item.PARTNERID,
                Name = item.DESCRIPTION,
                VendorId = item.PARTNERID,
                //VendorName = crd.NAMEENG,
                Tenant = tenant,
                SearchFields = item.SEARCHENG,


            };
        }

    }
}