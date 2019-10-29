using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.FTP;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class DocumentsFilingBackupHelper
    {
        public static void UploadDocumentToFTP(string DocumentFilingId, int tenant, out string p_message)
        {
		    p_message = "";
			ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commoncontext);
            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commoncontext);

            DocumentsFiling documentFilingPOCO = documentsFilingRepository.GetSingleDocumentsFiling(DocumentFilingId, tenant);
            Document document = documentRepository.GetSingleDocument(documentFilingPOCO.Tenant, documentFilingPOCO.DocumentId);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentType(documentFilingPOCO.DocumentTypeId, documentFilingPOCO.Tenant);
            Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            byte[] filedata = storageservice.Read(fileInfo);

            if (filedata != null)
            {
                DocumentFilingBackupSettingQuery documentFilingBackupSettingQuery = new DocumentFilingBackupSettingQuery(tenant);
                DocumentFilingBackupSettingPM settingsPM = documentFilingBackupSettingQuery.GetSinglePM(tenant, tenant);
                FTPDetail ftpDetail = null;
                FTPDetailRepository ftpDetailRepository = new FTPDetailRepository(commoncontext);
                if (!string.IsNullOrEmpty(settingsPM.FTPDetailId))
                    ftpDetail = ftpDetailRepository.GetSingleFTPDetail(settingsPM.FTPDetailId, tenant);

                if (ftpDetail != null)
                {
                    string ftpHostIP = @"ftp://" + ftpDetail.Host;
                    string ftpUserName = ftpDetail.UserName;
                    string ftpPassword = ftpDetail.Password;
                    string ftpFolderName = ftpDetail.Folder;
                    string fileName = GetFileName(documentFilingPOCO, document, documentType);
					if (!ftpDetail.UseSFTP)
					{
						FTPService ftpService = new FTPService(ftpHostIP, ftpUserName, ftpPassword);
						ftpService.Upload(fileName, ftpFolderName, filedata,out p_message, true, true);
					}
					else
					{
						ftpHostIP = ftpDetail.Host;
						string p_status = "";
						
						SFTPService sftpService = new SFTPService();
						sftpService.Logon(ftpHostIP, ftpUserName, ftpPassword, "22", ftpFolderName, out p_status, out p_message);
						if (p_status == "0")
						{

							sftpService.Upload(fileName, filedata, true, true, out p_status, out p_message);

							if (p_status == "-1")
								throw new FTPServiceException("SFTP upload file failed: " + p_message);
						}
						else
							throw new FTPServiceException("SFTP Login failed: " + p_message);
					}

                    documentFilingPOCO.BackedupExternally = true;
                    documentFilingPOCO.LastBackupDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    documentsFilingRepository.Update(documentFilingPOCO);
                    documentsFilingRepository.SubmitChanges();
                }
                else
                {
                    throw new Exception("ftp credentials is not definded!");
                }
            }
            else
            {
                throw new Exception("The file data was not found!");
            }
        }

        private static string GetFileName(DocumentsFiling documentFiling, Document document, DocumentType documentType)
        {

            ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);
            string filename = (!string.IsNullOrEmpty(documentFiling.Description) ? documentFiling.Description : (!string.IsNullOrEmpty(document.FileName) ? document.FileName : document.Id)) + "_" + documentFiling.Code + "." + document.Extension;
            if ((!string.IsNullOrEmpty(documentFiling.EntityNumber) || !string.IsNullOrEmpty(documentFiling.EntityId)) && !string.IsNullOrEmpty(documentFiling.ObjectTableId))
            {
                ObjectTable table = objectTableRepository.GetSingleObjectTable(documentFiling.ObjectTableId, 0, false);
                switch (table.Name)
                {
                    case "Shipment":
                        {
                            string forwarderShipmentNumber = "";
                            ShipmentRepository shipmentRepository = new ShipmentRepository(documentFiling.Tenant);
                            if (!string.IsNullOrEmpty(documentFiling.EntityNumber))
                                forwarderShipmentNumber = shipmentRepository.GetForwarderShipmentNumberByShipmentNumberTenant(documentFiling.EntityNumber, documentFiling.Tenant);
                            else
                                forwarderShipmentNumber = shipmentRepository.GetForwarderShipmentNumberByShipmentIdTenant(documentFiling.EntityId, documentFiling.Tenant);

                            filename = documentFiling.Tenant + "_" + documentType.Code + "_" + documentFiling.Code + (!string.IsNullOrWhiteSpace(forwarderShipmentNumber) ? ("_" + forwarderShipmentNumber) : "") + (!string.IsNullOrEmpty(documentFiling.Description) ? "_" + documentFiling.Description : "") + "." + document.Extension;
                            break;
                        }
                    default:
                        break;
                }
            }
            Encoding unicode = Encoding.ASCII;
            var UnicodeFileName = unicode.GetString(Encoding.UTF8.GetBytes(filename));
            UnicodeFileName = UnicodeFileName.Replace("?", "");
            return UnicodeFileName;
        }
    }
}
