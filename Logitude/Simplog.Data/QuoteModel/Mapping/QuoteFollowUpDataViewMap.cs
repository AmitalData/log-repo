using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteFollowUpDataViewMap : EntityTypeConfiguration<QuoteFollowUpDataView>
    {
        public QuoteFollowUpDataViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Tenant, t.QuoteNumber, t.OpenDate, t.IsClosed, t.LastModified, t.CreatedByUserId, t.DirectionId, t.TransportModeId, t.IsDangerous, t.StageId, t.IsByKG, t.IsByContainer, t.QuoteTypeCode, t.EstimateProfitEdited, t.IsFreightBySteps, t.IsCancelled, t.QuoteCustomerTypeCode, t.CustomerName, t.SaleCurrencyId, t.ExchangeRate, t.IsFixedPrice, t.ConcurrencyGUID, t.FollowUpId, t.FollowUpOwnerUserId });

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(15);

            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.QuoteNumber).IsRequired().HasMaxLength(15);

            this.Property(t => t.ShipperReference1).HasMaxLength(50);

            this.Property(t => t.ShipperReference2).HasMaxLength(50);
            this.Property(t => t.ConsigneeReference1).HasMaxLength(50);

            this.Property(t => t.ConsigneeReference2).HasMaxLength(50);

            this.Property(t => t.Notes).HasMaxLength(250);

            this.Property(t => t.DescriptionOfGoods)
                .HasMaxLength(512);

            this.Property(t => t.LastModified)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            this.Property(t => t.Field1)
                .HasMaxLength(250);

            this.Property(t => t.Field2)
                .HasMaxLength(250);

            this.Property(t => t.Field3)
                .HasMaxLength(250);

            this.Property(t => t.Field4)
                .HasMaxLength(250);

            this.Property(t => t.Field5)
                .HasMaxLength(250);

            this.Property(t => t.Field6)
                .HasMaxLength(250);

            this.Property(t => t.Field7)
                .HasMaxLength(250);

            this.Property(t => t.Field8)
                .HasMaxLength(250);

            this.Property(t => t.Field9)
                .HasMaxLength(250);

            this.Property(t => t.Field10)
                .HasMaxLength(250);

            this.Property(t => t.DimensionsUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.GrossWeightUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.VolumeUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.ShipmentTypeId)
                .HasMaxLength(4);

            this.Property(t => t.ShipperId)
                .HasMaxLength(15);

            this.Property(t => t.ConsigneeId)
                .HasMaxLength(15);

            this.Property(t => t.ShipperContactId)
                .HasMaxLength(15);

            this.Property(t => t.ConsigneeContactId)
                .HasMaxLength(15);

            this.Property(t => t.FromPortId)
                .HasMaxLength(15);

            this.Property(t => t.ToPortId)
                .HasMaxLength(15);

            this.Property(t => t.IncotermId)
                .HasMaxLength(15);

            this.Property(t => t.SalesmanUserId)
                .HasMaxLength(15);

            this.Property(t => t.CreatedByUserId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.UpdatedByUserId)
              .IsRequired()
              .HasMaxLength(15);

            this.Property(t => t.DirectionId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.TransportModeId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

           
            this.Property(t => t.BranchId)
                .HasMaxLength(15);

            this.Property(t => t.DepartmentId)
                .HasMaxLength(15);

            this.Property(t => t.PackageType1Id)
                .HasMaxLength(15);

            this.Property(t => t.PackageType2Id)
                .HasMaxLength(15);

            this.Property(t => t.PackageType3Id)
                .HasMaxLength(15);

            this.Property(t => t.PackageType4Id)
                .HasMaxLength(15);

            this.Property(t => t.PackageType5Id)
                .HasMaxLength(15);

            this.Property(t => t.QuoteTypeCode)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.MainCarriageCarrierId)
                .HasMaxLength(15);

            this.Property(t => t.CustomerId)
                .HasMaxLength(15);

            this.Property(t => t.CustomerContactId)
                .HasMaxLength(15);

            this.Property(t => t.CustomerReference1).HasMaxLength(50);
            this.Property(t => t.CustomerReference2).HasMaxLength(50);

            this.Property(t => t.QuoteCustomerTypeCode)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SaleCurrencyId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.ShipperName)
                .HasMaxLength(100);

            this.Property(t => t.ConsigneeName)
                .HasMaxLength(100);

            this.Property(t => t.PickUpAddress)
                .HasMaxLength(250);

            this.Property(t => t.DeliveryAddress)
                .HasMaxLength(250);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000);

            this.Property(t => t.ChargeableWeightUnitCode)
                .HasMaxLength(3);

            this.Property(t => t.ConcurrencyGUID)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.FromPartnerId)
                .HasMaxLength(15);

            this.Property(t => t.ToPartnerId)
                .HasMaxLength(15);

            this.Property(t => t.FromPartnerAddressId)
                .HasMaxLength(15);

            this.Property(t => t.ToPartnerAddressId)
                .HasMaxLength(15);

            this.Property(t => t.FollowUpId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.FollowUpNotes)
                .HasMaxLength(250);

            this.Property(t => t.FollowUpOwnerUserId)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Shipper)
                .HasMaxLength(60);

            this.Property(t => t.Consignee)
                .HasMaxLength(60);

            this.Property(t => t.MainCarriageCarrierName)
              .HasMaxLength(60);

            this.Property(t => t.FollowUpType)
                .HasMaxLength(40);

            this.Property(t => t.FromPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.FromPortName)
                .HasMaxLength(40);

            this.Property(t => t.FromPortCountry)
                .HasMaxLength(120);

            this.Property(t => t.ToPortCode)
                .IsFixedLength()
                .HasMaxLength(3);

            this.Property(t => t.ToPortName)
                .HasMaxLength(40);

            this.Property(t => t.ToPortCountry)
                .HasMaxLength(120);

            this.Property(t => t.StageName)
                .HasMaxLength(40);

            this.Property(t => t.CreatedByUser)
                .HasMaxLength(40);

            this.Property(t => t.UpdatedByUser)
               .HasMaxLength(40);

            this.Property(t => t.FollowUpOwner)
                .HasMaxLength(40);

            this.Property(t => t.QuoteTypeName)
                .HasMaxLength(40);

            this.Property(t => t.ShipmentTypeName)
               .HasMaxLength(40);

            this.Property(t => t.FollowUpTypeId).HasMaxLength(15);
            this.Property(t => t.FollowUpOwnerId).HasMaxLength(15);
            this.Property(t => t.Subject).HasMaxLength(60).IsUnicode(false).IsUnicode(true);
            this.Property(d => d.StageId).HasMaxLength(15).IsUnicode(false);
            this.Property(d => d.RatingCode).HasMaxLength(1).IsUnicode(false);
            this.Property(d => d.LastActivityTypeCode).HasMaxLength(2).IsUnicode(false);
            this.Property(d => d.NextActivityTypeCode).HasMaxLength(2).IsUnicode(false);
            this.Property(d => d.LastActivitySubject).HasMaxLength(255).IsUnicode(true);
            this.Property(d => d.NextActivitySubject).HasMaxLength(255).IsUnicode(true);
            this.Property(t => t.OpportunityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProductCode).HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.IncotermCode).HasMaxLength(3).IsUnicode(false);

            this.Ignore(t => t.QuoteFollowUpId);
            // Table & Column Mappings
            this.ToTable("QuoteFollowUpDataView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.QuoteNumber).HasColumnName("QuoteNumber");
            this.Property(t => t.ShipperReference1).HasColumnName("ShipperReference1");
            this.Property(t => t.ShipperReference2).HasColumnName("ShipperReference2");
            this.Property(t => t.ConsigneeReference1).HasColumnName("ConsigneeReference1");
            this.Property(t => t.ConsigneeReference2).HasColumnName("ConsigneeReference2");
            this.Property(t => t.OpenDate).HasColumnName("OpenDate");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods");
            this.Property(t => t.IsClosed).HasColumnName("IsClosed");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.LastModified).HasColumnName("LastModified");
            this.Property(t => t.Field1).HasColumnName("Field1");
            this.Property(t => t.Field2).HasColumnName("Field2");
            this.Property(t => t.Field3).HasColumnName("Field3");
            this.Property(t => t.Field4).HasColumnName("Field4");
            this.Property(t => t.Field5).HasColumnName("Field5");
            this.Property(t => t.Field6).HasColumnName("Field6");
            this.Property(t => t.Field7).HasColumnName("Field7");
            this.Property(t => t.Field8).HasColumnName("Field8");
            this.Property(t => t.Field9).HasColumnName("Field9");
            this.Property(t => t.Field10).HasColumnName("Field10");
            this.Property(t => t.DimensionsUnitCode).HasColumnName("DimensionsUnitCode");
            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.NumberOfContainers).HasColumnName("NumberOfContainers");
            this.Property(t => t.NumberOfPackages).HasColumnName("NumberOfPackages");
            this.Property(t => t.Ratio).HasColumnName("Ratio");
            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode");
            this.Property(t => t.ShipmentTypeId).HasColumnName("ShipmentTypeId");
            this.Property(t => t.ShipperId).HasColumnName("ShipperId");
            this.Property(t => t.ConsigneeId).HasColumnName("ConsigneeId");
            this.Property(t => t.ShipperContactId).HasColumnName("ShipperContactId");
            this.Property(t => t.ConsigneeContactId).HasColumnName("ConsigneeContactId");
            this.Property(t => t.FromPortId).HasColumnName("FromPortId");
            this.Property(t => t.ToPortId).HasColumnName("ToPortId");
            this.Property(t => t.IncotermId).HasColumnName("IncotermId");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId");
            this.Property(t => t.IsDangerous).HasColumnName("IsDangerous");
            this.Property(t => t.ExpirationDays).HasColumnName("ExpirationDays");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.VolumetricWeight).HasColumnName("VolumetricWeight");            
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.PackageType1Id).HasColumnName("PackageType1Id");
            this.Property(t => t.PackageType2Id).HasColumnName("PackageType2Id");
            this.Property(t => t.PackageType3Id).HasColumnName("PackageType3Id");
            this.Property(t => t.PackageType4Id).HasColumnName("PackageType4Id");
            this.Property(t => t.PackageType5Id).HasColumnName("PackageType5Id");
            this.Property(t => t.PackageType1Quantity).HasColumnName("PackageType1Quantity");
            this.Property(t => t.PackageType3Quantity).HasColumnName("PackageType3Quantity");
            this.Property(t => t.PackageType2Quantity).HasColumnName("PackageType2Quantity");
            this.Property(t => t.PackageType4Quantity).HasColumnName("PackageType4Quantity");
            this.Property(t => t.PackageType5Quantity).HasColumnName("PackageType5Quantity");
            this.Property(t => t.IsByKG).HasColumnName("IsByKG");
            this.Property(t => t.IsByContainer).HasColumnName("IsByContainer");
            this.Property(t => t.QuoteTypeCode).HasColumnName("QuoteTypeCode");
            this.Property(t => t.EstimateProfit).HasColumnName("EstimateProfit");
            this.Property(t => t.EstimateProfitEdited).HasColumnName("EstimateProfitEdited");
            this.Property(t => t.MinimumFreightCost).HasColumnName("MinimumFreightCost");
            this.Property(t => t.MinimumFreightSale).HasColumnName("MinimumFreightSale");
            this.Property(t => t.MainCarriageCarrierId).HasColumnName("MainCarriageCarrierId");
            this.Property(t => t.IsFreightBySteps).HasColumnName("IsFreightBySteps");
            this.Property(t => t.IsCancelled).HasColumnName("IsCancelled");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerContactId).HasColumnName("CustomerContactId");
            this.Property(t => t.CustomerReference1).HasColumnName("CustomerReference1");
            this.Property(t => t.CustomerReference2).HasColumnName("CustomerReference2");
            this.Property(t => t.QuoteCustomerTypeCode).HasColumnName("QuoteCustomerTypeCode");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.SaleCurrencyId).HasColumnName("SaleCurrencyId");
            this.Property(t => t.ExchangeRate).HasColumnName("ExchangeRate");
            this.Property(t => t.ShipperName).HasColumnName("ShipperName");
            this.Property(t => t.ConsigneeName).HasColumnName("ConsigneeName");
            this.Property(t => t.PickUpAddress).HasColumnName("PickUpAddress");
            this.Property(t => t.DeliveryAddress).HasColumnName("DeliveryAddress");
            this.Property(t => t.IsFixedPrice).HasColumnName("IsFixedPrice");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode");
            this.Property(t => t.ConcurrencyGUID).HasColumnName("ConcurrencyGUID");
            this.Property(t => t.FromPartnerId).HasColumnName("FromPartnerId");
            this.Property(t => t.ToPartnerId).HasColumnName("ToPartnerId");
            this.Property(t => t.FromPartnerAddressId).HasColumnName("FromPartnerAddressId");
            this.Property(t => t.ToPartnerAddressId).HasColumnName("ToPartnerAddressId");
            this.Property(t => t.FollowUpId).HasColumnName("FollowUpId");
            this.Property(t => t.FollowUpDate).HasColumnName("FollowUpDate");
            this.Property(t => t.FollowUpNotes).HasColumnName("FollowUpNotes");
            this.Property(t => t.FollowUpOwnerUserId).HasColumnName("FollowUpOwnerUserId");
            this.Property(t => t.Shipper).HasColumnName("Shipper");
            this.Property(t => t.Consignee).HasColumnName("Consignee");
            this.Property(t => t.FollowUpType).HasColumnName("FollowUpType");
            this.Property(t => t.FromPortCode).HasColumnName("FromPortCode");
            this.Property(t => t.FromPortName).HasColumnName("FromPortName");
            this.Property(t => t.FromPortCountry).HasColumnName("FromPortCountry");
            this.Property(t => t.ToPortCode).HasColumnName("ToPortCode");
            this.Property(t => t.ToPortName).HasColumnName("ToPortName");
            this.Property(t => t.ToPortCountry).HasColumnName("ToPortCountry");            
            this.Property(t => t.CreatedByUser).HasColumnName("CreatedByUser");
            this.Property(t => t.UpdatedByUser).HasColumnName("UpdatedByUser");
            this.Property(t => t.FollowUpOwner).HasColumnName("FollowUpOwner");
            this.Property(t => t.QuoteTypeName).HasColumnName("QuoteTypeName");
            this.Property(t => t.FollowUpTypeId).HasColumnName("FollowUpTypeId");
            this.Property(t => t.FollowUpOwnerId).HasColumnName("FollowUpOwnerId");
            this.Property(t => t.ShipmentTypeName).HasColumnName("ShipmentTypeName");
            this.Property(t => t.MainCarriageCarrierName).HasColumnName("MainCarriageCarrierName");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.IsSubjectEdited).HasColumnName("IsSubjectEdited");
            this.Property(t => t.StageId).HasColumnName("StageId");
            this.Property(t => t.StageName).HasColumnName("StageName");
            this.Property(t => t.StageDueDate).HasColumnName("StageDueDate");
            this.Property(t => t.RatingCode).HasColumnName("RatingCode");
            this.Property(t => t.LastActivityTypeCode).HasColumnName("LastActivityTypeCode");
            this.Property(t => t.LastActivitySubject).HasColumnName("LastActivitySubject");
            this.Property(t => t.NextActivityTypeCode).HasColumnName("NextActivityTypeCode");
            this.Property(t => t.NextActivitySubject).HasColumnName("NextActivitySubject");
            this.Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");
            this.Property(t => t.NextActivityDate).HasColumnName("NextActivityDate");
            this.Property(t => t.OpportunityId).HasColumnName("OpportunityId");
            this.Property(t => t.IsAutomaticallyClosed).HasColumnName("IsAutomaticallyClosed");
            this.Property(t => t.AutomaticallyCloseDate).HasColumnName("AutomaticallyCloseDate");
            this.Property(t => t.AutomaticallyCloseDays).HasColumnName("AutomaticallyCloseDays");
            this.Property(t => t.SalesmanUserId).HasColumnName("SalesmanUserId");
            this.Property(t => t.BusinessUnitId).HasColumnName("BusinessUnitId");
            this.Property(t => t.BusinessUnitName).HasColumnName("BusinessUnitName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.IncotermCode).HasColumnName("IncotermCode");
            this.Property(t => t.ValueOfGoods).HasColumnName("ValueOfGoods");
            this.Property(t => t.GrossWeightInKG).HasColumnName("GrossWeightInKG");
            this.Property(t => t.GrossWeightPerTon).HasColumnName("GrossWeightPerTon");
        }
    }
}
