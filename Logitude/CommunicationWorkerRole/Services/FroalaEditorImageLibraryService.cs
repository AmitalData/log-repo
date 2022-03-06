using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services
{
    public class FroalaEditorImageLibraryService
    {
        private EmailParameters emailParameters;
        private List<LinkedResource> linkedResources;
        private string emailBody;
        public FroalaEditorImageLibraryService(EmailParameters emailParameters)
        {
            this.emailParameters = emailParameters;
            this.emailBody = emailParameters.Body;
            this.linkedResources = new List<LinkedResource>();
        }

        public string BuildMailBody()
        {
            if (string.IsNullOrEmpty(emailParameters.EmailView) || emailBody == null || !emailParameters.IsBodyHtml) return emailBody;

            List<string> images = emailBody.Split('<').Where(s => s.StartsWith("img") && s.Contains("href='imagelibrary-")).ToList();
            if (!images.Any()) return emailBody;

            foreach (var image in images) BuildImageLibrary(image);
            return emailBody;
        }

        public List<LinkedResource> GetLinkedResources()
        {
            return linkedResources;
        }

        private void BuildImageLibrary(string imageTag)
        {
            string imageName = GetBetween(imageTag, "securityId=", "&");
            AddImageLinkedResource(imageTag, imageName);
            UpdateBodyImageTag(imageTag, imageName);
        }

        private void AddImageLinkedResource(string imageTag, string imageName)
        {
            LinkedResource linkedResource = BuildImageLinkedResource(imageName, imageTag);
            this.linkedResources.Add(linkedResource);
        }

        private LinkedResource BuildImageLinkedResource(string imageName, string imageTag)
        {
            string extension = GetBetween(imageTag, "imagelibrary-", "'");
            string imageSrc = GetBetween(imageTag, "src='", "'");

            byte[] imageBytes = new WebClient().DownloadData(imageSrc);
            MemoryStream memstream = new MemoryStream(imageBytes);
            memstream.Seek(0, SeekOrigin.Begin);

            LinkedResource logo = new LinkedResource(memstream, "image/" + extension);
            logo.ContentId = imageName;
            logo.ContentType.Name = imageName + "." + extension;
            return logo;
        }

        private void UpdateBodyImageTag(string imageTag, string imageName)
        {
            string imageSrc = GetBetween(imageTag, "src='", "'");
            string imageHref = GetBetween(imageTag, "href='", "'");

            string newImageTag = imageTag.Replace("href='" + imageHref + "'", "");
            newImageTag = newImageTag.Replace(imageSrc, "cid:" + imageName);
            newImageTag = AddWidthHeightToImageTag(newImageTag);
            emailBody = emailBody.Replace(imageTag, newImageTag);
        }

        private string AddWidthHeightToImageTag(string imageTag)
        {
            var style = GetBetween(imageTag, " style='", "'");
            var width = "width='" + GetBetween(style, "width:", "px;").Trim() + "'";
            var height = "height='" + GetBetween(style, "height:", "px;").Trim() + "'";

            return imageTag.Replace("img  src", "img " + width + " " + height + " src");
        }

        private string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (!strSource.Contains(strStart) || !strSource.Contains(strEnd)) return null;

            int start = strSource.IndexOf(strStart, 0) + strStart.Length;
            int end = strSource.IndexOf(strEnd, start);
            string output = strSource.Substring(start, end - start);
            return string.IsNullOrEmpty(output) ? null : output;
        }


    }
}
