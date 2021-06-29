using System;
using System.Collections.Generic;
using Logitude.BL.DataContracts;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.IO;
using System.Xml;
using Logitude.Server.Tools;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentCloudCustomDataDeserializer
    {
        public ShipmentAdditionalCloudCustomData BuildCustomDataFromXML(ShipmentAdditionalCloudData data)
        {
            ShipmentAdditionalCloudCustomData CustomData = new ShipmentAdditionalCloudCustomData();
            CustomData.IsPaymentRequired = data.IsPaymentRequired;
            CustomData.PaymentDateTime = data.PaymentDateTime;
            if (data != null && !string.IsNullOrEmpty(data.DeclarationXmlData))
            {
                //byte[] myByteArray = Convert.FromBase64String(data.DeclarationXmlData);
                //using (var mem = new MemoryStream(myByteArray))
                //using (var zipStream = new ZipInputStream(mem))
                //{
                //    ZipEntry currentEntry;
                //    while ((currentEntry = zipStream.GetNextEntry()) != null)
                //    {
                //        //Console.WriteLine("{0} is {1} bytes", currentEntry.Name, currentEntry.Size);
                //        byte[] zipdata = new byte[currentEntry.Size];
                //        //ZipEntry.Read(data, 0, zipdata.Length);

                //        // do what ever with the data
                //    }
                //}
                int MySize = 1024;
                byte[] BytesUncompressed = new byte[MySize];
                StringBuilder MyEncodedUncompressMessage = new StringBuilder();
                try
                {
                    var MyMemoryStream = new MemoryStream(Convert.FromBase64String(data.DeclarationXmlData));
                    var MyZipInputStream = new BZip2InputStream(MyMemoryStream);
                    StringBuilder MyUncompressMessage = new StringBuilder();


                    Encoding wind1252 = Encoding.GetEncoding(1255);
                    Encoding utf8 = Encoding.UTF8;
                    byte[] utf8Bytes = new byte[MySize];
                    //byte[] MyBytesUncompressed = new byte[MyMemoryStream.Length];
                    while (true)
                    {
                        MySize = MyZipInputStream.Read(BytesUncompressed, 0, MySize);
                        //MyBytesUncompressed = (MyBytesUncompressed.Concat(BytesUncompressed)).ToArray();
                        if (MySize > 0)
                        {

                            utf8Bytes = Encoding.Convert(wind1252, utf8, BytesUncompressed, 0, MySize);
                            MyEncodedUncompressMessage.Append(Encoding.UTF8.GetString(utf8Bytes));
                            MyUncompressMessage.Append(Encoding.UTF8.GetString(BytesUncompressed, 0, MySize));
                        }

                        else
                            break;
                    }
                }
                catch (Exception)
                {
                    MyEncodedUncompressMessage.Append(Encoding.UTF8.GetString(Convert.FromBase64String(data.DeclarationXmlData)));
                }


                //Encoding wind1252 = Encoding.GetEncoding(1255);
                //Encoding utf8 = Encoding.UTF8;
                //byte[] wind1252Bytes = MyBytesUncompressed;
                //byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                //string utf8String = Encoding.UTF8.GetString(utf8Bytes);

                var data_out = MyEncodedUncompressMessage.ToString();
                BytesUncompressed = null;
                //MyBytesUncompressed = null;
                //MyUncompressMessage.Remove(0, MyUncompressMessage.Length);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(data_out);

                CustomData.IsImporterApprovalRequried = data.IsImporterApprovalRequried;
                CustomData.ApprovedByUserName = data.ApprovedByUserName;
                CustomData.VersionApproved = data.VersionApproved;
                CustomData.ApproveDateTime = data.ApproveDateTime;
                CustomData.DenyReason = data.DenyReason;
                CustomData.DocumentsApprovedByUserName = data.DocumentsApprovedByUserName;
                CustomData.GoodsValueDetails = new List<GoodsValueDetails>();
                CustomData.TaxesDetails = new List<TaxesDetails>();

                XmlNodeList CustomsFileNo = xmldoc.GetElementsByTagName("customs_file_num");
                if (CustomsFileNo[0] != null)
                {
                    CustomData.CustomsFileNo = CustomsFileNo[0].InnerText;
                }

                XmlNodeList DeclarationNo = xmldoc.GetElementsByTagName("declaration_num");
                if (DeclarationNo[0] != null)
                {
                    CustomData.DeclarationNo = DeclarationNo[0].InnerText;
                }

                XmlNodeList MishgorDescOfGoods1 = xmldoc.GetElementsByTagName("mishgor-desc_of_goods1");
                if (MishgorDescOfGoods1[0] != null)
                {
                    CustomData.MishgorDescOfGoods1 = MishgorDescOfGoods1[0].InnerText;
                }

                XmlNodeList GoodsValue = xmldoc.GetElementsByTagName("goods_value");
                if (GoodsValue[0] != null)
                {
                    CustomData.GoodsValue = GoodsValue[0].InnerText;
                }

                XmlNodeList CifValue = xmldoc.GetElementsByTagName("cif_value");
                if (CifValue[0] != null)
                {
                    CustomData.CifValue = CifValue[0].InnerText;
                }

                XmlNodeList TotalTax = xmldoc.GetElementsByTagName("total_tax");
                if (TotalTax[0] != null)
                {
                    CustomData.TotalTax = TotalTax[0].InnerText;
                }
                XmlNodeList mehes_draft_status = xmldoc.GetElementsByTagName("mehes_draft_status");
                if (mehes_draft_status[0] != null)
                {
                    CustomData.DeclarationStatus = mehes_draft_status[0].InnerText;
                }
                XmlNodeList MishgorPackageQuantity = xmldoc.GetElementsByTagName("mishgor-package-quantity");
                if (MishgorPackageQuantity[0] != null)
                {
                    CustomData.MishgorPackageQuantity = MishgorPackageQuantity[0].InnerText;
                }

                XmlNodeList MishgorPackageWeight = xmldoc.GetElementsByTagName("mishgor-package-weight");
                if (MishgorPackageWeight[0] != null)
                {
                    CustomData.MishgorPackageWeight = MishgorPackageWeight[0].InnerText;
                }

                XmlNodeList VersionId = xmldoc.GetElementsByTagName("version_id");
                if (VersionId[0] != null)
                {
                    CustomData.VersionId = VersionId[0].InnerText;
                }
                ////////////////////////////////////

                XmlNodeList TaxDetailsList = xmldoc.GetElementsByTagName("tax");
                for (int i = 0; i < TaxDetailsList.Count; i++)
                {
                    var ChildNodes = TaxDetailsList[i].ChildNodes;
                    var TaxDetails = new TaxesDetails();
                    for (int j = 0; j < ChildNodes.Count; j++)
                    {
                        if (ChildNodes[j].Name == "tax-taxtype")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "tax-taxtype-name")
                                {
                                    TaxDetails.Taxtypename = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                                else if (ChildNodes[j].ChildNodes[x].Name == "tax-taxtype-id")
                                {
                                    TaxDetails.TaxTypeCode = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                        }
                        else if (ChildNodes[j].Name == "tax-tax_basis")
                        {
                            TaxDetails.TaxBasis = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "tax-tax_to_pay")
                        {
                            TaxDetails.TaxToPay = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "tax-tax_amount")
                        {
                            TaxDetails.TaxAmount = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "tax-postponed_tax")
                        {
                            TaxDetails.TaxPostponed = ChildNodes[j].InnerText;
                        }
                    }
                    CustomData.TaxesDetails.Add(TaxDetails);
                }


                ///////////////////////////////////////////////////


                ////////////////////////////////

                XmlNodeList GoodsDetailsList = xmldoc.GetElementsByTagName("acc_supplier");
                for (int i = 0; i < GoodsDetailsList.Count; i++)
                {
                    var ChildNodes = GoodsDetailsList[i].ChildNodes;
                    var GoodValue = new GoodsValueDetails();
                    for (int j = 0; j < ChildNodes.Count; j++)
                    {
                        if (ChildNodes[j].Name == "acc_supplier-sup_account")
                        {
                            GoodValue.SupAccount = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-incoterm")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-incoterm-id")
                                {
                                    GoodValue.IncotermId = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-value")
                        {
                            GoodValue.Value = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-currency")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-currency-name" || ChildNodes[j].ChildNodes[x].Name == "acc_supplier-currency-id")
                                {
                                    GoodValue.CurrencyName = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                            //GoodValue.CurrencyName = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-country")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-country-name")
                                {
                                    GoodValue.CountryName = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }
                            //GoodValue.CountryName = ChildNodes[j].InnerText;
                        }
                        else if (ChildNodes[j].Name == "acc_supplier-supplier")
                        {
                            for (int x = 0; x < ChildNodes[j].ChildNodes.Count; x++)
                            {
                                if (ChildNodes[j].ChildNodes[x].Name == "acc_supplier-supplier-name")
                                {
                                    GoodValue.SupplierName = ChildNodes[j].ChildNodes[x].InnerText;
                                }
                            }

                        }
                        else if (ChildNodes[j].Name == "Acc_supplierfreight")
                        {
                            GoodValue.SupplierFreight = ChildNodes[j].InnerText;
                        }
                    }
                    CustomData.GoodsValueDetails.Add(GoodValue);
                }
                //XmlNodeList SupAccount = xmldoc.GetElementsByTagName("acc_supplier-sup_account");
                //if (SupAccount[0] != null)
                //{
                //    GoodValue.SupAccount = SupAccount[0].InnerText;
                //}

                //XmlNodeList IncotermId = xmldoc.GetElementsByTagName("acc_supplier-incoterm-id");
                //if (IncotermId[0] != null)
                //{
                //    GoodValue.IncotermId = IncotermId[0].InnerText;
                //}

                //XmlNodeList Value = xmldoc.GetElementsByTagName("Acc_supplierfreight");
                //if (Value[0] != null)
                //{
                //    GoodValue.Value = Value[0].InnerText;
                //}

                //XmlNodeList CurrencyName = xmldoc.GetElementsByTagName("acc_supplier-currency-name");
                //if (CurrencyName[0] != null)
                //{
                //    GoodValue.CurrencyName = CurrencyName[0].InnerText;
                //}

                //XmlNodeList CountryName = xmldoc.GetElementsByTagName("acc_supplier-country-name");
                //if (CountryName[0] != null)
                //{
                //    GoodValue.CountryName = CountryName[0].InnerText;
                //}

                //XmlNodeList SupplierName = xmldoc.GetElementsByTagName("acc_supplier-supplier-name");
                //if (SupplierName[0] != null)
                //{
                //    GoodValue.SupplierName = SupplierName[0].InnerText;
                //}

                //XmlNodeList SupplierFreight = xmldoc.GetElementsByTagName("Acc_supplierfreight");
                //if (SupplierFreight[0] != null)
                //{
                //    GoodValue.SupplierFreight = SupplierFreight[0].InnerText;
                //}
                //CustomData.GoodsValueDetails.Add(GoodValue);

            }
            if (data != null && !string.IsNullOrEmpty(data.PaymentRequestXML))
            {
                var MyPaymentData = LogitudeXmlSerializer.DeserializeObject<RequestPayment>(data.PaymentRequestXML);
                CustomData.RequestPaymentData = MyPaymentData;
            }

            if(data != null && string.IsNullOrEmpty(data.PaymentRequestXML) && string.IsNullOrEmpty(data.DeclarationXmlData))
            {
                return null;
            }

            return CustomData;
        }
    }
}