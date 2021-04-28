using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.QuoteModel.Mocks
{
    public class MockQuoteContext : IQuotesContext
    {
        List<MarkUpType> markUpTypes;
        MockObjectSet<MarkUpType> markUpObjectSet;
        public IDbSet<MarkUpType> MarkUpTypes
        {
            get
            {
                if (markUpTypes == null)
                {
                    markUpTypes = new List<MarkUpType>() {
                        new MarkUpType() { Code="MK" },
                        new MarkUpType() { Code="QE" } };
                    markUpObjectSet = new MockObjectSet<MarkUpType>(markUpTypes);
                }
                return markUpObjectSet;

            }
        }

        List<QuoteCharge> quoteCharge;
        MockObjectSet<QuoteCharge> quoteChargeObjectSet;
        public IDbSet<QuoteCharge> QuoteCharges
        {
            get
            {
                if (quoteCharge == null)
                {
                    quoteCharge = new List<QuoteCharge>() {
                        new QuoteCharge() {  },
                        new QuoteCharge() { } };
                    quoteChargeObjectSet = new MockObjectSet<QuoteCharge>(quoteCharge);
                }
                return quoteChargeObjectSet;

            }
        }

        List<Quote> qoutes;
        MockObjectSet<Quote> qouteObjectSet;
        public IDbSet<Quote> Quotes
        {
            get
            {
                if (qoutes == null)
                {
                    MockWebFreightContext webFreightMockContext = new MockWebFreightContext();

                    qoutes = new List<Quote>() {
                        new Quote() { Id= "1-1" , Tenant=1, QuoteTypeCode="QT", QuoteType=QuoteTypes.Where(d=>d.Code=="QT").FirstOrDefault(), ToPort=webFreightMockContext.Ports.Where(d=>d.Id=="1-LY").FirstOrDefault() , ToPortId="1-LY" , FromPortId="1-LY" , FromPort=webFreightMockContext.Ports.Where(d=>d.Id=="1-LY").FirstOrDefault() , DirectionId="111" , Direction=webFreightMockContext.Directions.Where(d=>d.Id=="111").FirstOrDefault() , TransportModeId="111" , TransportMode=webFreightMockContext.TransportModes.Where(d=>d.Id=="111").FirstOrDefault() , Stage = QuoteStages.Where(d=>d.Id=="111").FirstOrDefault() , StageId="111"  },
                        new Quote() { Id= "1-2" , Tenant=2} };
                    qouteObjectSet = new MockObjectSet<Quote>(qoutes);
                }
                return qouteObjectSet;

            }
        }

        List<QuoteCustomerType> quoteCustomerTypes;
        MockObjectSet<QuoteCustomerType> quoteCustomerTypeObjectSet;
        public IDbSet<QuoteCustomerType> QuoteCustomerTypes
        {
            get
            {
                if (quoteCustomerTypes == null)
                {
                    quoteCustomerTypes = new List<QuoteCustomerType>() {
                        new QuoteCustomerType() { Code="QC" },
                        new QuoteCustomerType() { Code="ER"} };
                    quoteCustomerTypeObjectSet = new MockObjectSet<QuoteCustomerType>(quoteCustomerTypes);
                }
                return quoteCustomerTypeObjectSet;

            }
        }

        List<QuotePriceSteps> quotePriceSteps;

        public IDbSet<QuotePriceSteps> QuotePriceSteps
        {
            get
            {
                if (quotePriceSteps == null)
                {
                    quotePriceSteps = new List<QuotePriceSteps>() {
                        new QuotePriceSteps() {   },
                        new QuotePriceSteps() { } };
                }
                return new MockObjectSet<QuotePriceSteps>(quotePriceSteps);

            }
        }

        List<QuoteType> quoteTypes;
        MockObjectSet<QuoteType> QuoteTypeObjectSet;
        public IDbSet<QuoteType> QuoteTypes
        {
            get
            {
                if (quoteTypes == null)
                {
                    quoteTypes = new List<QuoteType>() {
                        new QuoteType() { Code="QT"   },
                        new QuoteType() { Code = "QS"  } };

                    QuoteTypeObjectSet = new MockObjectSet<QuoteType>(quoteTypes);
                }
                return QuoteTypeObjectSet;

            }
        }

        List<QuotePackage> quotePackages;
        MockObjectSet<QuotePackage> quotePackageObjectSet;
        public IDbSet<QuotePackage> QuotePackages
        {
            get
            {
                if (quotePackages == null)
                {
                    quotePackages = new List<QuotePackage>() {
                        new QuotePackage() {  },
                        new QuotePackage() { } };
                    quotePackageObjectSet = new MockObjectSet<QuotePackage>(quotePackages);
                }
                return quotePackageObjectSet;

            }
        }

        public void SetAsModified(object entity)
        {
            throw new NotImplementedException();
        }

        public void DetectChanges()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 1;
        }

        public IDbSet<Simplog.Data.InfrastructureModel.EntityPOCOs.FollowUp> FollowUps
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplate> QuoteTemplates
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateSetting> QuoteTemplateSettings
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateSectionType> QuoteTemplateSectionTypes
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<BorderType> BorderTypes
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateTextDesign> QuoteTemplateTextDesigns
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateTableDesign> QuoteTemplateTableDesigns
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateSection> QuoteTemplateSections
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateTextCode> QuoteTemplateTextCodes
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateDetailsField> QuoteTemplateDetailsFields
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateHeaderField> QuoteTemplateHeaderFields
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteDocumentVersion> QuoteDocumentVersions
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTemplateExcludedSection> QuoteTemplateExcludedSections
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteClosingReason> QuoteClosingReasons
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<QuoteStage> QuoteStages
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<QuoteRating> QuoteRatings
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<QuoteTemplateSectionModification> QuoteTemplateSectionModifications
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteTotalVAT> QuoteTotalVATs
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<QuoteSetting> QuoteSettings
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<QuoteComputedField> QuoteComputedField
        {
            get { throw new NotImplementedException(); }
        }

        public System.Data.Common.DbConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        public DbContext GetActiveDbContext()
        {
            throw new NotImplementedException();
        }


    }
}
