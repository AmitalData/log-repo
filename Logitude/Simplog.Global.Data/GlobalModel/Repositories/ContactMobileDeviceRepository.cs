using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class ContactMobileDeviceRepository : IRepository<ContactMobileDevice>
    {
        IGlobalContext globalContext;
        public ContactMobileDeviceRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public ContactMobileDeviceRepository(IGlobalContext context)
        {
            globalContext = context;
        }



        public ContactMobileDevice GetSingleContactMobileDevice(string deviceId)
        {
            ContactMobileDevice item = context.ContactMobileDevices.Where(d =>  d.DeviceId == deviceId).FirstOrDefault();

            return item;
        }

        public ContactMobileDevice GetSingleContactMobileDeviceByNotificationUniqueKey(string notificationUniqueKey)
        {
            ContactMobileDevice item = context.ContactMobileDevices.Where(d => d.NotificationUniqueKey == notificationUniqueKey).FirstOrDefault();

            return item;
        }


        public IQueryable<ContactMobileDevice> GetAllContactMobileDevices()
        {
            return from a in context.ContactMobileDevices
                   select a;
        }


        public List<ContactMobileDevice> GetAllDeviceByEmail(string email)
        {
            return (from a in context.ContactMobileDevices
                    where a.Email == email && !a.IsSignOut
                   select a).ToList();
        }

        public void Add(ContactMobileDevice entity)
        {
            context.ContactMobileDevices.Add(entity);
        }

        public void Remove(ContactMobileDevice entity)
        {
            context.ContactMobileDevices.Attach(entity);
            context.ContactMobileDevices.Remove(entity);
        }

        public void Update(ContactMobileDevice entity)
        {
            context.ContactMobileDevices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContactMobileDevice> All()
        {
            return context.ContactMobileDevices.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ContactMobileDevice> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ContactMobileDevice GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }



        public IQueryable<ContactMobileDevice> GetAllContactMobileDevicesByEmails(List<string> emails)
        {

            return (from a in context.ContactMobileDevices
                    where emails.Contains(a.Email) && !a.IsSignOut
                    select a);

           
        }
    }
}