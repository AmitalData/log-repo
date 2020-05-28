
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

namespace Logitude.BL.QuoteModel.APIDataContract.ApiV1
{
   
    public partial class QuoteCharge
    {

	    
	[XmlAttribute]
    public string Id { get; set; }
    
    public ChargesType ChargesType { get; set; }
    
    public Currency CostCurrency { get; set; }
    
    public double? CostExchangeRate { get; set; }
    
    public bool CostIsFixedRate { get; set; }
    
    public Measurement CostMeasurement { get; set; }
    
    public double? CostQuantity { get; set; }
    
    public double? CostTotalAmount { get; set; }
    
    public double? CostUnitPrice { get; set; }
    
    public bool IsAllIN { get; set; }
    
    public double? SaleExchangeRate { get; set; }
    
    public bool SaleIsFixedRate { get; set; }
    
    public Measurement SaleMeasurement { get; set; }
    
    public double? SaleQuantity { get; set; }
    
    public double? SaleUnitPrice { get; set; }
    
    public double? SaleTotalAmount { get; set; }
    
    public double? CostContainerType1UnitPrice { get; set; }
    
    public double? SaleContainerType1UnitPrice { get; set; }
    
    public double? CostContainerType2UnitPrice { get; set; }
    
    public double? SaleContainerType2UnitPrice { get; set; }
    
    public double? CostContainerType3UnitPrice { get; set; }
    
    public double? SaleContainerType3UnitPrice { get; set; }
    
    public double? CostContainerType4UnitPrice { get; set; }
    
    public double? SaleContainerType4UnitPrice { get; set; }
    
    public double? CostContainerType5UnitPrice { get; set; }
    
    public double? SaleContainerType5UnitPrice { get; set; }
    
    public double? SaleMaxAmount { get; set; }
    
    public double? SaleMinAmount { get; set; }
    
    public List<QuotePriceSteps> PriceBreaks { get; set; }

    public  string  ComputingPartnerCode { get; set; }

    }
} 