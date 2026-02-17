using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.QuoteModel
{
    public interface IQuotesContext : IContext
    {
        IDbSet<Quote> Quotes { get; }
        IDbSet<QuoteCharge> QuoteCharges { get; }
        IDbSet<QuotePriceSteps> QuotePriceSteps { get; }
        IDbSet<QuoteType> QuoteTypes { get; }
        IDbSet<MarkUpType> MarkUpTypes { get; }
        IDbSet<QuoteCustomerType> QuoteCustomerTypes { get; }
        IDbSet<QuotePackage> QuotePackages { get; }
        IDbSet<QuoteTemplate> QuoteTemplates { get; }
        IDbSet<QuoteTemplateSetting> QuoteTemplateSettings { get; }
        IDbSet<BorderType> BorderTypes { get; }
        IDbSet<QuoteTemplateSectionType> QuoteTemplateSectionTypes { get; }
        IDbSet<QuoteTemplateSection> QuoteTemplateSections { get; }
        IDbSet<QuoteTemplateTextCode> QuoteTemplateTextCodes { get; }
        IDbSet<QuoteClosingReason> QuoteClosingReasons { get; set; }
        IDbSet<QuoteTemplateTextDesign> QuoteTemplateTextDesigns { get; }
        IDbSet<QuoteTemplateTableDesign> QuoteTemplateTableDesigns { get; }
        IDbSet<QuoteDocumentVersion> QuoteDocumentVersions { get; }        
        IDbSet<QuoteTemplateDetailsField> QuoteTemplateDetailsFields { get; }
        IDbSet<QuoteTemplateHeaderField> QuoteTemplateHeaderFields { get; }
        IDbSet<QuoteTemplateExcludedSection> QuoteTemplateExcludedSections { get; }  
        IDbSet<QuoteStage> QuoteStages { get; }
        IDbSet<QuoteRating> QuoteRatings { get; }
        IDbSet<QuoteTemplateSectionModification> QuoteTemplateSectionModifications { get; }
        IDbSet<QuoteTotalVAT> QuoteTotalVATs { get; }
        IDbSet<QuoteSetting> QuoteSettings { get; }

        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}