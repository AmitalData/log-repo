using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
  public  class EncodedHtmlHelper
    {
        public string EncodedHtmlScript(string htmlstring)
        {
          //  htmlstring = "<html><style type='text/css' style= 'display: none; '></style></head><body><style type='text/css' style='display: none; '></style><a  href='https://www.google.com/' onclick=" + '"' + "alert('test')" + '"' + " style='font-size: 16px; word-wrap: break-word;'>xss</a></body></html>"; ;
            // htmlstring = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body><script>alert('test')</script></html>";
            //htmlstring = "<html><head> <meta http-='' equiv='Content- Type' content='text/html; charset = iso-8859-1'>  <style type='text/css' style= 'display: none; '></style></head><body><style type='text/css' style='display: none; '></style><a  href='' onclick1=" + '"' + "alert('test')" + '"' + " style='font-size: 16px; word-wrap: break-word;'>xss</a></body></html>";
            //if (!string.IsNullOrEmpty(htmlstring))
            //{
            //    while (htmlstring.Contains("<script") && htmlstring.Contains("</script>"))
            //    {
            //        var scripte = "<script" + getBetween(htmlstring, "<script", "</script>") + "</script>";
            //        string encodedString = System.Web.HttpUtility.HtmlEncode(scripte);

            //        htmlstring = htmlstring.Replace(scripte, encodedString);

            //    }
            //}
            //else htmlstring = "";

            return htmlstring;

        }


        public string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
