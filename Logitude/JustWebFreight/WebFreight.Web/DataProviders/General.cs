using System;

using Simplog.Data.CommonDataModel.EntityPOCOs;

using WebFreight.Web.WebServices;

namespace WebFreight.Web.DataProviders
{
    public class General
    {
        public static string GetAddress(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (address.State != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + " " + (address.State.LocalName != null ? address.State.LocalName : "");
                    }

                    else
                    {
                        resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
                    }
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.EnglishName;
                    }
                }
            }

            return resultAddress;
        }

        public static string GetAddress_OneLine(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + ", " + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + ", " + address.City;
                }

                if (address.State != null)
                {
                    resultAddress = resultAddress + ", " + (address.State.Code != null ? address.State.Code : "");
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + ", " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + ", " + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + ", " + address.Country.EnglishName;
                    }
                }
            }

            return resultAddress;
        }

        public static string GetAddressWithName(Address address, bool isStateCode = false )
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Name != null ? address.Name : "";
                
                if (!string.IsNullOrEmpty(address.Address1))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address1;
                }

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (address.State != null)
                {
                    if (!isStateCode)
                    {
                        if (address.IsLocalLanguage)
                        {
                            resultAddress = resultAddress + " " + (address.State.LocalName != null ? address.State.LocalName : "");
                        }
                        else
                        {
                            resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
                        }
                    } 
                    else
                    {
                        resultAddress = resultAddress + " " + (address.State.Code != null ? address.State.Code : "");
                    }
                   
                }
                
                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.EnglishName;
                    }
                }

                if (address.PhoneNumber != null || address.FaxNumber != null)
                {
                    resultAddress = resultAddress + Environment.NewLine + (address.PhoneNumber != null ? "Tel: " + address.PhoneNumber + " " : "") + (address.FaxNumber != null ? "Fax: " + address.FaxNumber + " " : "");
                }
            }

            return resultAddress;
        }

        public static byte[] GetLogo(int tenant)
        {
            Uploader uploaderservice = new Uploader();
            byte[] logodata = uploaderservice.DownloadFile("logo" + tenant.ToString(), "jpg", "logos", tenant);

            return logodata;
        }
    }
}