using Logitude.Workflow.Data.EntityLists;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Logitude.Workflow.Data.WorkflowStorage
{
    public class WorkflowInstanceStorage : WorkflowAzureStorage
    {
        public WorkflowInstanceStorage() : base(WorkflowStorageContainers.WorkflowInstances){}

        public List<WorkFlowInstanceActivityList> GetActivities(string workflowInstanceId, int tenant)
        {
            try
            {
                if (!string.IsNullOrEmpty(workflowInstanceId))
                {
                    string zipFileName = string.Format("{0}.{1}.zip", workflowInstanceId, tenant);
                    string jsonFileName = string.Format("{0}.{1}.json", workflowInstanceId, tenant);
                    byte[] blobBytes = GetBlobBytes(zipFileName);
                    if (blobBytes != null)
                    {





                        using (var zippedStream = new MemoryStream(blobBytes))
                        {
                            using (var archive = new ZipArchive(zippedStream, ZipArchiveMode.Read))
                            {
                                var entry = archive.Entries.FirstOrDefault(x => x.Name.ToLower() == jsonFileName.ToLower());

                                if (entry != null)
                                {
                                    using (var unzippedEntryStream = entry.Open())
                                    {
                                        using (var ms = new MemoryStream())
                                        {
                                            unzippedEntryStream.CopyTo(ms);

                                            var test = Encoding.ASCII.GetString(ms.ToArray());


                                        }
                                    }
                                }

                                return null;
                            }
                        }




                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}