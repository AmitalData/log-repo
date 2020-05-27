using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.StorageService
{
    public interface IBlobService
    {
        byte[] Read(BlobFileInfo fileInfo);
        void Write(byte[] data, BlobFileInfo fileInfo);
        void WriteBlock(byte[] buffer, long sentBytes, string[] blockIdsList, int bufferNumber, BlobFileInfo fileInfo);
        void Delete(BlobFileInfo fileInfo);
        bool FileExists(BlobFileInfo fileInfo);

        void AppendText(string text, BlobFileInfo fileInfo);

    }
}
