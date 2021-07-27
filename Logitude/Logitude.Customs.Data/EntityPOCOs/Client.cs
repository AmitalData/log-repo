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
   
    public class Client
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Code")]
	    public string Code { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("FullName")]
	    public string FullName { get; set; }
        [ForeignKey("CustomerTypeGeneral")]
        [Column("ClientTypeSpecificCode")]
	    public string ClientTypeSpecificCode { get; set; }
	      
        public virtual CustomerTypeGeneral CustomerTypeGeneral { get; set; }
        [Column("IsActive")]
	    public bool IsActive { get; set; }
        [Column("LocalFirstName")]
	    public string LocalFirstName { get; set; }
        [Column("LocalLastName")]
	    public string LocalLastName { get; set; }
        [Column("LocalCorporationName")]
	    public string LocalCorporationName { get; set; }
        [Column("EnglishFirstName")]
	    public string EnglishFirstName { get; set; }
        [Column("EnglishLastName")]
	    public string EnglishLastName { get; set; }
        [Column("EnglishCorporationName")]
	    public string EnglishCorporationName { get; set; }
        [Column("BirthDate")]
	    public DateTime? BirthDate { get; set; }
        [ForeignKey("Gender")]
        [Column("GenderCode")]
	    public string GenderCode { get; set; }
	      
        public virtual Gender Gender { get; set; }
        [Column("DunsNumber")]
	    public string DunsNumber { get; set; }
        [Column("PassportNumber")]
	    public string PassportNumber { get; set; }
        [ForeignKey("Country")]
        [Column("PassportCountryCode")]
	    public string PassportCountryCode { get; set; }
	      
        public virtual CustomsCountry Country { get; set; }
        [ForeignKey("PassportType")]
        [Column("PassportTypeCode")]
	    public string PassportTypeCode { get; set; }
	      
        public virtual PassportType PassportType { get; set; }
        [Column("PassportFirstName")]
	    public string PassportFirstName { get; set; }
        [Column("PassportLastName")]
	    public string PassportLastName { get; set; }
        [Column("EnglishBirthPlace")]
	    public string EnglishBirthPlace { get; set; }
        [Column("EnglishFatherName")]
	    public string EnglishFatherName { get; set; }
        [Column("PassportExpirationDate")]
	    public DateTime? PassportExpirationDate { get; set; }
        [Column("PassportIssueDate")]
	    public DateTime? PassportIssueDate { get; set; }
        [Column("IsImporter")]
	    public bool IsImporter { get; set; }
        [Column("IsExporter")]
	    public bool IsExporter { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [ForeignKey("FacilitationType")]
        [Column("FacilitationTypeCode")]
	    public string FacilitationTypeCode { get; set; }
	      
        public virtual FacilitationType FacilitationType { get; set; }
        [Column("NationalIdentificationNumber")]
	    public string NationalIdentificationNumber { get; set; }
        [Column("IsExportPoaActive")]
	    public int? IsExportPoaActive { get; set; }
    }
}
	 