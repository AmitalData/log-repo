using Azure.Storage.Blobs;

namespace AmitalCloud.Infrastructure.Data.Azure
{
    public static class BlobExtensions
    {
        public static bool Exists(this BlobClient blob)
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