using CWXSD;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.XSD.CW_API.ABM
{
    public class ABMDataBuilder
    {

        public ABMDataContext Context { get; set; }
        public ABMDataBuilder(ABMDataContext myContext)
        {
            this.Context = myContext;
        }

        public CustomsForceServiceRequestMessageHeader GetABMHeader()
        {
            CustomsForceServiceRequestMessageHeader header = new CustomsForceServiceRequestMessageHeader();

            header.SecurityToken = new CustomsForceServiceRequestMessageHeaderSecurityToken()
            {
                UserName = this.Context.UserID,
                Password = this.Context.Password,
                Company = this.Context.CompanyID,
                ApplicationId = this.Context.ApplicationID,
            };

            header.SessionInfo = new CustomsForceServiceRequestMessageHeaderSessionInfo()
            {
                //LanguageCode = "",
            };

            return header;
        }

        public CustomsForceServiceRequestMessageBody GetABMBody()
        {
            CustomsForceServiceRequestMessageBody body = new CustomsForceServiceRequestMessageBody();

            body.RequestGlobalData = new CustomsForceServiceRequestMessageBodyRequestGlobalData()
            {
                //AsyncFlag = "",
            };

            body.RequestList = new CustomsForceServiceRequestMessageBodyRequestList();
            body.RequestList.RequestItem = new CustomsForceServiceRequestMessageBodyRequestListRequestItem()
            {
                
            };

            body.RequestList.RequestItem.DataList = new CustomsForceServiceRequestMessageBodyRequestListRequestItemDataList();
            body.RequestList.RequestItem.DataList.DataItem = new CustomsForceServiceRequestMessageBodyRequestListRequestItemDataListDataItem();

            //body.RequestList.RequestItem.DataList.DataItem.Text = ;

            body.RequestList.RequestItem.DataList.DataItem.InputDocument = new InputDocument();

            body.RequestList.RequestItem.DataList.DataItem.InputDocument.Credentials = new Credentials()
            {
                UserID = this.Context.UserID,                                
                Password = this.Context.Password,
                CompanyID = this.Context.CompanyID,
                //LicenseCode = "",
            };

            body.RequestList.RequestItem.DataList.DataItem.InputDocument.ConsignmentList = new ConsignmentList();
            List<Consignment> Consignments = new List<Consignment>();
            Consignment myConsignment = this.BuildConsignment();
            Consignments.Add(myConsignment);
            body.RequestList.RequestItem.DataList.DataItem.InputDocument.ConsignmentList.Consignment = Consignments.ToArray<Consignment>();

            return body;
        }

        private Consignment BuildConsignment()
        {
            Consignment myItem = new Consignment();

            myItem.Version = "4.35";
            myItem.Command = "Update";

            myItem.ConsignmentHeader = new ConsignmentHeader();

            myItem.ConsignmentHeader.ConsignmentReference = this.Context.ShipmentNumber;

            #region ValueAmount
            string[] iCurrencyText = new string[1];
            if(this.Context.ValueOfGoodsCurrencyCode != null)
            {
                iCurrencyText[0] = this.Context.ValueOfGoodsCurrencyCode;
            }

            if (this.Context.ValueOfGoods != null)
            {
                List<ValueAmount> iValueAmounts = new List<ValueAmount>();

                iValueAmounts.Add(new ValueAmount()
                {
                    ValueType = "DocumentValue",
                    AmountValue = this.Context.ValueOfGoods.Value,
                    AmountValueSpecified = true,
                    
                    Currency = new Currency()
                    {                        
                        CodeType = CurrencyCodeType.ISO,
                        Text = iCurrencyText,
                    },
                });

                myItem.ConsignmentHeader.ValueAmount = iValueAmounts.ToArray<ValueAmount>();
            }
            #endregion

            //myItem.ConsignmentHeader.ConsignmentBaseCurrency = this.Context.ValueOfGoodsCurrencyCode;

            #region Transport
            CWXSD.Transport iTransportItem = new Transport()
            {
                Conveyance = this.Context.TransportConveyance,
                TransportType = TransportTransportType.Border,
                TransportTypeSpecified = true,
            };

            List<CWXSD.Transport> iTransport = new List<Transport>();

            iTransport.Add(iTransportItem);

            myItem.ConsignmentHeader.Transport = iTransport.ToArray<CWXSD.Transport>();
            #endregion

            #region Reference 
            List<Reference> references = new List<Reference>();
            references.Add(new Reference()
            {
                RefCode = "HWB",
                RefText = this.Context.HouseNumber,
            });

            references.Add(new Reference()
            {
                RefCode = "MWB",
                RefText = this.Context.MasterNumber,
            });

            myItem.ConsignmentHeader.Reference = references.ToArray<Reference>();
            #endregion

            #region Country
            List<Country> countries = new List<Country>();
            countries.Add(new Country()
            {
                CountryType = "Dispatch",
                CodeType = CountryCodeType.ISO,
                Text = new string[] { this.Context.MainCarriageFromPortCountryCode },
            });

            countries.Add(new Country()
            {
                CountryType = "Destination",
                CodeType = CountryCodeType.ISO,
                Text = new string[] { this.Context.FinalDestinationPortCountryCode },
            });

            myItem.ConsignmentHeader.Country = countries.ToArray<Country>();
            #endregion

            #region  Port
            List<Port> ports = new List<Port>();
            ports.Add(new Port()
            {
                Text = new string[] { this.Context.FromPortCode },
                PortCountry = this.Context.MainCarriageFromPortCountryCode,
                PortType = "Origin",
                CodeType = PortCodeType.UNLOC,
            });

            ports.Add(new Port()
            {
                Text = new string[] { this.Context.FinalDestinationPortCode },
                PortCountry = this.Context.FinalDestinationPortCountryCode,
                PortType = "Arrival",
                CodeType = PortCodeType.UNLOC,
            });

            myItem.ConsignmentHeader.Port = ports.ToArray<Port>();
            #endregion

            #region Parties
            if (this.Context.Parties.Count > 0)
            {
                myItem.ConsignmentHeader.Party = this.Context.Parties.ToArray<CWXSD.Party>();

            }            
            #endregion

            #region Goods Descriptio
            myItem.ConsignmentHeader.GoodsDescription = this.Context.DescriptionOfGoods;
            #endregion

            #region Measure
            List<ApplicationUnitsOfMeasure> measures = new List<ApplicationUnitsOfMeasure>();
            measures.Add(new ApplicationUnitsOfMeasure()
            {
                UOMCode = "DocumentPieces",
                UOMValue = new UOMValue() { Value = this.Context.NumberOfPackages },
            });

            measures.Add(new ApplicationUnitsOfMeasure()
            {
                UOMCode = "DocumentGrossWeight",
                UOMValue = new UOMValue() { Value = this.Context.GrossWeight },
            });

            measures.Add(new ApplicationUnitsOfMeasure()
            {
                UOMCode = "DocumentNetWeight",
                UOMValue = new UOMValue() { Value = this.Context.NetWeight },
            });

            //measures.Add(new ApplicationUnitsOfMeasure()
            //{
            //    UOMCode = "FreightProrate",
            //    UOMValue = new UOMValue() { Value = this.Context.FreightProrate },
            //});

            //measures.Add(new ApplicationUnitsOfMeasure()
            //{
            //    UOMCode = "WeightProrate",
            //    UOMValue = new UOMValue() { Value = this.Context.WeightProrate },
            //});

            myItem.ConsignmentHeader.Measure = measures.ToArray<ApplicationUnitsOfMeasure>();
            #endregion

            #region Container
            if (this.Context.Containers.Count() > 0)
            {
                myItem.ConsignmentHeader.Container = new Container();
                List<ContainerItem> containerItems = new List<ContainerItem>();

                foreach (ShipmentPackagePM container in this.Context.Containers)
                {
                    ContainerItem containerItem = new ContainerItem()
                    {
                        ContainerType = container.PackageTypeCode,
                        ContainerRef = container.ContainerNumber,
                        ContainerSealNumber = container.ShipperSeal,
                    };
                    
                    containerItems.Add(containerItem);
                }

                myItem.ConsignmentHeader.Container.ContainerItem = containerItems.ToArray<ContainerItem>();
            }
            #endregion

            #region Terms
            myItem.ConsignmentHeader.Terms = new Terms()
            {
                TermsCode = this.Context.IncotermCode,
            };
            #endregion

            return myItem;
        }
    }
}