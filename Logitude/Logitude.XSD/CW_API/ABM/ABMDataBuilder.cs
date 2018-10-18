using CWXSD;
using Logitude.BL.ShipmentsModel.EntityPMs;
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

            #region Party

            //Shipper
            Party shipper = new Party();
            shipper.PartyType = "Consignor";
            shipper.NameAddress = new NameAddress()
            {
                Name = this.Context.ShipperName,
                Address1 = this.Context.ShipperAddress1,
                Address2 = this.Context.ShipperAddress2,
                Address3 = this.Context.ShipperCity,
                PostCode = this.Context.ShipperZipCode,
                Country = new Country()
                {
                    CountryType = "Consignor",
                    CodeType = CountryCodeType.ISO,
                    Text = new string[] { this.Context.ShipperCountryCode },
                },
            };

            shipper.AddressLocation = new GPSEvent();

            if (!string.IsNullOrEmpty(this.Context.ShipperReference1) || !string.IsNullOrEmpty(this.Context.ShipperReference2))
            {
                List<Reference> shipperReference = new List<Reference>();

                if (!string.IsNullOrEmpty(this.Context.ShipperReference1))
                {
                    shipperReference.Add(new Reference()
                    {
                        RefCode = "reference 1",
                        RefText = this.Context.ShipperReference1,
                    });
                }

                if (!string.IsNullOrEmpty(this.Context.ShipperReference2))
                {
                    shipperReference.Add(new Reference()
                    {
                        RefCode = "reference 2",
                        RefText = this.Context.ShipperReference2,
                    });
                }

                shipper.Reference = shipperReference.ToArray<Reference>();
            }

            //Consignee
            Party consignee = new Party();
            consignee.PartyType = "Consignee";
            consignee.NameAddress = new NameAddress()
            {
                Name = this.Context.ConsigneeName,
                Address1 = this.Context.ConsigneeAddress1,
                Address2 = this.Context.ConsigneeAddress2,
                Address3 = this.Context.ConsigneeCity,
                PostCode = this.Context.ConsigneeZipCode,
                Country = new Country()
                {
                    CountryType = "Consignee",
                    CodeType = CountryCodeType.ISO,
                    Text = new string[] { this.Context.ConsigneeCountryCode },
                },
            };

            consignee.AddressLocation = new GPSEvent();

            if (!string.IsNullOrEmpty(this.Context.ConsigneeReference1) || !string.IsNullOrEmpty(this.Context.ConsigneeReference2))
            {
                List<Reference> consigneeReference = new List<Reference>();

                if (!string.IsNullOrEmpty(this.Context.ConsigneeReference1))
                {
                    consigneeReference.Add(new Reference()
                    {
                        RefCode = "reference 1",
                        RefText = this.Context.ConsigneeReference1,
                    });
                }

                if (!string.IsNullOrEmpty(this.Context.ConsigneeReference2))
                {
                    consigneeReference.Add(new Reference()
                    {
                        RefCode = "reference 2",
                        RefText = this.Context.ConsigneeReference2,
                    });
                }

                consignee.Reference = consigneeReference.ToArray<Reference>();
            }

            List<Party> parties = new List<Party>();
            parties.Add(shipper);
            parties.Add(consignee);
            myItem.ConsignmentHeader.Party = parties.ToArray<Party>();
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