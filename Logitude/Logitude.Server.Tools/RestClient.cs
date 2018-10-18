using System;
using System.IO;
using System.Net;
using System.Windows;
 
using System.Windows.Input;
 
using Newtonsoft.Json;

namespace Logitude.Server.Tools
{
    public class RestClient
    {

        private readonly Uri _baseAddress;
        public RestClient(string baseAddress)
        {
            _baseAddress = new Uri(baseAddress);
        }
        public void Get<T>(string commandPath, Action<Exception, T> callback)
        {
            var client = new WebClient();
            client.Headers[HttpRequestHeader.Accept] = "application/json";

            DownloadStringCompletedEventHandler handler = null;
            handler = (s, e) =>
            {
                client.DownloadStringCompleted -= handler;
                if (e.Error != null)
                {
                    callback(e.Error, default(T));
                    return;
                }
                T result = Deserialize<T>(new StringReader(e.Result));

                //if (result != null)
                //{
                //    EntityPM<T> pm = result as EntityPM<T>;
                //    if (pm != null)
                //    {
                //        T originalEntity = Deserialize<T>(new StringReader(e.Result));

                //        pm.OriginalEntity = originalEntity;
                //        pm.ChangeSetOperation = ChangeSetOperations.None;
                //    }

                //}

                callback(null, result);
            };
            client.DownloadStringCompleted += handler;
            client.DownloadStringAsync(new Uri(_baseAddress, commandPath));
        }

        public void Post<T>(string commandPath, object data, Action<Exception, T> callback)
        {
            var synchContext = System.Threading.SynchronizationContext.Current;
            var request = WebRequest.Create(new Uri(_baseAddress, commandPath));
            request.Method = "POST";
            request.ContentType = "application/json";
            request.BeginGetRequestStream(iar =>
            {
                var reqStr = request.EndGetRequestStream(iar);
                SerializeObject(data, reqStr);
                request.BeginGetResponse(iar2 =>
                {
                    WebResponse response;
                    try
                    {
                        response = request.EndGetResponse(iar2);
                    }
                    catch (Exception ex)
                    {
                        synchContext.Post((state) => callback((Exception)state, default(T)), ex);
                        return;
                    }
                    var result = Deserialize<T>(new StreamReader(response.GetResponseStream()));
                    synchContext.Post((state) => callback(null, (T)state), result);
                }, null);
            }, null);
        }

        public void Put<T>(string commandPath, object data, Action<Exception, T> callback)
        {
            var synchContext = System.Threading.SynchronizationContext.Current;
            var request = WebRequest.Create(new Uri(_baseAddress, commandPath));
            request.Method = "PUT"; request.ContentType = "application/json";
            request.BeginGetRequestStream(iar =>
            {
                var reqStr = request.EndGetRequestStream(iar);
                SerializeObject(data, reqStr);
                request.BeginGetResponse(iar2 =>
                {
                    WebResponse response;
                    try
                    {
                        response = request.EndGetResponse(iar2);
                    }
                    catch (Exception ex)
                    {
                        synchContext.Post((state) => callback((Exception)state, default(T)), ex);
                        return;
                    }
                    var result = Deserialize<T>(new StreamReader(response.GetResponseStream()));
                    synchContext.Post((state) => callback(null, (T)state), result);
                }, null);
            }, null);
        }

        private static void SerializeObject(object data, Stream stream)
        {

            ///********************************************************************
            var serializer = new JsonSerializer();
            var sw = new StreamWriter(stream);
            serializer.Serialize(sw, data);

            sw.Flush();
            sw.Close();


            ////**********************************************************
            //var serializer = new JsonSerializer();
            //MemoryStream memStream = new MemoryStream();
            //var sw = new StreamWriter(memStream);
            //serializer.Serialize(sw, data);

            //string datastring = Encoding.UTF8.GetString(memStream.ToArray(), 0, (int)memStream.Length);
            //byte[] result = CompressString(memStream.ToArray());

            //string compressedString = Encoding.UTF8.GetString(result, 0, result.Length);

            //var sw2 = new StreamWriter(stream);
            //serializer.Serialize(sw2, result);




            //int length = (int)sw2.BaseStream.Length;
            //byte[] mm = new byte[length];
            //sw2.BaseStream.Read(mm, 0, length);

            ////byte[] xx = ObjectToByteArray(data);
            ////byte[] result = CompressString(xx);


            ////var serializer = new JsonSerializer();
            ////var sw = new StreamWriter(stream);
            ////serializer.Serialize(sw, result);

            //sw.Flush();
            //sw.Close();

            //sw2.Flush();
            //sw2.Close();

        }

        private static T Deserialize<T>(TextReader reader)
        {
            var serializer = new JsonSerializer();
            return serializer.Deserialize<T>(new JsonTextReader(reader));
        }

        //private static byte[] ObjectToByteArray(Object obj)
        //{
        //    MemoryStream ms = new MemoryStream();

        //    // Serializer the User object to the stream.
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
        //    ser.WriteObject(ms, obj);
        //    byte[] json = ms.ToArray();

        //    return json;
        //}

      

        //public static byte[] CompressString(byte[] mybytearraydata)
        //{
        //    ZipFile file = new ZipFile(new MemoryStream());

        //    MemoryStream outputstream = new MemoryStream();
        //    using (ZipOutputStream zipStreamOut = new ZipOutputStream(outputstream))
        //    {
        //        zipStreamOut.PutNextEntry(new ZipEntry("arbitrary.ext"));
        //        zipStreamOut.Write(mybytearraydata, 0, mybytearraydata.Length);
        //        zipStreamOut.Finish();
        //        //Line below needed if outputstream is a MemoryStream and you are
        //        //passing it to a function expecting a stream.
        //        outputstream.Position = 0;

        //        //DoStuff.  Optional; Not necessary if e.g., outputstream is a FileStream.
        //        return outputstream.ToArray();

        //    }





        //}


        //static void UnZipFiles(byte[] dataBytes)
        //{
        //    ZipInputStream unzipInstance = new ZipInputStream(new MemoryStream(dataBytes, 0, dataBytes.Length));
        //    ZipEntry theEntry;
        //    while ((theEntry = unzipInstance.GetNextEntry()) != null)
        //    {
        //        using (MemoryStream mem = new MemoryStream())


        //            while (true)
        //            {
        //                int size = unzipInstance.Read(dataBytes, 0, dataBytes.Length);
        //                if (size > 0)
        //                {
        //                    mem.Write(dataBytes, 0, size);
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //    }
        //}


        


    }





} 
