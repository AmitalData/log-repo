using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Text;
using System.Diagnostics;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Xml.Linq;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CommunicationLogStepQuery
    {
        CommunicationLogStepRepository repository;

        public CommunicationLogStepQuery()
        {
            repository = new CommunicationLogStepRepository(); 
        }

        public CommunicationLogStepQuery(int tenant)
        {
            repository = new CommunicationLogStepRepository(tenant);
        }

        public CommunicationLogStepQuery(CommunicationLogStepRepository CommunicationLogStepRepository)
        {
            repository = CommunicationLogStepRepository;
        }

        public CommunicationLogStepPM GetSinglePM(string id, int stepNumber, int tenant)
        {
            return (from a in repository.context.CommunicationLogSteps
                    where a.CommunicationLogId == id && a.StepNumber == stepNumber && a.Tenant == tenant
                    select GetEntityPM(a)).FirstOrDefault();
        }

        

        public IQueryable<CommunicationLogStepPM> GetCommunicationLogStepPMsByTenant(int tenant)
        {
            return (from a in repository.context.CommunicationLogSteps
                    where a.Tenant == tenant
                    select GetEntityPM(a));
        }

        public IQueryable<CommunicationLogStepPM> GetCommunicationLogStepPMsByLogId(string id, int tenant)
        {
            return (from a in repository.context.CommunicationLogSteps
                    where a.Tenant == tenant && a.CommunicationLogId == id
                    select GetEntityPM(a));
        }

        public List<CommunicationLogStepList> GetCommunicationLogStepListsByLogId(string id, int tenant)
        {

            var pokoList = (from a in repository.context.CommunicationLogSteps
                            where a.Tenant == tenant && a.CommunicationLogId == id
                            orderby a.StepNumber ascending 
                            select a
                            
                    ).ToList();
            var listPm = pokoList.Select(rec => GetStepListVersion(rec)).ToList();
            return listPm;
        }

        public static CommunicationLogStepList GetStepListVersion(CommunicationLogStep a)///itzik said reUse !!!
        {
            a = a ?? new CommunicationLogStep();
            return new CommunicationLogStepList()
            {
                CommunicationLogId = a.CommunicationLogId,
                DocumentId = a.DocumentId,
                EndDate = a.EndDate,
                Log = a.LogNormalized(),// a.Log,
                Name = a.Name,
                Retries = a.Retries,
                StartDate = a.StartDate,
                Status = a.Status,
                StatusName = a.CommunicationStatusType != null ? a.CommunicationStatusType.Name : null,
                StepNumber = a.StepNumber,
                Tenant = a.Tenant,
            };
        }
        private CommunicationLogStepPM GetEntityPM(CommunicationLogStep a)
        {
            return new CommunicationLogStepPM()
            {
                CommunicationLogId = a.CommunicationLogId,
                DocumentId = a.DocumentId,
                EndDate = a.EndDate,
                Log = a.LogNormalized(), //a.Log,
                Name = a.Name,
                Retries = a.Retries,
                StartDate = a.StartDate,
                Status = a.Status,
                StepNumber = a.StepNumber,
                StatusName = a.CommunicationStatusType != null ? a.CommunicationStatusType.Name : null,
                Tenant = a.Tenant,
            };
        }

        public IQueryable<CommunicationLogStepList> GetIQueryableEntityList(IQueryable<CommunicationLogStep> iQueryable)
        {
            IQueryable<CommunicationLogStepList> result = from a in iQueryable
                                                             select GetStepListVersion(a);
            return result;
        }

        public  List<CommunicationLogStepList> GetCommunicationLogStepsDocumentData(string communicationLogId, int tenant, int[] blobStepFilter,bool serverRequest /*= false*/, bool suppressCache)
        {
            byte[] ArryByte = null;
            //var myFilter = new int[] { 0, 30 };
            var stepNumberList = blobStepFilter.Select(step => (int)step).ToList();
            //var communicationLogStepRepository = new CommunicationLogStepRepository(tenant);

            List<CommunicationLogStep>  stepList = repository.GetMultiCommunicationLog(communicationLogId, tenant);
            //var stepList = GetCommunicationLogStepsListsByLogId(logId, tenant);
            List<CommunicationLogStep> filter = stepList.Where(rec => stepNumberList.Contains(rec.StepNumber)).ToList();
            var stepLIstOut = new List<CommunicationLogStepList>();
            if (serverRequest)
            {
                foreach (var step in stepList)
                {
                    var stepListVersion = CommunicationLogStepQuery.GetStepListVersion(step);

                    if (stepNumberList.Contains(step.StepNumber) 
                        ///&& step.Status=="D" 
                        )
                    {
                        if (GetBlob(tenant, step.Document, out ArryByte, suppressCache))
                        {
                            if (ArryByte != null)
                            {
                                string xml = Encoding.UTF8.GetString(ArryByte);
                                stepListVersion.DocumentData = xml;
                            }
                        }
                    }
                    stepLIstOut.Add(stepListVersion);
                }
                
            }
            else
            {
                foreach (var step in filter)
                {
                    
                    var stepListVersion = CommunicationLogStepQuery.GetStepListVersion(step);
                    if (GetBlob(tenant, step.Document, out ArryByte, suppressCache))
                    {


                        string xml = "";
                        if (ArryByte != null)
                        {
                            xml = Encoding.UTF8.GetString(ArryByte);
                        }
                        stepListVersion.DocumentData = xml;
                    }

                    stepLIstOut.Add(stepListVersion);


                }
            }
            
            return stepLIstOut;
        }
        public static bool GetBlob(int tenant, Document document, out byte[] ArryByte,bool suppressCache)
        {
            ArryByte = null;
            var stopwatch = Stopwatch.StartNew();


            string filename = document.Id + "." + document.Extension;
            string filePath = "tenant" + document.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);


            

            string entityKeyString = "GetBlob:" + filePath;

            if (suppressCache)
            {
                ArryByte = GetBlob_NoCache(tenant, document, stopwatch, filePath);
                return true;
            }
            ArryByte = CacheManager.GetOrInsertNewObject<byte[]>(entityKeyString, () =>
            {
                return GetBlob_NoCache(tenant, document, stopwatch, filePath);
            });

            return true;
        }

        public static byte[] GetBlob_NoCache(int tenant, Document document, Stopwatch stopwatch, string filePath)
        {
            byte[] myArryByte = null;
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            LogMessagingUtil.Instance.AppendLine("Try Read Bolb :" + filePath);

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,

            };
            myArryByte = storageservice.Read(fileInfo);

            stopwatch.Stop();
            LogMessagingUtil.Instance.AppendLine("getBolb:" + filePath + "Took:" + stopwatch.Elapsed.ToString());
            if (myArryByte == null)
            {
                LogMessagingUtil.Instance.AppendLine("Bolb is null! ");
                //LogMessagingUtil.Instance.AppendLine(response.ErrorMessage);
                //throw new Exception("Bolb is null!  " + response.ErrorMessage);
                return null;
            }

            return myArryByte;
        }

        public string  GetStartRequestParams(int tenant,string RequestComminicationId)
        {
            List<CommunicationLogStepList> stepList = null;
            
            var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            List<int> reqDataList = new List<int>();
            int StartRequestParams = 0;
            reqDataList.Add(StartRequestParams); //(int)CustomsStepEnum.StartRequestParams);
            stepList = communicationLogStepQuery
                .GetCommunicationLogStepsDocumentData(RequestComminicationId, tenant, reqDataList.ToArray(), true,false);

            var stepReq = stepList.FirstOrDefault(rec => rec.StepNumber == (int)StartRequestParams //CustomsStepEnum.StartRequestParams
            );
            if (stepReq != null)
            {
                var requestParamXml = stepReq.DocumentData;
                return requestParamXml;
            }
            return null;
        }

    }
}