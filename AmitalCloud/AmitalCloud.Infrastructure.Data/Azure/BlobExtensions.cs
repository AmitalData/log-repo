using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace AmitalCloud.Infrastructure.Data.Azure
{
    public static class BlobExtensions
    {
        public static bool Exists(this CloudBlockBlob blob)
        {
            try
            {
                blob.FetchAttributes();
                return true;
            }
            catch (StorageException e)
            {
                if (e.RequestInformation.HttpStatusMessage == "The specified blob does not exist.")
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}