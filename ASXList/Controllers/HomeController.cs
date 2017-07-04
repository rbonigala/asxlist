using ASXList.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;

namespace ASXList.Controllers
{
    public class HomeController : Controller
    {
        public const int RecordsPerPage = 20;
        List<Company> asxCompanyList;
        List<string> asxCodeList;

        public ActionResult Index()
        {
            return RedirectToAction("GetAsxCompanies");
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetAsxCompanies(int? pageNum)
        {
            int fromPageNum = 0;
            pageNum = pageNum ?? 0;


            if (Request.IsAjaxRequest())
            {
                var companies = GetRecordsForPage(pageNum.Value);
                ViewBag.IsEndOfRecords = (companies.Any());
                return PartialView("_CompanyData", companies);
            }
            else
            {
                //GetAsxCodeListFromFile();

                //int from = (fromPageNum * RecordsPerPage);
                //List<string> tempList = GetAsxCodesToFetchData(pageNum.Value);
                //foreach (var item in tempList)
                //{
                //    asxCompanyList.Add(GetCompanyDataByAsxCode(item));
                //}

                

                ViewBag.Companies = GetRecordsForPage(pageNum.Value);
                return View("Index");
            }
        }

        private List<string> GetAsxCodesToFetchData(int fromPageNum)
        {
            var tempList = (from rec in asxCodeList
                            select rec).Skip(fromPageNum).Take(RecordsPerPage).ToList<string>();
            asxCompanyList = new List<Company>();
            return tempList;
        }

        private void GetAsxCodeListFromFile()
        {
            var fileName = Path.Combine(Server.MapPath("~/App_Data/"), "list.txt");
            using (StreamReader sr = new StreamReader(fileName))
            {
                asxCodeList = new List<string>();
                while (!sr.EndOfStream)
                {
                    // Read the stream to a string, and write the string to the console.
                    String asxCode = sr.ReadLine();
                    asxCodeList.Add(asxCode);
                }
            }
        }

        public List<Company> GetRecordsForPage(int pageNum)
        {
            GetAsxCodeListFromFile();            

            if (asxCompanyList == null)
            {
                asxCompanyList = new List<Company>();
            }

            int from = (pageNum * RecordsPerPage);
            List<string> tempList = GetAsxCodesToFetchData(from);

            foreach (var item in tempList)
            {
                asxCompanyList.Add(GetCompanyDataByAsxCode(item));
            }
            
            return asxCompanyList;
        }

        private Company GetCompanyDataByAsxCode(string asxCode)
        {
            Company _company = new Company();
            var client = new RestClient();
            client.BaseUrl = new Uri("http://www.asx.com.au");
            var request = new RestRequest("b2c-api/1/company/{asxCode}?fields=primary_share,latest_annual_reports,last_dividend,primary_share.indices", Method.GET);
            request.AddUrlSegment("asxCode", asxCode);
            var response = client.Execute(request);

            Company company = JsonConvert.DeserializeObject<Company>(response.Content);

            return company;
        }
    }
}