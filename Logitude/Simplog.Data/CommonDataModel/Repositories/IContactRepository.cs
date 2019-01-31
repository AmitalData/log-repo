using System;
namespace Simplog.Data.CommonDataModel.Repositories
{
   public interface IContactRepository
    {
        void Add(Simplog.Data.CommonDataModel.EntityPOCOs.Contact entity);
        System.Collections.Generic.List<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> All();
        bool CheckEmailAvailabilityForTenant(string email, int tenant);
        bool CheckEmailAvailabilityForTenant0(string email);
        Simplog.Data.CommonDataModel.ICommonDataContext context { get; }
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetActiveContacts(int tenant);
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetAllContacts(int tenant);
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetAllContactsThatHaveSignature();
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.CardContact> GetCardContactsForTenant(int tenant);
        string GetConactEmail(string id);
        string GetConactIdByemail(string email, int tenant);
        string GetConactNameByemail(string email, int tenant);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetContactByUserTypeAndTenant(string userType, int tenant);
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetContactListsForShipmentFollow(System.Collections.Generic.List<string> trackedIds, int tenant);
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetContacts(System.Collections.Generic.List<string> allContactsId, int tenant);
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetContacts(int tenant);
        System.Linq.IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetContactsByEmail(string email);
        System.Collections.Generic.List<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetContactsByEmailAndTenant(string email);
        System.Collections.Generic.List<Simplog.Data.Helpers.EmailTypeClass> GetContactsListByEmailAndTenant(System.Collections.Generic.List<Simplog.Data.Helpers.EmailTypeClass> emails, int tenant);
        string GetEmailContactByIdAndTenant(int tenant, string id);
        System.Collections.Generic.List<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingleContact(string id, int tenant);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingleContactByEmail(string email, int tenant, bool getFromCache = false);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingleContactByEmailAndTenant(string email, int tenant);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingleContactByEmailSpecificTenant(string email, int tenant);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingleContactByExternalId(string externalId, int tenant);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingleContactByIdAndTenant(string id, int tenant, bool getFromCache);
        Simplog.Data.CommonDataModel.EntityPOCOs.Contact GetSingleContactForUpdate(string id, int tenant);
        bool IsContactByEmailExists(string email, int tenant);
        void Remove(Simplog.Data.CommonDataModel.EntityPOCOs.Contact entity);
        void SubmitChanges();
        void Update(Simplog.Data.CommonDataModel.EntityPOCOs.Contact entity);
    }
}
