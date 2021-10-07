using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Net;

namespace RSSFeeder.Pages
{
    public class PrivacyModel : PageModel
    {
        public string Message { get; set; }

        public string Check { get; set; }
        //private readonly decimal currentRate = 64.1m;

        public string url { get; set; }
        public int timeS { get; set; }

        private static Timer aTimer;

        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Message = "Сохранить";
            ReadXML();
            //SetTimer();
            
        }
        public void OnPost(string urltextbox, int timetextbox)
        {
            
            WriteXML(urltextbox, timetextbox);
            ReadXML();
        }

        public void ReadXML()
        {
            //Message = "Ссылка подходит";
            //
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("Settings.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.Name == "url")
                    url = xnode.InnerText;
                if (xnode.Name == "updatetime")
                    timeS = Convert.ToInt32(xnode.InnerText);
            }
        }

        public void WriteXML(string url, int time)
        {
            Uri urlCheck = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
            request.Timeout = 5000;
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load("Settings.xml");
                    XmlElement xRoot = xDoc.DocumentElement;
                    xRoot.RemoveAll();
                    xDoc.Save("Settings.xml");
                    //XmlElement configRSSElement = xDoc.CreateElement("configRSS");
                    XmlElement urlElem = xDoc.CreateElement("url");
                    XmlElement updatetimeElem = xDoc.CreateElement("updatetime");
                    XmlText urlText = xDoc.CreateTextNode(url);
                    XmlText updatetimeText = xDoc.CreateTextNode(Convert.ToString(time));

                    urlElem.AppendChild(urlText);
                    updatetimeElem.AppendChild(updatetimeText);
                    xRoot.AppendChild(urlElem);
                    xRoot.AppendChild(updatetimeElem);
                    //xRoot.AppendChild(configRSSElement);
                    xDoc.Save("Settings.xml");
                    Message = "Сохранено";
                }
            }
            catch (Exception)
            {
                Message = "Неподходящая ссылка";
            }
        }
    }
}
