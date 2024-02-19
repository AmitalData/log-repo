using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code
{
    public class ContactsUnseenEntitiesController : ApiController
    {
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5

        //public bool PostDeleteContactsUnseenEntitie(int tenant, ContactsUnseenEntitieFilter filters)
        //{
        //    bool issuccess;
        //    ObjectTabelRepository rep = new ObjectTabelRepository(tenant);
        //    ObjectTable table = rep.GetObjectTableByName(filters.ObjectTableName, 0, false);

        //    ContactsUnseenEntitieRepository contactsUnseenRepository = new ContactsUnseenEntitieRepository(tenant);

        //    List<ContactsUnseenEntitie> ContactsUnseenEntitieList = contactsUnseenRepository.GetContactsUnseenEntitiesByContactAndObjectTable(filters.ContactId, table.Id, tenant);

        //    if (ContactsUnseenEntitieList != null)
        //    {
        //        foreach (ContactsUnseenEntitie item in ContactsUnseenEntitieList)
        //        {
        //            contactsUnseenRepository.Remove(item);

        //        }
        //        issuccess = true;connection
        //    }
        //    else
        //    {
        //        issuccess = false;

        //    }

        //    contactsUnseenRepository.SubmitChanges();

        //    return issuccess;

        //}


        //public bool PostDeleteContactsUnseenEntitie(int tenant, ContactsUnseenEntitieFilter filters)
        //{
        //    bool IsScuss = false;
        //    ObjectTabelRepository rep = new ObjectTabelRepository(tenant);
        //    ObjectTable table = rep.GetObjectTableByName(filters.ObjectTableName, 0, false);
        //  //string dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
        //  //DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);

        //    string strConnString = GetConnection(tenant);// ConfigurationManager.ConnectionStrings["str"].ConnectionString;
        //  // string strConnString = connection.ConnectionString;
        //  using (SqlConnection cn = new SqlConnection(strConnString))
        //  {

        //      SqlParameter pcontactId = new SqlParameter("@pContactId", SqlDbType.VarChar, 15);
        //      SqlParameter pobjectTableId = new SqlParameter("@pObjectTableId", SqlDbType.VarChar, 15);
        //      SqlParameter ptenant = new SqlParameter("@pTenant", SqlDbType.Int);


        //      pcontactId.Direction = ParameterDirection.Input;
        //      pobjectTableId.Direction = ParameterDirection.Input;
        //      ptenant.Direction = ParameterDirection.Input;

        //      pcontactId.Value = filters.ContactId;
        //      pobjectTableId.Value = table.Id;
        //      ptenant.Value = tenant;

        //      SqlCommand cmd = new SqlCommand("usp_DeleteContactsUnseenEntitiesByContactId", cn);
        //      cmd.CommandType = CommandType.StoredProcedure;


        //      cmd.Parameters.Add(pcontactId);
        //      cmd.Parameters.Add(pobjectTableId);
        //      cmd.Parameters.Add(ptenant);
        //      //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
        //      //{
        //          cn.Open();
        //          cmd.ExecuteNonQuery();
        //          cn.Close();
        //          //scope.Complete();
        //      //}
        //          IsScuss =  true;

        //  }

        //  return IsScuss;
        //}


        public string ObjectTableName { get; set; }
        public string ContactId { get; set; }
        public bool GetDeleteContactsUnseenEntitie(int tenant, string contactId, string objectTableName)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            bool IsScuss = false;
            ObjectTableRepository rep = new ObjectTableRepository(tenant);
            ObjectTable table = rep.GetObjectTableByName(objectTableName, 0, false);
    
            string strConnString = GetConnection(tenant);// ConfigurationManager.ConnectionStrings["str"].ConnectionString;
            
            using (SqlConnection cn = new SqlConnection(strConnString))
            {

                SqlParameter pcontactId = new SqlParameter("@pContactId", SqlDbType.VarChar, 15);
                SqlParameter pobjectTableId = new SqlParameter("@pObjectTableId", SqlDbType.VarChar, 15);
                SqlParameter ptenant = new SqlParameter("@pTenant", SqlDbType.Int);


                pcontactId.Direction = ParameterDirection.Input;
                pobjectTableId.Direction = ParameterDirection.Input;
                ptenant.Direction = ParameterDirection.Input;

                pcontactId.Value = contactId;
                pobjectTableId.Value = table.Id;
                ptenant.Value = tenant; 

                SqlCommand cmd = new SqlCommand("usp_DeleteContactsUnseenEntitiesByContactId", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(pcontactId);
                cmd.Parameters.Add(pobjectTableId);
                cmd.Parameters.Add(ptenant);
                //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                //{
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                //scope.Complete();
                //}
                IsScuss = true;
                
            }

            return IsScuss;
        }


        public static string GetConnection(int tenant)
        {
           
            GlobalDB currentDb;
            currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
            string dbConnectionInfo = currentDb.DBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }

    }
}