using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPTemplateTextDesign
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("FontSize")]
	    public double FontSize { get; set; }
        [Column("TextColor")]
	    public string TextColor { get; set; }
        [Column("FontFamily")]
	    public string FontFamily { get; set; }
        [Column("BackgroundColor")]
	    public string BackgroundColor { get; set; }
        [Column("FontWeight")]
	    public string FontWeight { get; set; }
        [Column("Italic")]
	    public bool Italic { get; set; }
        [Column("UnDerLine")]
	    public bool UnDerLine { get; set; }
        [Column("Alignment")]
	    public string Alignment { get; set; }
    }
}
	 