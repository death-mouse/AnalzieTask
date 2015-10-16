using System;
using System.Text;
using System.Net;
using System.IO;


namespace AnalizeTask.need
{
    public class Browser
    {
        public Browser()
        {
            Cookies = new CookieContainer();
            Randomize();
        }


        private CookieContainer Cookies;


        public string GET(string url, Encoding _encoding, Boolean needHeaders = false)
        {
            try
            {
                HttpWebRequest hwrq = CreateRequest(url);
                hwrq.Credentials = new NetworkCredential(Properties.Settings.Default["Login"].ToString(), Properties.Settings.Default["Password"].ToString(), Properties.Settings.Default["Domain"].ToString());
                hwrq.CookieContainer = Cookies;
                using (HttpWebResponse hwrs = (HttpWebResponse)hwrq.GetResponse())
                {
                    Cookies.Add(hwrs.Cookies);
                    using (StreamReader sr = new StreamReader(hwrs.GetResponseStream(), _encoding))
                    {
                        if (needHeaders)
                            return hwrs.Headers.ToString() + sr.ReadToEnd().Trim();
                        else
                            return sr.ReadToEnd().Trim();
                    }
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }


        public string POST(string url, string query)
        {
            HttpWebRequest hwrq = CreateRequest(url);
            hwrq.CookieContainer = Cookies;
            hwrq.Method = "POST";
            hwrq.ContentType = "application/x-www-form-urlencoded";
            byte[] data = Encoding.UTF8.GetBytes(query);
            hwrq.ContentLength = data.Length;
            hwrq.GetRequestStream().Write(data, 0, data.Length);
            using (HttpWebResponse hwrs = (HttpWebResponse)hwrq.GetResponse())
            {
                Cookies.Add(hwrs.Cookies);
                using (StreamReader sr = new StreamReader(hwrs.GetResponseStream(), Encoding.Default))
                {
                    return hwrs.Headers.ToString() + sr.ReadToEnd().Trim();
                }
            }
        }


        public Cookie GetCookie(string url, string name)
        {
            foreach (Cookie c in Cookies.GetCookies(new Uri(url)))
            {
                if (c.Name == name)
                    return c;
            }
            return null;
        }


        string UserAgent;
        string Accept;
        string AcceptLang;
        DecompressionMethods DMethod;


        private void Randomize()
        {
            string[] useragents = {  
                                  "Mozilla/5.0 (Windows; U; Windows NT 5.1; ru; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13",  
                                  "Mozilla/5.0 (Macintosh; U; PPC Max OS X Mach-O; en-US; rv:1.8.0.7) Gecko/200609211 Camino/1.0.3"  
                              };
            string[] acceptlang = {  
                               "en-us;q=0.5,en;q=0.3",  
                               "ru-ru,ru; q=0.3",  
                               "q=0.8,en-us; q=0.3",  
                               "q=0.5,en"  
                           };
            string[] accepts = {
                              "application/xml"
                          };
            DecompressionMethods[] dmethods = {  
                                     DecompressionMethods.Deflate,  
                                     DecompressionMethods.GZip,  
                                     DecompressionMethods.None,  
                                     (DecompressionMethods.Deflate | DecompressionMethods.GZip),  
                                     (DecompressionMethods.Deflate | DecompressionMethods.None),  
                                     (DecompressionMethods.GZip | DecompressionMethods.None)  
                                 };
            AcceptLang = acceptlang[new Random().Next(acceptlang.Length)];
            UserAgent = useragents[new Random().Next(useragents.Length)];
            Accept = accepts[new Random().Next(accepts.Length)];
            DMethod = dmethods[new Random().Next(dmethods.Length)];
        }


        private HttpWebRequest CreateRequest(string url)
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            Request.UserAgent = UserAgent;
            Request.Accept = Accept;
            Request.Headers.Add("Accept-Language", AcceptLang);
            Request.AutomaticDecompression = DMethod;
            return Request;
        }
    }  
}
