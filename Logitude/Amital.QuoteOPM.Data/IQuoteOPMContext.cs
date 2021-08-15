using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Amital.QuoteOPM.Data.EntityPOCOs;

namespace Amital.QuoteOPM.Data
{

    public partial interface IQuoteOPMContext : IContext
    {
   
       	 IDbSet<BorderOPType> BorderOPTypes { get; }
		 IDbSet<MarkUpOPType> MarkUpOPTypes { get; }
		 IDbSet<OPSpecialServicesType> OPSpecialServicesTypes { get; }
		 IDbSet<QuoteOP> QuoteOPs { get; }
		 IDbSet<QuoteOPCharge> QuoteOPCharges { get; }
		 IDbSet<QuoteOPClosingReason> QuoteOPClosingReasons { get; }
		 IDbSet<QuoteOPComputedField> QuoteOPComputedFields { get; }
		 IDbSet<QuoteOPCustomerType> QuoteOPCustomerTypes { get; }
		 IDbSet<QuoteOPPackage> QuoteOPPackages { get; }
		 IDbSet<QuoteOPPriceSteps> QuoteOPPriceSteps { get; }
		 IDbSet<QuoteOPRating> QuoteOPRatings { get; }
		 IDbSet<QuoteOPSetting> QuoteOPSettings { get; }
		 IDbSet<QuoteOPStage> QuoteOPStages { get; }
		 IDbSet<QuoteOPTemplate> QuoteOPTemplates { get; }
		 IDbSet<QuoteOPTemplateSection> QuoteOPTemplateSections { get; }
		 IDbSet<QuoteOPTemplateSectionType> QuoteOPTemplateSectionTypes { get; }
		 IDbSet<QuoteOPTemplateSetting> QuoteOPTemplateSettings { get; }
		 IDbSet<QuoteOPTemplateTableDesign> QuoteOPTemplateTableDesigns { get; }
		 IDbSet<QuoteOPTemplateTextCode> QuoteOPTemplateTextCodes { get; }
		 IDbSet<QuoteOPTemplateTextDesign> QuoteOPTemplateTextDesigns { get; }
		 IDbSet<QuoteOPTotalVAT> QuoteOPTotalVATs { get; }
		 IDbSet<QuoteOPType> QuoteOPTypes { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}