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

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class DeclarationStatus
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
     [Key]
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
        [Column("FieldC1")]
	    public bool FieldC1 { get; set; }
        [Column("FieldC2")]
	    public bool FieldC2 { get; set; }
        [Column("FieldC3")]
	    public bool FieldC3 { get; set; }
        [Column("FieldC4")]
	    public bool FieldC4 { get; set; }
        [Column("FieldC5")]
	    public bool FieldC5 { get; set; }
        [Column("FieldC6")]
	    public bool FieldC6 { get; set; }
        [Column("FieldC7")]
	    public bool FieldC7 { get; set; }
        [Column("FieldC8")]
	    public bool FieldC8 { get; set; }
        [Column("FieldC9")]
	    public bool FieldC9 { get; set; }
        [Column("FieldC10")]
	    public bool FieldC10 { get; set; }
        [Column("FieldC11")]
	    public bool FieldC11 { get; set; }
        [Column("FieldC12")]
	    public bool FieldC12 { get; set; }
        [Column("FieldC13")]
	    public bool FieldC13 { get; set; }
        [Column("FieldC14")]
	    public bool FieldC14 { get; set; }
        [Column("FieldC15")]
	    public bool FieldC15 { get; set; }
        [Column("FieldC16")]
	    public bool FieldC16 { get; set; }
        [Column("FieldC17")]
	    public bool FieldC17 { get; set; }
        [Column("FieldC18")]
	    public bool FieldC18 { get; set; }
        [Column("FieldC19")]
	    public bool FieldC19 { get; set; }
        [Column("FieldC20")]
	    public bool FieldC20 { get; set; }
        [Column("FieldC21")]
	    public bool FieldC21 { get; set; }
        [Column("FieldC22")]
	    public bool FieldC22 { get; set; }
        [Column("FieldC23")]
	    public bool FieldC23 { get; set; }
        [Column("FieldC24")]
	    public bool FieldC24 { get; set; }
        [Column("FieldC25")]
	    public bool FieldC25 { get; set; }
        [Column("FieldC26")]
	    public bool FieldC26 { get; set; }
        [Column("FieldC27")]
	    public bool FieldC27 { get; set; }
        [Column("FieldC28")]
	    public bool FieldC28 { get; set; }
        [Column("FieldC29")]
	    public bool FieldC29 { get; set; }
        [Column("FieldC30")]
	    public bool FieldC30 { get; set; }
        [Column("FieldC31")]
	    public bool FieldC31 { get; set; }
        [Column("FieldC32")]
	    public bool FieldC32 { get; set; }
        [Column("FieldC33")]
	    public bool FieldC33 { get; set; }
        [Column("FieldC34")]
	    public bool FieldC34 { get; set; }
        [Column("FieldC35")]
	    public bool FieldC35 { get; set; }
        [Column("FieldC36")]
	    public bool FieldC36 { get; set; }
        [Column("FieldC37")]
	    public bool FieldC37 { get; set; }
        [Column("FieldC38")]
	    public bool FieldC38 { get; set; }
        [Column("FieldC39")]
	    public bool FieldC39 { get; set; }
        [Column("FieldC40")]
	    public bool FieldC40 { get; set; }
        [Column("FieldC41")]
	    public bool FieldC41 { get; set; }
        [Column("FieldC42")]
	    public bool FieldC42 { get; set; }
        [Column("FieldC43")]
	    public bool FieldC43 { get; set; }
        [Column("FieldC44")]
	    public bool FieldC44 { get; set; }
        [Column("FieldC45")]
	    public bool FieldC45 { get; set; }
        [Column("FieldC46")]
	    public bool FieldC46 { get; set; }
        [Column("FieldC47")]
	    public bool FieldC47 { get; set; }
        [Column("FieldC48")]
	    public bool FieldC48 { get; set; }
        [Column("FieldC49")]
	    public bool FieldC49 { get; set; }
        [Column("FieldC50")]
	    public bool FieldC50 { get; set; }
        [Column("FieldD1")]
	    public DateTime? FieldD1 { get; set; }
        [Column("FieldD2")]
	    public DateTime? FieldD2 { get; set; }
        [Column("FieldD3")]
	    public DateTime? FieldD3 { get; set; }
        [Column("FieldD4")]
	    public DateTime? FieldD4 { get; set; }
        [Column("FieldD5")]
	    public DateTime? FieldD5 { get; set; }
        [Column("FieldD6")]
	    public DateTime? FieldD6 { get; set; }
        [Column("FieldD7")]
	    public DateTime? FieldD7 { get; set; }
        [Column("FieldD8")]
	    public DateTime? FieldD8 { get; set; }
        [Column("FieldD9")]
	    public DateTime? FieldD9 { get; set; }
        [Column("FieldD10")]
	    public DateTime? FieldD10 { get; set; }
        [Column("FieldD11")]
	    public DateTime? FieldD11 { get; set; }
        [Column("FieldD12")]
	    public DateTime? FieldD12 { get; set; }
        [Column("FieldD13")]
	    public DateTime? FieldD13 { get; set; }
        [Column("FieldD14")]
	    public DateTime? FieldD14 { get; set; }
        [Column("FieldD15")]
	    public DateTime? FieldD15 { get; set; }
        [Column("FieldD16")]
	    public DateTime? FieldD16 { get; set; }
        [Column("FieldD17")]
	    public DateTime? FieldD17 { get; set; }
        [Column("FieldD18")]
	    public DateTime? FieldD18 { get; set; }
        [Column("FieldD19")]
	    public DateTime? FieldD19 { get; set; }
        [Column("FieldD20")]
	    public DateTime? FieldD20 { get; set; }
        [Column("FieldD21")]
	    public DateTime? FieldD21 { get; set; }
        [Column("FieldD22")]
	    public DateTime? FieldD22 { get; set; }
        [Column("FieldD23")]
	    public DateTime? FieldD23 { get; set; }
        [Column("FieldD24")]
	    public DateTime? FieldD24 { get; set; }
        [Column("FieldD25")]
	    public DateTime? FieldD25 { get; set; }
        [Column("FieldD26")]
	    public DateTime? FieldD26 { get; set; }
        [Column("FieldD27")]
	    public DateTime? FieldD27 { get; set; }
        [Column("FieldD28")]
	    public DateTime? FieldD28 { get; set; }
        [Column("FieldD29")]
	    public DateTime? FieldD29 { get; set; }
        [Column("FieldD30")]
	    public DateTime? FieldD30 { get; set; }
        [Column("FieldD31")]
	    public DateTime? FieldD31 { get; set; }
        [Column("FieldD32")]
	    public DateTime? FieldD32 { get; set; }
        [Column("FieldD33")]
	    public DateTime? FieldD33 { get; set; }
        [Column("FieldD34")]
	    public DateTime? FieldD34 { get; set; }
        [Column("FieldD35")]
	    public DateTime? FieldD35 { get; set; }
        [Column("FieldD36")]
	    public DateTime? FieldD36 { get; set; }
        [Column("FieldD37")]
	    public DateTime? FieldD37 { get; set; }
        [Column("FieldD38")]
	    public DateTime? FieldD38 { get; set; }
        [Column("FieldD39")]
	    public DateTime? FieldD39 { get; set; }
        [Column("FieldD40")]
	    public DateTime? FieldD40 { get; set; }
        [Column("FieldD41")]
	    public DateTime? FieldD41 { get; set; }
        [Column("FieldD42")]
	    public DateTime? FieldD42 { get; set; }
        [Column("FieldD43")]
	    public DateTime? FieldD43 { get; set; }
        [Column("FieldD44")]
	    public DateTime? FieldD44 { get; set; }
        [Column("FieldD45")]
	    public DateTime? FieldD45 { get; set; }
        [Column("FieldD46")]
	    public DateTime? FieldD46 { get; set; }
        [Column("FieldD47")]
	    public DateTime? FieldD47 { get; set; }
        [Column("FieldD48")]
	    public DateTime? FieldD48 { get; set; }
        [Column("FieldD49")]
	    public DateTime? FieldD49 { get; set; }
        [Column("FieldD50")]
	    public DateTime? FieldD50 { get; set; }
        [Column("FieldR1")]
	    public string FieldR1 { get; set; }
        [Column("FieldR2")]
	    public string FieldR2 { get; set; }
        [Column("FieldR3")]
	    public string FieldR3 { get; set; }
        [Column("FieldR4")]
	    public string FieldR4 { get; set; }
        [Column("FieldR5")]
	    public string FieldR5 { get; set; }
        [Column("FieldR6")]
	    public string FieldR6 { get; set; }
        [Column("FieldR7")]
	    public string FieldR7 { get; set; }
        [Column("FieldR8")]
	    public string FieldR8 { get; set; }
        [Column("FieldR9")]
	    public string FieldR9 { get; set; }
        [Column("FieldR10")]
	    public string FieldR10 { get; set; }
        [Column("FieldR11")]
	    public string FieldR11 { get; set; }
        [Column("FieldR12")]
	    public string FieldR12 { get; set; }
        [Column("FieldR13")]
	    public string FieldR13 { get; set; }
        [Column("FieldR14")]
	    public string FieldR14 { get; set; }
        [Column("FieldR15")]
	    public string FieldR15 { get; set; }
        [Column("FieldR16")]
	    public string FieldR16 { get; set; }
        [Column("FieldR17")]
	    public string FieldR17 { get; set; }
        [Column("FieldR18")]
	    public string FieldR18 { get; set; }
        [Column("FieldR19")]
	    public string FieldR19 { get; set; }
        [Column("FieldR20")]
	    public string FieldR20 { get; set; }
        [Column("SVC")]
	    public DateTime? SVC { get; set; }
        [Column("INA")]
	    public DateTime? INA { get; set; }
        [Column("RSG")]
	    public DateTime? RSG { get; set; }
        [Column("RSH")]
	    public DateTime? RSH { get; set; }
    }
}
	 