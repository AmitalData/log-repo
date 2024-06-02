using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.StorageService
{
    public class DatabaseBlobService : IBlobService
    {
        public byte[] Read(BlobFileInfo fileInfo)
        {
            
            // decompress
            byte[] result = null;
            if (fileInfo.UnifreightFillingDownload(out result))
            {
                return result;
            }
            BlobFileRepository blobFileRepository = new BlobFileRepository(0);
            BlobFile file = blobFileRepository.GetSingleBlobFile(fileInfo.FileName);

            if (file != null && file.Blob != null)
            {
                if (file.IsCompressed)
                    result = ByteCompressor.Decompress(file.Blob);
                else
                    result = file.Blob;

            }
            return result;

        }

        public void Write(byte[] data, BlobFileInfo fileInfo)
        {
            if (fileInfo.UnifreightFillingUpload(data))
            {
                return;
            }
            BlobFileRepository blobFileRepository = new BlobFileRepository(0);
            BlobFile file = blobFileRepository.GetSingleBlobFile(fileInfo.FileName);

            if (file != null)
            {
                file.Blob = ByteCompressor.Compress(data);
                file.IsCompressed = true;
                blobFileRepository.Update(file);
            }
            else
            {
                file = new BlobFile()
                {
                    Id = fileInfo.FileName,
                    Blob = ByteCompressor.Compress(data),
                    IsCompressed = true,
                };

                blobFileRepository.Add(file);

            }

            blobFileRepository.SubmitChanges();


        }

        public void WriteBlock(byte[] buffer, long sentBytes, string[] blockIdsList, int bufferNumber, BlobFileInfo fileInfo)
        {
            if (fileInfo.UnifreightFillingUploadBlock(buffer))
            {
                return;
            }
            BlobFileRepository blobFileRepository = new BlobFileRepository(0);
            BlobFile file = blobFileRepository.GetSingleBlobFile(fileInfo.FileName);

            if (file != null)
            {
                if (file.Blob == null || (buffer != null && fileInfo.FileSize == buffer.Length))
                {
                    file.Blob = buffer;
                }
                else
                {
                    int i = file.Blob.Length;
                    byte[] blobData = file.Blob;
                    Array.Resize<byte>(ref blobData, i + buffer.Length);
                    buffer.CopyTo(blobData, i);

                    //if (sentBytes == fileSize)
                    //{
                    //    blobData = ByteCompressor.Compress(blobData);
                    //}

                    file.Blob = blobData;

                }

                blobFileRepository.Update(file);
            }
            else
            {
                file = new BlobFile()
                {
                    Id = fileInfo.FileName,
                    Blob = buffer
                };

                blobFileRepository.Add(file);

            }

            blobFileRepository.SubmitChanges();


        }

        public void Delete(BlobFileInfo fileInfo)
        {
            if (fileInfo.UnifreightFillingDelete())
            {
                return;
            }
            BlobFileRepository blobFileRepository = new BlobFileRepository(0);
            BlobFile file = blobFileRepository.GetSingleBlobFile(fileInfo.FileName);

            if (file != null)
            {
                blobFileRepository.Remove(file);
            }

            blobFileRepository.SubmitChanges();


        }

        public bool FileExists(BlobFileInfo fileInfo)
        {
            // decompress
            byte[] result = null;
            if (fileInfo.UnifreightFillingDownload(out result))
            {
                return (result != null);
            }
            BlobFileRepository blobFileRepository = new BlobFileRepository(0);
            BlobFile file = blobFileRepository.GetSingleBlobFile(fileInfo.FileName);

            return (file != null);

        }

        public void AppendText(string text, BlobFileInfo fileInfo)
        {
          
            if (!string.IsNullOrEmpty(text))
            {

                BlobFileRepository blobFileRepository = new BlobFileRepository(0);
                BlobFile file = blobFileRepository.GetSingleBlobFile(fileInfo.FileName);

                if (file != null)
                {

                    if (file.Blob == null)
                    {
                        var txtBytes = Encoding.UTF8.GetBytes(text);
                        file.Blob = txtBytes;
                    }
                    else
                    {
                        var fileText = Encoding.UTF8.GetString(file.Blob);
                        fileText += text;
                        var txtBytes = Encoding.UTF8.GetBytes(fileText);
                        file.Blob = txtBytes;
                        blobFileRepository.Update(file);
                    }
                }
                else
                {
                    var txtBytes = Encoding.UTF8.GetBytes(text);
                    file = new BlobFile()
                    {
                        Id = fileInfo.FileName,
                        Blob = txtBytes,
                    };

                    blobFileRepository.Add(file);

                }

                blobFileRepository.SubmitChanges();
            }
        }

        public void Dispose()
        {
          //  throw new NotImplementedException();
        }

        public void MoveFromAnotherStorage(string containerSASURI, string fileNameSource, BlobFileInfo destinationFileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
