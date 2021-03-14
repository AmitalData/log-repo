using System;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.WebServices;
using System.Linq;
using Microsoft.Practices.Unity;
using Logitude.BL.CommonDataModel.EntityQueries;

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



    }
}