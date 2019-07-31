using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code 
{
    public class NotificationMobileController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>


        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        public bool PostContactMobileDevice(int tenant, ContactMobileDeviceFilters filters)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ContactMobileDeviceRepository repository = new ContactMobileDeviceRepository();
            ContactMobileDevice item = repository.GetSingleContactMobileDevice(filters.DeviceId);

            if (string.IsNullOrEmpty(filters.AppVersion)) filters.AppVersion = "1.01";
            if (item != null)

            {
                item.NotificationUniqueKey = !string.IsNullOrEmpty(filters.NotificationUniqueKey) ? filters.NotificationUniqueKey : filters.DeviceId;
                item.Email = filters.Email;
                item.Platform = filters.Platform;
                item.UpdateDate = filters.UpdateDate;
                item.Version = filters.Version;
                item.IsSignOut = filters.IsSignOut;
                item.AppVersion = filters.AppVersion;
                repository.Update(item);
                repository.SubmitChanges();
            }
            else
            {
                //CheckedIfExist(filters, repository, item);
                item = new ContactMobileDevice()
                {
                    Email = filters.Email,
                    DeviceId =filters.DeviceId,
                    NotificationUniqueKey = !string.IsNullOrEmpty(filters.NotificationUniqueKey) ? filters.NotificationUniqueKey : filters.DeviceId,
                    Devicetype = filters.Devicetype,
                    CreateDate = filters.CreateDate,
                    Platform = filters.Platform,
                    UpdateDate = filters.UpdateDate,
                    Version = filters.Version,
                   AppVersion = filters.AppVersion,

                };
                repository.Add(item);
                repository.SubmitChanges();
            }

          
            return true;

        }

    //    BudgetNumber

        [OperationContract]
        [WebGet(UriTemplate = "getbudgetnumber/{email}")]
        public int GetBudgetNumber(string email)
        {
           // SecurityUtility.AuthenticationOnTenant(tenant);
          // ContactsUnseenEntitieRepository contactsUnseenRepository = new ContactsUnseenEntitieRepository(tenant);
           // return contactsUnseenRepository.GetContactsUnseenEntitiesCountByContactIdAndTenant(contactId, tenant);
            MobileNotificationLogRepository mobileNotificationLogRepository = new MobileNotificationLogRepository();
            return mobileNotificationLogRepository.GetNotificationCountByEmail(email);
        }


      //  Notifiactions List

        [OperationContract]
        [WebGet(UriTemplate = "getnotifiactionformobile/{tenant}/{email}/{pageIndex}/{pageSize}")]
        public List<NotificationDetails> GetNotifiactionForMobile(int tenant, string email, int pageIndex, int pageSize)
        {
            DateTime DateBeforeGetNotifiactionList = DateTime.Now;

            SecurityUtility.AuthenticationOnTenant(tenant);
            List<NotificationDetails> NotificationList = new List<NotificationDetails>();
            MobileNotificationLogRepository mobileNotificationLogRepository = new MobileNotificationLogRepository();

            IQueryable<MobileNotificationLog> MobileNotificationLogList = (from a in mobileNotificationLogRepository.context.MobileNotificationLogs
                                                                           where a.Email == email && a.IsDelete == false
                                                                           orderby a.CreateDate descending
                                                                           select a).Skip(pageIndex).Take(pageSize);


           foreach(MobileNotificationLog item in MobileNotificationLogList)
           {
               NotificationDetails notificationDetails = new NotificationDetails() { Message = item.NotificationMessage};
               if (!string.IsNullOrEmpty(item.XML))  
               {
                    notificationDetails = LogitudeXmlSerializer.DeserializeObject<NotificationDetails>(item.XML);
                    notificationDetails.CreateData = item.CreateDate;
                    notificationDetails.IsRead = item.IsRead;
               }
               NotificationList.Add(notificationDetails);
           }

            


            //if (MobileNotificationLogList.Count() > 0)
            //{
            //    List<ShipmentList> result = new List<ShipmentList>();
               
            //    IEnumerable<IGrouping<int, MobileNotificationLog>> NotificationLogListGrouping = MobileNotificationLogList.GroupBy(q => q.Tenant);

            //    foreach (IGrouping<int, MobileNotificationLog> notificationLogList in NotificationLogListGrouping)
            //    {

            //        ShipmentRepository shipmentRepository = new ShipmentRepository(notificationLogList.Key);

            //        int notificationTenant = notificationLogList.Key;
            //        List<string> trackedIds = (from a in notificationLogList
            //                                   where a.Email.ToLower() == email.ToLower() && a.IsDelete == false
            //                                   select a.EntityId).ToList();

            //        IQueryable<ShipmentList> shipments = GetShipmentList(notificationTenant, shipmentRepository, trackedIds);
            //        result.AddRange(shipments);

            //    }

            //    if (result.Count() > 0)
            //    {
            //        foreach (MobileNotificationLog item in MobileNotificationLogList)
            //        {
            //            NotificationDetails notification = new NotificationDetails()
            //            {
            //                Message = item.NotificationMessage,
            //                IsException = item.IsException,
            //                CreateData = item.CreateDate,
            //                Tenant = item.Tenant,
            //                IsRead = item.IsRead,
            //                EntitiyId = item.EntityId,
            //                NotificationId = item.Id,
                            
            //            };

            //            string messagebody = notification.Message.Replace("\\n", "\n");

            //            if (notification.Message.Contains(":")) notification.IsNewDesignNotification = true;
                   
            //            else notification.IsNewDesignNotification = false;
                        
            //            string[] message = messagebody.Split('\n');


            //           notification =  BuildMessageNotification(notification, message);

            //        ShipmentList shipmentList = result.Where(d => d.Id == item.EntityId).FirstOrDefault();
            //         if (shipmentList != null)
            //              {
            //                notification.TransportModeId = shipmentList.TransportModeId;
            //                notification.DirectionId = shipmentList.DirectionId;
            //                notification.ForeignPartnerCountryCode = shipmentList.ForeignPartnerCountryCode; 
            //                notification.FromCountryCode = shipmentList.ForeignPartnerCountryCode;
            //                notification.ToCountryCode = shipmentList.ForeignPartnerCountryCode; 
            //              }

            //            NotificationList.Add(notification);
            //        }

            //    }

            //    if (NotificationList != null)
            //    {
            //        NotificationList = NotificationList.OrderByDescending(d => d.CreateData).ToList();
            //    }
            //}

            int executionTime = (int)((DateTime.Now.Ticks - DateBeforeGetNotifiactionList.Ticks) / TimeSpan.TicksPerMillisecond);
            //  HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());
            if (HttpContext.Current.Response.Headers["ServerTime"] != null)
            {
                HttpContext.Current.Response.Headers["ServerTime"] = executionTime.ToString();
            }
            else HttpContext.Current.Response.Headers.Add("ServerTime", executionTime.ToString());

            return NotificationList;

        }

       // MakeNotificationRead

        [OperationContract]
        [WebGet(UriTemplate = "getmakenotificationread/{email}/{isAll}/{notificationId}/{type}")]
        public bool GetMakeNotificationRead(string email, bool isAll, string notificationId, string type)
        {
            bool IsScuss = false;
            string dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);

            if (notificationId == null)
            {
                notificationId = "";
            }
            using (SqlConnection cn = new SqlConnection(connection.ConnectionString))
            {
                SqlParameter ptype = new SqlParameter("@pType", SqlDbType.VarChar, 10);
                SqlParameter pemail = new SqlParameter("@pEmail", SqlDbType.VarChar, 70);
                SqlParameter pnotificationId = new SqlParameter("@pNotificationId", SqlDbType.VarChar, 50);
                SqlParameter pisAll = new SqlParameter("@pIsAll", SqlDbType.Bit);


                ptype.Direction = ParameterDirection.Input;
                pemail.Direction = ParameterDirection.Input;
                pnotificationId.Direction = ParameterDirection.Input;
                pisAll.Direction = ParameterDirection.Input;


                ptype.Value = type;
                pemail.Value = email;
                pnotificationId.Value = notificationId;
                pisAll.Value = isAll;


                SqlCommand cmd = new SqlCommand("usp_UpdateMobileNotificationLogsMarkReadOrDelete", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(pemail);
                cmd.Parameters.Add(ptype);
                cmd.Parameters.Add(pnotificationId);
                cmd.Parameters.Add(pisAll);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                IsScuss = true;

            }

            return IsScuss;
        }

        private void CheckedIfExist(ContactMobileDeviceFilters filters, ContactMobileDeviceRepository repository, ContactMobileDevice item)
        {
            if (filters.Platform == "Android")
            {
                item = repository.GetSingleContactMobileDeviceByNotificationUniqueKey(filters.NotificationUniqueKey);
                if (item != null) repository.Remove(item);
            }
        }

        private static IQueryable<ShipmentList> GetShipmentList(int tenant, ShipmentRepository shipmentRepository, List<string> trackedIds)
        {

            IQueryable<ShipmentList> shipments = from entity in shipmentRepository.context.Shipments

                                                 where entity.Tenant == tenant && trackedIds.Contains(entity.Id)
                                                 select new ShipmentList()
                                                 {
                                                     Id = entity.Id,
                                                     DirectionId = entity.DirectionId,
                                                     TransportModeId = entity.TransportModeId,
                                                     ForeignPartnerCountryCode = entity.ForeignPartnerCountryCode,   
                                                 };
            return shipments;
        }

        //private NotificationDetails BuildMessageNotification(NotificationDetails notification, string[] message)
        //{
        //    if (message.Count() > 0)
        //    {
        //        if (message[0].Split(':').Count() > 0) notification.Row0Colum0 = message[0].Split(':')[0];
        //        if (message[0].Split(':').Count() > 1) notification.Row0Colum1 = message[0].Split(':')[1];
        //    }
        //    if (message.Count() > 1)
        //    {
        //        if (message[1].Split(':').Count() > 0) notification.Row1Colum0 = message[1].Split(':')[0];
        //        if (message[1].Split(':').Count() > 1) notification.Row1Colum1 = message[1].Split(':')[1];

        //    }
        //    if (message.Count() > 2)
        //    {
        //        if (message[2].Split(':').Count() > 0) notification.Row2Colum0 = message[2].Split(':')[0];
        //        if (message[2].Split(':').Count() > 1) notification.Row2Colum1 = message[2].Split(':')[1];

        //    }
        //    if (message.Count() > 3)
        //    {
        //        if (message[3].Split(':').Count() > 0) notification.Row3Colum0 = message[3].Split(':')[0];
        //        if (message[3].Split(':').Count() > 1) notification.Row3Colum1 = message[3].Split(':')[1];

        //    }

        //    return notification;
        //}

    }
}