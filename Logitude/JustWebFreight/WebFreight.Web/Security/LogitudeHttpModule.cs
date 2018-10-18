using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Threading;
using Simplog.Server.Infrastructure;
using System.Web.Http;

namespace WebFreight.Web.Security
{
    public class LogitudeHttpModule : IHttpModule
    {
       
        private StreamWatcher watcher;
        HttpApplication context;
        public void Init(HttpApplication theContext)
        {
            this.context = theContext;
            theContext.EndRequest += new EventHandler(context_EndRequest);
            
            theContext.BeginRequest += (o, e) =>
            {
                if (theContext.Request.CurrentExecutionFilePathExtension == ".svc")
                {
                    if (theContext.Request.PathInfo.ToUpper().Contains("GET"))
                    {
                        if (theContext.Request.QueryString["xapname"] == null)
                        {
                            watcher = new StreamWatcher(theContext.Response.Filter);
                            theContext.Response.Filter = watcher;
                        }
                    }
                }
                
            };

            theContext.AuthenticateRequest += new EventHandler(theContext_AuthenticateRequest);
        }

      

        void theContext_AuthenticateRequest(object sender, EventArgs e)
        {
            
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            
                string xapname = context.Request.QueryString["xapname"];
                if (xapname != null)
                {
                    
                    this.context.Response.Headers.Remove("Cache-Control");
                    this.context.Response.Headers.Remove("X-AspNet-Version");
                  


                }

            
        }

         

        public void Dispose()
        {

        }

      
    }

    #region BinaryXMLReader


    public class BinaryXMLReader
    {
        //private static readonly XmlDictionary _wcfBinaryDictionary = WcfBinaryDictionary.CreateWcfBinaryDictionary();


        public static void CheckDataSecurity(byte[] bytes)
        {

            //string responseXml = "";
            //try
            //{
            //    responseXml = GetWcfBinaryMessageAsText(bytes);
            //}
            //catch (Exception ex)
            //{

            //}
            //if (!string.IsNullOrEmpty(responseXml))
            //{

            //    //if (responseXml.Contains("IsSecured"))
            //    //{
            //    //    Locate(bytes, isSecuredbytes);
            //    //}



            //    if (responseXml.Contains("IsSecured>false<"))
            //    {
            //        //throw new Exception("You have no permission to access this data");
            //    }


            //}
            
            byte[] isSecuredbytes = new byte[] { 73, 115, 83, 101, 99, 117, 114, 101, 100, 133 };
            if (SearchBytePattern(bytes, isSecuredbytes) > 0)
            {
                throw new AutenticationException("You have no permission to access this data");
            }

          
            //string mobileVersion = HttpContext.Current.Request.Headers["MobileVersion"];
            //if (!string.IsNullOrEmpty(mobileVersion))
            //{
            //    double version = 0;
            //    if (double.TryParse(mobileVersion, out version))
            //    {
            //        if (version < LogitudeSettings.IOSSharedAppMinimumVersion)
            //        {
            //            throw new Exception("Your application version is out-of-date. Please upgrade your application to the latest version");
            //        }
            //    }
            //}


        }


        #region SimpleBoyerMooreSearch
        
       
        static int SearchBytePattern(byte[] haystack, byte[] needle)
        {
            int[] lookup = new int[256];
            for (int i = 0; i < lookup.Length; i++) { lookup[i] = needle.Length; }

            for (int i = 0; i < needle.Length; i++)
            {
                lookup[needle[i]] = needle.Length - i - 1;
            }

            int index = needle.Length - 1;
            var lastByte = needle.Last();
            while (index < haystack.Length)
            {
                var checkByte = haystack[index];
                if (haystack[index] == lastByte)
                {
                    bool found = true;
                    for (int j = needle.Length - 2; j >= 0; j--)
                    {
                        if (haystack[index - needle.Length + j + 1] != needle[j])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                        return index - needle.Length + 1;
                    else
                        index++;
                }
                else
                {
                    index += lookup[checkByte];
                }
            }
            return -1;
        }


        #endregion
  
        //#region GetWcfBinaryMessageAsText
        
       
        //private static string GetWcfBinaryMessageAsText(byte[] encodedMessage)
        //{
        //    var document = LoadMessageIntoDocument(encodedMessage);
        //    var decodedMessage = WriteDocumentToString(document);

        //    return decodedMessage;
        //}

        //private static string WriteDocumentToString(XmlDocument document)
        //{
        //    var stringWriter = new StringWriter();

        //    using (var xmlWriter = new XmlTextWriter(stringWriter))
        //    {
        //        xmlWriter.Formatting = Formatting.Indented;
        //        xmlWriter.Indentation = 1;

        //        document.WriteTo(xmlWriter);
        //    }

        //    return stringWriter.ToString();
        //}

        //private static XmlDocument LoadMessageIntoDocument(byte[] encodedMessage)
        //{
        //    using (var reader = CreateReaderForMessage(encodedMessage))
        //    {
        //        var document = new XmlDocument();
        //        document.Load(reader);
        //        return document;
        //    }
        //}

        //private static XmlDictionaryReader CreateReaderForMessage(byte[] encodedMessage)
        //{
        //    return XmlDictionaryReader.CreateBinaryReader(encodedMessage,
        //                                                  0,
        //                                                  encodedMessage.Length,
        //                                                  _wcfBinaryDictionary,
        //                                                  XmlDictionaryReaderQuotas.Max);
        //}

        //public bool bDirty
        //{
        //    get { return false; }
        //}

        //public bool bReadOnly
        //{
        //    get { return true; }
        //    set { }
        //}

        //#endregion

    }

    #endregion

    #region StreamWatcher
    
    
    public class StreamWatcher : Stream
    {
        private Stream _base;
        private MemoryStream memoryStream = new MemoryStream();
       
        XmlReader xmlReader;
        BinaryXMLReader binaryXmlReader = new BinaryXMLReader();
        public StreamWatcher(Stream stream)
        {
            _base = stream;
        }

        public override void Flush()
        {
            _base.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _base.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {

            BinaryXMLReader.CheckDataSecurity(buffer);

            memoryStream.Write(buffer, offset, count);
            _base.Write(buffer, offset, count);
        }

        public override string ToString()
        {
             
            if (xmlReader != null)
            {
                return xmlReader.ToString();
            }
            else
            {
                return "";
            }
        }

        #region Rest of the overrides
        public override bool CanRead
        {
            get {return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(long value)
        {
            //throw new NotImplementedException();
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get
            {
                return 0;
            }
            set
            {
                //throw new NotImplementedException();
            }
        }
        #endregion
    }


    #endregion

  
}
