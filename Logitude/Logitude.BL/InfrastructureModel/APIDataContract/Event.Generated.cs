
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1; 
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using System.Xml.Serialization;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{
   
    public partial class Event
    {

	    
    public string Id { get; set; }
    
    public EventType EventType { get; set; }
    
    public DateTime LogDateTime { get; set; }
    
    public DateTime EventDateTime { get; set; }
    
    public string Notes { get; set; }
    
    public User CreatedBy { get; set; }
    
    public bool IsAddedManually { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 