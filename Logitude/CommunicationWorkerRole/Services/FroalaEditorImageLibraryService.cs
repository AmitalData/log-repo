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
        public FroalaEditorImageLibraryService(EmailParameters emailParameters)
        {
            this.emailParameters = emailParameters;
            this.linkedResources = new List<LinkedResource>();
        }

        public string BuildMailBody()
        {
            if (string.IsNullOrEmpty(emailParameters.EmailView) || emailParameters.Body == null || !emailParameters.IsBodyHtml) return emailParameters.Body;

            List<string> images = emailParameters.Body.Split('<').Where(s => s.StartsWith("img") && s.Contains("href='imagelibrary-")).ToList();
            if (!images.Any()) return emailParameters.Body;

            foreach (var image in images) BuildImageLibrary(image);
            return emailParameters.Body;
        }

        public List<LinkedResource> GetLinkedResources()
        {
            return linkedResources;
        }

        private void BuildImageLibrary(string imageTag)
        {
            string imageName = GetBetween(imageTag, "securityId=", "&");
            BuildImageLinkedResource(imageTag, imageName);
            UpdateMainMessageBodyImageTag(imageTag, imageName);
        }

        private void BuildImageLinkedResource(string imageTag, string imageName)
        {
            LinkedResource linkedResource = GetImageLinkedResource(imageName, imageTag);
            this.linkedResources.Add(linkedResource);
        }

        private LinkedResource GetImageLinkedResource(string imageName, string imageTag)
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

        private void UpdateMainMessageBodyImageTag(string imageTag, string imageName)
        {
            string imageSrc = GetBetween(imageTag, "src='", "'");
            string imageHref = GetBetween(imageTag, "href='", "'");

            string newImageTag = imageTag.Replace("href='" + imageHref + "'", "");
            newImageTag = newImageTag.Replace(imageSrc, "cid:" + imageName);
            newImageTag = AddWidthHeightToImageTag(newImageTag);
            emailParameters.Body = emailParameters.Body.Replace(imageTag, newImageTag);
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

            int Start = strSource.IndexOf(strStart, 0) + strStart.Length;
            int End = strSource.IndexOf(strEnd, Start);
            string output = strSource.Substring(Start, End - Start);
            return string.IsNullOrEmpty(output) ? null : output;
        }


    }
}
