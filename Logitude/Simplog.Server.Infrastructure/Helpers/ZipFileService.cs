using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;


namespace Simplog.Server.Infrastructure.Helpers
{
    public class ZipFileService
    {
        public ZipFileService()
        {

        }

        public static byte[] Compress(Dictionary<string, byte[]> data)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3);
            foreach (string key in data.Keys)
            {
                AddEntryToZipFile(data, zipStream, key);
            }

            zipStream.IsStreamOwner = false;
            zipStream.Close();
            outputMemStream.Position = 0;

            return outputMemStream.ToArray();

        }

        private static void AddEntryToZipFile(Dictionary<string, byte[]> data, ZipOutputStream zipStream, string key)
        {
            byte[] bytes;
            var newEntry = new ZipEntry(key + ".json");
            newEntry.DateTime = DateTime.Now;

            zipStream.PutNextEntry(newEntry);

            bytes = data[key];

            MemoryStream inStream = new MemoryStream(bytes);
            long inStreamLength = inStream.Length;
            if (inStreamLength < 200)
            {
                inStreamLength = 200;
            }

            StreamUtils.Copy(inStream, zipStream, new byte[inStreamLength]);
            inStream.Close();
            zipStream.CloseEntry();
        }

        public static Dictionary<string , byte[]> Extract(byte[] data)
        {
            Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();
            MemoryStream outputMemStream = new MemoryStream(data);

            using (var zip = new ZipArchive(outputMemStream, ZipArchiveMode.Read))
            {
                result = GetZipFileEntries(zip);
            }
            return result;
        }

        private static Dictionary<string, byte[]> GetZipFileEntries(ZipArchive zip)
        {
            Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();
            foreach (var entry in zip.Entries)
            { 
                result.Add(entry.FullName, GetEntryData(entry));
            }
            return result;
        }

        private static byte[] GetEntryData(ZipArchiveEntry entry)
        {
            using (var stream = entry.Open())
            {
                byte[] bytes;
                bytes = GetEntryBytes(stream);
                return bytes;
            }
        }

        private static byte[] GetEntryBytes(Stream stream)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bytes = ms.ToArray();
            }

            return bytes;
        }

    }
}
