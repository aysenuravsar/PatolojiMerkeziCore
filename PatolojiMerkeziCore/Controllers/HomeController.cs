using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PatolojiMerkeziCore.Models;

namespace PatolojiMerkeziCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index(string adsoyad, string email, string phone, string kod, string mesaj)
        {
            string bolumadi = "";
            string hospital = "";
            string testtipi = "";

            string Utm_Source = "", Utm_Campaign = "", Utm_Content = "", Utm_Medium = "", Utm_Term = "";

            var queryCollection = System.Web.HttpUtility.ParseQueryString(Request.Headers["Referer"].ToString());

            if (queryCollection.AllKeys.Contains("utm_source") && !string.IsNullOrEmpty(queryCollection.Get("utm_source")))
                Utm_Source = queryCollection.Get("utm_source");

            if (queryCollection.AllKeys.Contains("utm_campaign") && !string.IsNullOrEmpty(queryCollection.Get("utm_campaign")))
                Utm_Campaign = queryCollection.Get("utm_campaign");

            if (queryCollection.AllKeys.Contains("utm_content") && !string.IsNullOrEmpty(queryCollection.Get("utm_content")))
                Utm_Content = queryCollection.Get("utm_content");

            if (queryCollection.AllKeys.Contains("utm_medium") && !string.IsNullOrEmpty(queryCollection.Get("utm_medium")))
                Utm_Medium = queryCollection.Get("utm_medium");

            if (queryCollection.AllKeys.Contains("utm_term") && !string.IsNullOrEmpty(queryCollection.Get("utm_term")))
                Utm_Term = queryCollection.Get("utm_term");

            var url= Request.Headers["Referer"].ToString()

            string postData = "{'FormSourceCode': 100000001, 'Utm_Source': '" + Utm_Source + "','Email': '" + email + "', 'LeadChannel': 3, 'Description': 'MEMORİAL PATOLOJİ MERKEZİ !!! /  Mesaj: " + mesaj + " / Test Tipi: " + testtipi + "' ,'CaseOriginCode': 3, 'Prefix': '" + kod + "', 'Phone': '" + phone + "', 'Referrer': '','FacebookUsername': 1, 'HospitalId': '" + hospital + "', 'Name': '" + adsoyad + "','Language': 100000000, 'Utm_Term': '" + Utm_Term + "', 'Utm_Content': '" + Utm_Content + "','LeadSource':100000000, 'RequestId': '" + Guid.NewGuid() + "', 'SubOriginCode': 100000000, 'isKvkkApproved':true, 'Utm_Campaign': '" + Utm_Campaign + "', 'SourceUrl': '" + HttpContext.Current.Request.Url + "', 'Surname': '', 'Campaign': 665544, 'SectionId': '" + bolumadi + "', 'FacebookAdSet': '', 'DoctorId': '','Utm_Medium': '" + Utm_Medium + "', 'Subject': 100000003}";

            WebRequest request = WebRequest.Create("https://prodapicrm.memorial.com.tr/api/v1/CreateContactAndIncident");
            request.Method = "POST";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();

            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string strHtml = readStream.ReadToEnd();

            string strReturn = "{" +
                              "\"success\":\"1\"" +
                            "}";

            var model= new ViewModel { strReturn=strReturn};
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
