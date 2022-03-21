using Logitude.Base.Extensions;
using Logitude.Base.Models.PartnersPreparation;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Models.Builders
{
    public class DirectBuilder
    {
        private Direct Direct;

        public DirectBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            Direct = new Direct();
        }

        public DirectBuilder Id(string id)
        {
            Direct.Id = id;
            return this;
        }

        public DirectBuilder Agent(string agentCode)
        {
            Direct.Agent = new Card
            {
                Code = agentCode
            };
            return this;
        }

        public DirectBuilder Direction(string directionCode)
        {
            Direct.Direction = new Direction
            {
                Code = directionCode
            };
            return this;
        }

        public DirectBuilder TransportMode(string transportModeCode)
        {
            Direct.TransportMode = new TransportMode
            {
                Code = transportModeCode
            };
            return this;
        }

        public DirectBuilder ShipmentType(string shipmentTypeCode)
        {
            Direct.ShipmentType = new ShipmentType
            {
                Code = shipmentTypeCode
            };
            return this;
        }

        public DirectBuilder Shipper(string shipperCode)
        {
            Direct.Shipper = new Card
            {
                Code = shipperCode
            };
            return this;
        }

        public DirectBuilder ShipperReference1(string shipperReference1)
        {
            Direct.ShipperReference1 = shipperReference1;
            return this;
        }

        public DirectBuilder ShipperReference2(string shipperReference2)
        {
            Direct.ShipperReference2 = shipperReference2;
            return this;
        }

        public DirectBuilder GrossWeightUnit(string grossWeightUnitCode)
        {
            Direct.GrossWeightUnit = new WeightUnit
            {
                Code = grossWeightUnitCode
            };
            return this;
        }

        public DirectBuilder ChargeableWeightUnit(string chargeableWeightUnitCode)
        {
            Direct.ChargeableWeightUnit = new WeightUnit
            {
                Code = chargeableWeightUnitCode
            };
            return this;
        }

        public DirectBuilder VolumeUnit(string volumeUnitCode)
        {
            Direct.VolumeUnit = new VolumeUnit
            {
                Code = volumeUnitCode
            };
            return this;
        }

        public DirectBuilder Incoterm(string incotermCode)
        {
            Direct.Incoterm = new Incoterm
            {
                Code = incotermCode
            };
            return this;
        }

        public DirectBuilder MainCarriageCarrier(string mainCarriageCarrierCode)
        {
            Direct.MainCarriageCarrier = new Card
            {
                Code = mainCarriageCarrierCode
            };
            return this;
        }

        public DirectBuilder MainCarriageCarrierNumber(string mainCarriageCarrierNumber)
        {
            Direct.MainCarriageCarrierNumber = mainCarriageCarrierNumber;
            return this;
        }

        public DirectBuilder MainCarriageATD(DateTime? mainCarriageATD)
        {
            Direct.MainCarriageATD = mainCarriageATD;
            return this;
        }

        public DirectBuilder NewConcurrencyGUID(string newConcurrencyGUID)
        {
            Direct.NewConcurrencyGUID = newConcurrencyGUID;
            return this;
        }

        public DirectBuilder ConcurrencyGUID(string concurrencyGUID)
        {
            Direct.ConcurrencyGUID = concurrencyGUID;
            return this;
        }

        public DirectBuilder OceanOrInlandPackages(List<OceanOrInlandPackage> oceanOrInlandPackages)
        {
            Direct.OceanOrInlandPackages.AddRange(oceanOrInlandPackages);
            return this;
        }

        public DirectBuilder MainCarriageLegs(List<MainCarriageLeg> mainCarriageLegs)
        {
            Direct.MainCarriageLegs.AddRange(mainCarriageLegs);
            return this;
        }

        public Direct Build()
        {
            Direct result = Direct;

            this.Reset();

            return result;
        }

        public DirectBuilder WithDefualtValues()
        {
            Direct = new Direct
            {
                Shipper = new Card
                {
                    Code = PartnersData.ShipperExportCode
                },
                Agent = new Card
                {
                    Code = PartnersData.AgentCode
                }
            };
            return this;
        }

        public DirectBuilder FromDataTable(Table directDataTable)
        {
            Direct = directDataTable.CreateComplexInstance<Direct>();
            return this;
        }
    }
}