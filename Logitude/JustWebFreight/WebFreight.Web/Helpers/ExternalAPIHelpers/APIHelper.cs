using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.ExternalAPIHelpers
{
    public class APIHelper
    {
        public static void AddCommunicationLog<T, T2>(string communicationStatusTypeCode, T body, T2 response, string tableName, string entityId, string subject, int? tenantnumber = null)
        {
            try
            {
                int tenant = 0;
                if (tenantnumber == null && HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null)
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken != null)
                        {
                            tenant = authToken.Tenant;
                        }
                    }
                }
                else tenant = (int)tenantnumber;

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                DocumentRepository documentRepository = new DocumentRepository(commonContext);
                UserRepository userRepository = new UserRepository(commonContext);
                CommunicationLogService communicationLogService = new CommunicationLogService(commonContext, tenant);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);

                string objectTableId = null;
                if (!string.IsNullOrEmpty(tableName))
                {
                    ObjectTable table = objectTableRepository.GetObjectTableByName(tableName, tenant, false);
                    if (table != null)
                        objectTableId = table.Id;

                }

                string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                User loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, true);

                #region Body Message Document
                byte[] bytearray = body != null ? LogitudeXmlSerializer.SerializeObject<T>(body) : new byte[0];
                Document bodyDocument = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = bytearray.Length,
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "api",
                };

                documentRepository.Add(bodyDocument);
                #endregion

                #region Response Body Document
                Document responseDocument = null;
                byte[] responseByteArray = null;
                if (response != null)
                {
                    responseByteArray = LogitudeXmlSerializer.SerializeObject<T2>(response);

                    responseDocument = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "xml",
                        FileSize = responseByteArray.Length,
                        Tenant = Convert.ToInt32(tenant),
                        Id = IdCounter.GetNumber("Document", tenant),
                        HasFile = true,
                        Folder = "api",
                    };

                    documentRepository.Add(responseDocument);

                }
                #endregion

                documentRepository.SubmitChanges();

                CommunicationLogPM commLog = new CommunicationLogPM()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    LastStatusDateUTC = DateTime.UtcNow,
                    InOut = "I",
                    Subject = subject,
                    Tenant = tenant,
                    CommunicationLogTypeCode = "A",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CommunicationStatusTypeCode = communicationStatusTypeCode,
                    DocumentId = bodyDocument.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    CreatedByUserId = loggedUser.Id,
                    ObjectTableId = objectTableId,
                    EntityId = entityId,
                    ResponseDocumentId = responseDocument != null ? responseDocument.Id : null,

                };

                communicationLogService.Create(commLog);


                #region Write Document On Storage
                Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = bodyDocument.Id,
                    FolderName = bodyDocument.Folder,
                    Extension = bodyDocument.Extension,
                    Tenant = tenant,
                    FileSize = bytearray.Length,
                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                storageservice.Write(bytearray.ToArray(), fileInfo);

                if (responseByteArray != null && responseDocument != null)
                {
                    fileInfo.FileName = responseDocument.Id;
                    fileInfo.FolderName = responseDocument.Folder;
                    fileInfo.Extension = responseDocument.Extension;
                    fileInfo.Tenant = tenant;
                    fileInfo.FileSize = responseByteArray.Length;
                    storageservice.Write(responseByteArray.ToArray(), fileInfo);
                }
                #endregion

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "API", null, null);
            }
        }

        public static string AddCommunicationLog<T, T2>(T body, T2 response, string tableName, string entityId, string subject, int? tenantnumber = null)
        {
            try
            {
                int tenant = 0;
                if (tenantnumber == null && HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null)
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken != null)
                        {
                            tenant = authToken.Tenant;
                        }
                    }
                }
                else tenant = (int)tenantnumber;

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                DocumentRepository documentRepository = new DocumentRepository(commonContext);
                UserRepository userRepository = new UserRepository(commonContext);
                CommunicationLogService communicationLogService = new CommunicationLogService(commonContext, tenant);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);

                string objectTableId = null;
                if (!string.IsNullOrEmpty(tableName))
                {
                    ObjectTable table = objectTableRepository.GetObjectTableByName(tableName, tenant, false);
                    if (table != null)
                        objectTableId = table.Id;

                }

                string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                User loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, true);

                #region Body Message Document
                byte[] bytearray = body != null ? LogitudeXmlSerializer.SerializeObject<T>(body) : new byte[0];
                Document bodyDocument = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = bytearray.Length,
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "api",
                };

                documentRepository.Add(bodyDocument);
                #endregion

                #region Response Body Document
                Document responseDocument = null;
                byte[] responseByteArray = null;
                if (response != null)
                {
                    responseByteArray = LogitudeXmlSerializer.SerializeObject<T2>(response);

                    responseDocument = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "xml",
                        FileSize = responseByteArray.Length,
                        Tenant = Convert.ToInt32(tenant),
                        Id = IdCounter.GetNumber("Document", tenant),
                        HasFile = true,
                        Folder = "api",
                    };

                    documentRepository.Add(responseDocument);

                }
                #endregion

                documentRepository.SubmitChanges();

                CommunicationLogPM commLog = new CommunicationLogPM()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    LastStatusDateUTC = DateTime.UtcNow,
                    InOut = "I",
                    Subject = subject,
                    Tenant = tenant,
                    CommunicationLogTypeCode = "A",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CommunicationStatusTypeCode = "W",
                    DocumentId = bodyDocument.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    CreatedByUserId = loggedUser.Id,
                    ObjectTableId = objectTableId,
                    EntityId = entityId,
                    ResponseDocumentId = responseDocument != null ? responseDocument.Id : null,

                };

                communicationLogService.Create(commLog);



                #region Write Document On Storage
                Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = bodyDocument.Id,
                    FolderName = bodyDocument.Folder,
                    Extension = bodyDocument.Extension,
                    Tenant = tenant,
                    FileSize = bytearray.Length,
                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                storageservice.Write(bytearray.ToArray(), fileInfo);

                if (responseByteArray != null && responseDocument != null)
                {
                    fileInfo.FileName = responseDocument.Id;
                    fileInfo.FolderName = responseDocument.Folder;
                    fileInfo.Extension = responseDocument.Extension;
                    fileInfo.Tenant = tenant;
                    fileInfo.FileSize = responseByteArray.Length;
                    storageservice.Write(responseByteArray.ToArray(), fileInfo);
                }
                #endregion

                return commLog.Id;

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "API", null, null);
                return "0";
            }
        }

        public static void UpdateCommunicationLog<T, T2>(string communicationId, string communicationStatusTypeCode, T body, T2 response, string tableName, string entityId, string subject, int? tenantnumber = null)
        {
            try
            {
                int tenant = 0;
                if (tenantnumber == null && HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null)
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken != null)
                        {
                            tenant = authToken.Tenant;
                        }
                    }
                }
                else tenant = (int)tenantnumber;

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                DocumentRepository documentRepository = new DocumentRepository(commonContext);
                UserRepository userRepository = new UserRepository(commonContext);
                CommunicationLogQuery communicationLogQuery = new CommunicationLogQuery(communicationLogRepository);
                CommunicationLogService communicationLogService = new CommunicationLogService(commonContext, tenant);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);

                string objectTableId = null;
                if (!string.IsNullOrEmpty(tableName))
                {
                    ObjectTable table = objectTableRepository.GetObjectTableByName(tableName, tenant, false);
                    if (table != null)
                        objectTableId = table.Id;

                }

                string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                User loggedUser = userRepository.GetSingleUserByEmail(loggedUserEmail, tenant, true);

         
                #region Response Body Document
                Document responseDocument = null;
                byte[] responseByteArray = null;
                if (response != null)
                {
                    responseByteArray = LogitudeXmlSerializer.SerializeObject<T2>(response);

                    responseDocument = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "xml",
                        FileSize = responseByteArray.Length,
                        Tenant = Convert.ToInt32(tenant),
                        Id = IdCounter.GetNumber("Document", tenant),
                        HasFile = true,
                        Folder = "api",
                    };

                    documentRepository.Add(responseDocument);

                }
                #endregion

                documentRepository.SubmitChanges();

                CommunicationLogPM commLog = communicationLogQuery.GetSinglePM(communicationId, tenant);
                if (commLog == null)
                {
                    AddCommunicationLog(body, response, "Journal", null, "Journal API", tenant); 
                }
                commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                commLog.LastStatusDateUTC = DateTime.UtcNow;   
                commLog.CommunicationStatusTypeCode = communicationStatusTypeCode;   
                commLog.EntityId = entityId;
                commLog.ResponseDocumentId = responseDocument != null ? responseDocument.Id : null;

                communicationLogService.Update(commLog);


                #region Write Document On Storage
                Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo(); 
                Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
 
                if (responseByteArray != null && responseDocument != null)
                {
                    fileInfo.FileName = responseDocument.Id;
                    fileInfo.FolderName = responseDocument.Folder;
                    fileInfo.Extension = responseDocument.Extension;
                    fileInfo.Tenant = tenant;
                    fileInfo.FileSize = responseByteArray.Length;
                    storageservice.Write(responseByteArray.ToArray(), fileInfo);
                }
                #endregion

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "API", null, null);
            }
        }
    }
}