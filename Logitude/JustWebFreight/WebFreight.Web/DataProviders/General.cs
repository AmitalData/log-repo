using System;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.WebServices;
using System.Linq;
using Microsoft.Practices.Unity;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Server.Infrastructure;

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

        public static string GetFieldString(string field)
        {
            string output = field;

            if (string.IsNullOrEmpty(output))
            {
                output = "";
            }

            return output;
        }
        public static string GetFieldString(int? field)
        {
            string output = "";

            if (field != null)
            {
                output = field.Value.ToString();
            }

            return output;
        }
        public static string GetFieldString(double? field, string extension = null)
        {
            string output = "";

            if (field != null)
            {
                output = field.Value.ToString();

                if(extension != null)
                {
                    output += " " + extension;
                }
            }

            return output;
        }

        public static byte[] GetCarrierLogo(string carrierId, int tenant)
        {
            byte[] output = null;

            if (!string.IsNullOrEmpty(carrierId))
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);
                
                string imageDetailId = (from d in commonContext.Cards where d.Id == carrierId select d.ImageDetailId).FirstOrDefault();

                if (!string.IsNullOrEmpty(imageDetailId))
                {
                    ImageDetailRepository imageDetailsRepository = new ImageDetailRepository(webfreightContext);
                    ImageDetail imageDetail = imageDetailsRepository.GetSingleImageDetail(imageDetailId, tenant);

                    if (imageDetail != null)
                    {
                        output = GetFile(imageDetail.Id, imageDetail.Extension, "images", tenant);
                    }
                }
            }

            return output;
        }

        public static byte[] GetUserSignatureImage(string imageDetailId, int tenant)
        {
            byte[] output = null;
            if (!string.IsNullOrEmpty(imageDetailId))
            {
                IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);
                ImageDetailRepository imageDetailsRepository = new ImageDetailRepository(webfreightContext);
                ImageDetail imageDetail = imageDetailsRepository.GetSingleImageDetail(imageDetailId, tenant);

                if (imageDetail != null)
                {
                    output = GetFile(imageDetail.Id, imageDetail.Extension, "images", tenant);
                }
            }
            return output;
        }

        private static byte[] GetFile(string fileid, string extention, string location, int tenant)
        {
            try
            {
                Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = fileid,
                    FolderName = location,
                    Extension = extention,
                    Tenant = tenant,

                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;

                return storageservice.Read(fileInfo);
            }

            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetCompanyName(int tenant)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            return tenantQuery.GetCompanyNameById(tenant);
        }

        private static string tenantManagementCustomerURL;
        private static bool hideSharedlogistics;
        private static bool isBrandingEnabled;
        public static string BuildShipmentNumberLink(string shipmentId, string shipmentLevelCode, string securitykey, int tenant)
        {
            string shipmentNumberURL = "";

            SetTenantManagementProperties(tenant);
            string myUrl = tenantManagementCustomerURL;

            if (string.IsNullOrEmpty(tenantManagementCustomerURL))
            {
                myUrl = LogitudeSettings.LogitudeURL;
                if (isBrandingEnabled)
                {
                    myUrl = LogitudeSettings.LogitudeURL + "/login.aspx?tenant=" + tenant;
                }
            }
            
            if (myUrl.Contains("login.aspx"))
            {
                myUrl = GetOnlyDomainNameFromSystemUrl(myUrl);
            }

            string pagePath = @"/SharedLogistic/ShipmentPage.aspx";
            Tenant myTenant = GetCurrentTenant(tenant);

            if (myTenant != null && myTenant.SharedLogisMasterMessageLink && shipmentLevelCode == "C")
            {
                myUrl = GetSystemURL(tenant, myUrl);
                pagePath = @"/SharedMasterDocumentsPage.aspx";
                shipmentNumberURL = (myUrl + pagePath).ToLower() + "?securitykey=" + securitykey;
            }

            else
            {
                shipmentNumberURL = (myUrl + pagePath).ToLower() + "?securitykey=" + securitykey + ":" + shipmentId + ":" +
                                  tenant + ":" + hideSharedlogistics;            
            }

            if (!shipmentNumberURL.Contains("//"))
            {
                shipmentNumberURL = "https://" + shipmentNumberURL;
            }

            return shipmentNumberURL;
        }

        private static void SetTenantManagementProperties(int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetTenantManagementPM(tenant);
                if (tenantManagementPM != null)
                {
                    tenantManagementCustomerURL = tenantManagementPM.CustomerURL;
                    isBrandingEnabled = tenantManagementPM.EnableBranding;
                    if (tenantManagementPM.EnableBranding)
                    {
                        hideSharedlogistics = tenantManagementPM.HideSharedlogistics;
                    }
                }
                scope.Complete();
            }
        }
        private static string GetOnlyDomainNameFromSystemUrl(string systemUrl)
        {
            string url = systemUrl;
            string[] test = systemUrl.Split('/');
            if (test != null && test.Length > 0)
            {
                url = systemUrl.Replace("/" + test[test.Length - 1], "");
            }

            return url;
        }
        private static Tenant GetCurrentTenant(int tenant)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            return context.Tenants.Where(t => t.Id == tenant).FirstOrDefault();
        }        
        private static string GetSystemURL(int tenant, string systemUrl)
        {
            string myUrl = systemUrl;
            if (!string.IsNullOrEmpty(tenantManagementCustomerURL))
            {
                myUrl = tenantManagementCustomerURL;
            }
            myUrl = myUrl.ToLower().Replace("/cargotracking", "");

            return myUrl;
        }

        public static double? ComputeWeightInSelectedUnit(double? grossWeight, string srcUnitCode, string grossWeightUnitCode)
        {
            if (grossWeight == null) return null;
            if (string.IsNullOrEmpty(grossWeightUnitCode) || string.IsNullOrEmpty(srcUnitCode)) return grossWeight;
            if (srcUnitCode == grossWeightUnitCode) return grossWeight;

            if (grossWeightUnitCode == "KG") return ConvertWeightToKG(grossWeight, srcUnitCode);
            if (grossWeightUnitCode == "LB") return ConvertWeightToLB(grossWeight, srcUnitCode);
            if (grossWeightUnitCode == "MT") return ConvertWeightToMT(grossWeight, srcUnitCode);
            return grossWeight;
        }

        private static double? ConvertWeightToMT(double? weight, string srcUnitCode)
        {
            if (srcUnitCode == "LB") return weight * 0.000453592;
            if (srcUnitCode == "KG") return weight * 0.001;
            return weight;
        }

        private static double? ConvertWeightToLB(double? weight, string srcUnitCode)
        {
            if (srcUnitCode == "KG") return weight * 2.20462;
            if (srcUnitCode == "MT") return weight * 2204.62;
            return weight;
        }

        private static double? ConvertWeightToKG(double? weight, string srcUnitCode)
        {
            if (srcUnitCode == "LB") return weight * 0.45359237;
            if (srcUnitCode == "MT") return weight * 1000;
            return weight;
        }

        public static double? ComputeVolumeInSelectedUnit(double? volume, string srcUnitCode, string volumeUnitCode)
        {
            if (volume == null) return null;
            if (string.IsNullOrEmpty(volumeUnitCode) || string.IsNullOrEmpty(srcUnitCode)) return volume;
            if (srcUnitCode == volumeUnitCode) return volume;

            if (volumeUnitCode == "CBM") return ConvertVolumeToCBM(volume, srcUnitCode);
            if (volumeUnitCode == "CBI") return ConvertWeightToCBI(volume, srcUnitCode);
            if (volumeUnitCode == "CBF") return ConvertWeightToCBF(volume, srcUnitCode);
            return volume;
        }

        private static double? ConvertWeightToCBF(double? volume, string srcUnitCode)
        {
            if (srcUnitCode == "CBI") return volume * 0.000578704;
            if (srcUnitCode == "CBM") return volume * 35.315;
            return volume;
        }

        private static double? ConvertVolumeToCBM(double? volume, string srcUnitCode)
        {
            if (srcUnitCode == "CBI") return volume / 61024;
            if (srcUnitCode == "CBF") return volume * 0.0283168;
            return volume;
        }

        private static double? ConvertWeightToCBI(double? volume, string srcUnitCode)
        {
            if (srcUnitCode == "CBM") return volume * 61024;
            if (srcUnitCode == "CBF") return volume * 0.000578704;
            return volume;
        }
    }
}