using Logitude.Test.Base.Extensions;
using System;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Models.Builders
{
    public class MainCarriageLegBuilder
    {
        private MainCarriageLeg MainCarriageLeg;
        
        public MainCarriageLegBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            MainCarriageLeg = new MainCarriageLeg();
        }

        public MainCarriageLegBuilder LegIndex(int legIndex)
        {
            MainCarriageLeg.LegIndex = legIndex;
            return this;
        }

        public MainCarriageLegBuilder Carrier(string carrierCode)
        {
            MainCarriageLeg.Carrier = new Card
            {
                Code = carrierCode
            };
            return this;
        }
        
        public MainCarriageLegBuilder FromPort(string fromPortCode)
        {
            MainCarriageLeg.FromPort = new Port
            {
                Code = fromPortCode
            };
            return this;
        }

        public MainCarriageLegBuilder ToPort(string toPortCode)
        {
            MainCarriageLeg.ToPort = new Port
            {
                Code = toPortCode
            };
            return this;
        }

        public MainCarriageLegBuilder ETD(DateTime? ETD)
        {
            MainCarriageLeg.ETD = ETD;
            return this;
        }

        public MainCarriageLegBuilder ETA(DateTime? ETA)
        {
            MainCarriageLeg.ETA = ETA;
            return this;
        }

        public MainCarriageLegBuilder ATD(DateTime? ATD)
        {
            MainCarriageLeg.ATD = ATD;
            return this;
        }

        public MainCarriageLegBuilder ATA(DateTime? ATA)
        {
            MainCarriageLeg.ATA = ATA;
            return this;
        }

        public MainCarriageLeg Build()
        {
            MainCarriageLeg result = MainCarriageLeg;

            this.Reset();

            return result;
        }

        public MainCarriageLegBuilder WithDefualtValues()
        {
            return this;
        }

        public MainCarriageLegBuilder FromDataTable(Table mainCarriageLegDataTable)
        {
            MainCarriageLeg = mainCarriageLegDataTable.CreateComplexInstance<MainCarriageLeg>();
            return this;
        }
    }
}