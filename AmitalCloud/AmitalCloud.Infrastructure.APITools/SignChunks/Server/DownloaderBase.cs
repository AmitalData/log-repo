using AmitalCloud.Infrastructure.APITools.BaseClasses;
using AmitalCloud.Infrastructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.IO;


namespace AmitalCloud.Infrastructure.APITools.Sign
{
    public abstract class DownloaderBase<ReqData, ResData>
        where ReqData : ReqDataBase, new()
        where ResData : RestDataDownloaderBase, new()
    {
        protected ResData _ResData;
        protected ReqData _ReqData;
        private string _ServerMD5Hash;

        public DownloaderBase()
        {
            _ResData = new ResData();
        }

        public ResData Doit(ReqData reqData)
        {
            _ReqData = reqData;

            Stream myStream = GetMyStream(reqData);
            if (myStream != null)
            {
                RegisterBytesToDownload(myStream);
            }

            return ManipulateResponse(_ResData); ;
        }

        protected virtual ResData ManipulateResponse(ResData resData)
        {
            return resData;
        }

        protected virtual Stream GetMyStream(ReqData reqData)
        {
            throw new NotImplementedException();
        }




        public void RegisterBytesToDownload(Stream myStream)
        {


            _ResData.ServerMD5Hash = MD5HashUtil.GetMD5Hash((myStream as MemoryStream).ToArray());

            var all = LargeDownloadService.GetMyChunks(myStream, Guid.NewGuid());
            MakeBlobFileChunksEnum(all);
        }

        private void MakeBlobFileChunksEnum(IEnumerable<BlobChunks> all)
        {

            bool _1stInit = false; ;
            int i = 0;
            foreach (var item in all)
            {
                var cur = new BlobChunks()
                {
                    BlobFileId = item.BlobFileId,
                    ChunkId = item.ChunkId,
                    Length = item.Length
                };
                if (!_1stInit)
                {
                    _1stInit = true;
                    cur.Data = item.Data;
                }
                i++;
                _ResData.BlobChunksOnly1stWithData.Add(cur);

            }
            if (i < 2) return;
            LargeDownloadService.AddDownloadBlobFileSet(all);


        }

    }

}
