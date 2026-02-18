using AmitalCloud.Infrastructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AmitalCloud.Infrastructure.APITools.Sign
{
    public class LargeUploadService
    {
        private static List<BlobFile> UploadBlobFileSet = new List<BlobFile>();



        public static void AddUploadBlobFileSet(BlobFile blobFile)
        {
            var created = DateTime.Now;
            lock (UploadBlobFileSet)
            {
                var have2del = new List<BlobFile>(UploadBlobFileSet
                        .Where(rec => DateTime.Now.Subtract(rec.CreatedOn.GetValueOrDefault()) > TimeSpan.FromMinutes(5)));

                foreach (var curr in have2del)
                {
                    UploadBlobFileSet.Remove(curr);
                }
                blobFile.BlobFileId = Guid.NewGuid();
                blobFile.CreatedOn = created;
                blobFile.Chunks = blobFile.Chunks ?? new List<BlobChunks>();

                UploadBlobFileSet.Add(blobFile);
            }
            if (blobFile.BlobChunks1st != null)
            {

                AddUploadBlobFileChunkAsync(blobFile.BlobFileId, 0, blobFile.BlobChunks1st);

                //blobFile.CurrentTotalSize =  
            }
        }
        public static void AddUploadBlobFileChunkAsync(Guid blobFileId, int chunkId, byte[] data)
        {
            var chunk = new BlobChunks
            {
                BlobFileId = blobFileId,
                ChunkId = chunkId,
                Length = data.Length,
                Data = data
            };

            lock (UploadBlobFileSet)
            {
                var blobFile = UploadBlobFileSet.Single(rec => rec.BlobFileId == blobFileId);
                blobFile.CurrentTotalSize += chunk.Length;
                blobFile.Chunks.Add(chunk);
#if false
                if (blobFile.CurrentTotalSize ==blobFile.SizeOnClient)
                {
                    var filePath = Path.Combine("c:", blobFile.Name);
                    using (var outputStream = File.OpenWrite
                        (
                        ///@"D:\file2.txt"
filePath
))
                    {
                        foreach (var item in blobFile.Chunks.OrderBy(x => x.ChunkId))
                        {
                            outputStream.Write(item.Data, 0, item.Length);
                        }
                    }
                }
#endif
            }
        }

        public static byte[] FinishUploadFileAsync(Guid blobFileId)
        {
            List<int> chunksId;
            List<BlobChunks> sortedChunks;
            string MD5HashClient = "";
            lock (UploadBlobFileSet)
            {
                var blobFile = UploadBlobFileSet.Single(rec => rec.BlobFileId == blobFileId);

                if (blobFile.CurrentTotalSize != blobFile.SizeOnClient)
                {
                    throw new Exception("blobFile.TotalSize != blobFile.Size");
                }
                MD5HashClient = blobFile.MD5HashClient;
                sortedChunks = blobFile.Chunks.ToList().OrderBy(x => x.ChunkId).ToList();

                UploadBlobFileSet.Remove(blobFile);

            }




            var outputStream = new MemoryStream();
            foreach (var item in sortedChunks)
            {
                outputStream.Write(item.Data, 0, item.Length);
            }
            if (MD5HashClient != MD5HashUtil.GetMD5Hash(outputStream.ToArray()))
            {
                throw new Exception("MD5Hash Client != server");
            }
            return outputStream.ToArray();
            //var result = 0;
            //            using (var stream = new MultiStream(
            //sortedChunks.Select(chunk => new MemoryStream(chunk.Data))
            //                ))
            //            {


            //                //foreach (var chunk in stream.GetByteChunks(1024))
            //                //    result = (result * 31) ^ ComputeHash(chunk);
            //            }

            //            return result;
            //string.Format("File Hash Code is: {0}", result);
        }



        private static int ComputeHash(params byte[] data)
        {
            unchecked
            {
                var result = 0;
                foreach (var b in data)
                    result = (result * 31) ^ b;
                return result;
            }
        }


    }

}
