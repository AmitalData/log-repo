using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.;
using Logitude.BL.QuoteModel.APIDataContract.;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.;
using Logitude.BL.ShipmentsModel.APIDataContract.;
using Logitude.BL.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;

 namespace Logitude.Accounting.BL.APIDataContract.
{ 
   public partial class JournalQueryService
   {
   
		IAccountingContext  context;
		//JournalService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.JournalQueryService query; 

        public JournalQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new JournalService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.JournalQueryService(tenant);
        }

		
		public Journal GetJournalById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Journal with Id " + Id + " doesn't exist");

				return JournalDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Journal JournalDataMapping(JournalPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Journal(); 