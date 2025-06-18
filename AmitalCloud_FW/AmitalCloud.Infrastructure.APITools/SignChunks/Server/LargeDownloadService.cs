//using FileUploadDemoServer.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.APITools.Sign
{
    public partial class LargeDownloadService
    {
        const int DEFAULT_ChunkSize =
            100 * 1024;
        private static List<BlobChunks> DownloadBlobFileSet = new List<BlobChunks>();


        public BlobChunks DownloadBlobChunksAsync(Guid BlobFileId, int ChunkId)
        {
            BlobChunks curr = null;
            lock (DownloadBlobFileSet)
            {
                curr = DownloadBlobFileSet.First(rec => rec.BlobFileId == BlobFileId && rec.ChunkId == ChunkId);
                DownloadBlobFileSet.Remove(curr);
            }
            return curr;
        }
        public static IEnumerable<BlobChunks> GetFileChunkAsync(String file)
        {
            try
            {
                using (var resource = new System.IO.FileStream(
//@"C:\PerformanceTest Software Downloadpetst.exe"
file
, System.IO.FileMode.Open))
                {
                    return GetMyChunks(resource, Guid.NewGuid());

                }



            }
            catch (Exception)
            {

                throw;
            }

        }


        public static IEnumerable<BlobChunks> GetMyChunks(Stream myStream, Guid guid)
        {

            var list = new List<BlobChunks>();
            byte[] buffer = new byte[DEFAULT_ChunkSize];
            int numRead;
            int chkIdx = 0;
            //guid = guid ?? Guid.NewGuid();
#if false            
            using (var resource = 
                //new System.IO.FileStream(
File.OpenWrite(@"C:\PerformanceTest Software Downloadpetst-CopyServer.exe"
//, System.IO.FileMode.CreateNew
))

            
#endif
            {



                BlobChunks cur = null;
                while ((numRead = myStream.Read(buffer, 0, buffer.Length)) != 0)
                {

                    cur = new BlobChunks()
                    {
                        BlobFileId = guid,
                        ChunkId = chkIdx,
                        //Data =  buffer,
                        Length = numRead

                    };
                    var ms = new MemoryStream();
                    ms.Write(buffer, 0, numRead);
                    cur.Data = ms.ToArray();

                    list.Add(cur);
                    //resource.Write(buffer, 0, numRead);
                    chkIdx++;
                }


            }
            return list;
        }







        internal static void AddDownloadBlobFileSet(
            IEnumerable<BlobChunks> allChunks
            //List<BlobChunks> all
            )
        {
            var created = DateTime.Now;
            lock (DownloadBlobFileSet)
            {
                var have2del = new List<BlobChunks>(DownloadBlobFileSet
                    .Where(rec => DateTime.Now.Subtract(rec.CreateAt) > TimeSpan.FromMinutes(5)));
                foreach (var curr in have2del)
                {
                    DownloadBlobFileSet.Remove(curr);
                }
                foreach (var item in allChunks)
                {
                    item.CreateAt = created;
                    DownloadBlobFileSet.Add(item);
                }

            }


        }


    }
}
