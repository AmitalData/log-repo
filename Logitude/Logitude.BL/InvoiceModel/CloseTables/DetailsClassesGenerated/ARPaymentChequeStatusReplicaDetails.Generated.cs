

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.CloseTablesClasses;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs; 
using Simplog.Data.InvoiceModel;

namespace Logitude.BL.InvoiceModel
{
   public class ARPaymentChequeStatusReplicaDetails : ARPaymentChequeStatusReplica, ICloseTable<ARPaymentChequeStatusReplica, ARPaymentChequeStatusReplicaDetails>
   {
       public List<ARPaymentChequeStatusReplicaDetails> GetAll()
       {
		    var all = new List<ARPaymentChequeStatusReplicaDetails>(); 
            return all;
       }

	    public void MapPoco(ARPaymentChequeStatusReplica newPoco)
        {    
        }

		public string GetSearchFields(ARPaymentChequeStatusReplica rec)
        {   
           return string.Empty;
        }
   }
}

