using FluentAssertions;
using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services
{
    public class DocumentFileService
    {
        const string FileName = "manage_users.pdf";
        const string FileExtension = "pdf";
        const int FileSize = 267607;
        public int Uploadfile(DocumentsFilingPM document)
        {
            var chunk1 = UploadChunk1(document);
            var chunk2 = UploadChunk2(document,chunk1);
            var chunk3 = UploadChunk3(document,chunk2);
            return chunk3.SentSize;
        }
        public DocumentsFilingPM UpdateDocumentWithFile(DocumentsFilingPM document)
        {
            document.HasFile = true;
            document.FileName = FileName;
            document.Received = true;
            document.ReceivedByUserId = UserTenant.UserId;
            document.ReceivedDate = DateTime.Now;
            document.UpdateDate = DateTime.Now;
            document.FileExtension = FileExtension;
            document.FileSize = FileSize;
            return APICaller.CallPut<DocumentsFilingPM>(document, Urls.DocumentsFilingsController, UserTenant.Token)?.Data;

        }
        public void AssertDocument(DocumentsFilingPM document)
        {
            document.FileName.Should().Be(FileName);
            document.HasFile.Should().Be(true);
        }

        private ImageParameter UploadChunk1(DocumentsFilingPM document)
        {
            var chunk1 = GetChunk1(document);
            var chunk1Result = APICaller.CallPost<ImageParameter>(chunk1, Urls.PostUploadFile, UserTenant.Token)?.Data;
            AssertChunk1(chunk1Result);
            return chunk1Result;
        }
        private void AssertChunk1(ImageParameter chunk)
        {
            chunk.Buffersize.Should().NotBe(0);
            chunk.BlockIdsList.Should().NotBeEmpty();
            chunk.BlockIdsList[0].Should().NotBeNull();
        }
        private ImageParameter UploadChunk2(DocumentsFilingPM document, ImageParameter chunk1)
        {
            var chunk2 = GetChunk2(document,chunk1);
            var chunk1Result = APICaller.CallPost<ImageParameter>(chunk2, Urls.PostUploadFile, UserTenant.Token)?.Data;
            AssertChunk2(chunk1Result);
            return chunk1Result;
        }

        

        private void AssertChunk2(ImageParameter chunk)
        {
            chunk.Buffersize.Should().NotBe(0);
            chunk.BlockIdsList.Should().NotBeEmpty();
            chunk.BlockIdsList[1].Should().NotBeNull();
        }
        private ImageParameter UploadChunk3(DocumentsFilingPM document, ImageParameter chunk2)
        {
            var chunk3 = GetChunk3(document,chunk2);
            var chunk1Result = APICaller.CallPost<ImageParameter>(chunk3, Urls.PostUploadFile, UserTenant.Token)?.Data;
            AssertChunk3(chunk1Result);
            return chunk1Result;
        }

        

        private void AssertChunk3(ImageParameter chunk)
        {
            chunk.SentSize.Should().NotBe(0);
            chunk.FileSize.Should().NotBe(0);
        }
        private ImageParameter GetChunk1(DocumentsFilingPM document)
        {
            return new ImageParameter()
            {
                IsFirstTry = true,
                Tenant = UserTenant.Tenant,
                Extension = FileExtension,
                EntityId = document.Id,
                FileSize = FileSize,
                BufferNumber = -1,
                UploadMode = UploadModes.AttachmentUploader,
                SentSize = 0,
                FileName = FileName,
                Base64String = ReadFilebyName("Chunk1.txt")
            };
        }
        private ImageParameter GetChunk2(DocumentsFilingPM document, ImageParameter chunk1)
        {
            return new ImageParameter()
            {
                IsFirstTry = true,
                Tenant = UserTenant.Tenant,
                Extension = FileExtension,
                EntityId = document.Id,
                FileSize = FileSize,
                BufferNumber = 0,
                UploadMode = UploadModes.AttachmentUploader,
                SentSize = chunk1.SentSize,
                FileName = FileName,
                Base64String = ReadFilebyName("Chunk2.txt"),
                BlocksNumber = 3,
                EncodedFileName = chunk1.EncodedFileName,
                BlockIdsList = new List<string>() { chunk1.BlockIdsList[0] }
            };
        }
        private ImageParameter GetChunk3(DocumentsFilingPM document,ImageParameter chunk2)
        {
            return new ImageParameter()
            {
                IsFirstTry = true,
                Tenant = UserTenant.Tenant,
                Extension = FileExtension,
                EntityId = document.Id,
                FileSize = FileSize,
                BufferNumber = 1,
                UploadMode = UploadModes.AttachmentUploader,
                SentSize = chunk2.SentSize,
                FileName = FileName,
                Base64String = ReadFilebyName("Chunk3.txt"),
                BlocksNumber = 3,
                EncodedFileName = chunk2.EncodedFileName,
                BlockIdsList = new List<string>() { chunk2.BlockIdsList[0], chunk2.BlockIdsList[1] },
                Position = 0,
            };
        }

        public string ReadFilebyName(string fileName)
        {
            var path = "./Data/" + fileName;
            return File.ReadAllText(path);
        }
    }
}
