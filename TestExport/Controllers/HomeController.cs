using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http;
using System.Web.Mvc.Ajax;
using Aspose.Cells;
using Aspose.Cells.Utility;
using Microsoft.SharePoint.Client;

namespace TestExport.Controllers
{
    /// <summary>
    /// Model for post
    /// </summary>
    public class Model
    {
        /// <summary>
        /// json string of csv document
        /// </summary>
        public String jsonString { get; set; }
    }

    [RoutePrefix("api/Home")]
    public class HomeController : ApiController
    {
        private string siteUrl = "https://heglagroup.sharepoint.com/sites/hegla_documents";
        private string password = "Boj46040";
        private string userName = "ashish.shukla@hegla.de";
        private string p = "test.csv";

        /// <summary>
        /// Resturns string
        /// </summary>
        /// 
        /// <returns>string</returns>
        /// <response code="200">Welcome</response>
        [ResponseType(typeof(String))]
        [HttpGet]
        public string Index()
        {
            return "Welcome";
        }

        /// <summary>
        /// Uploads the file to sharepoint and returns url
        /// </summary>
        /// <returns>string</returns>
        /// <response code="200">Upload FIle</response>
        [ResponseType(typeof(String))]
        [HttpPost]
        [Route("uploadFile")]
        public string uploadFile(Model model)
        {

            Workbook workbook = new Workbook();

            Worksheet worksheet = workbook.Worksheets[0];


            // Set JsonLayoutOptions
            JsonLayoutOptions options = new JsonLayoutOptions();
            options.ArrayAsTable = true;

            // Import JSON Data
            Aspose.Cells.Utility.JsonUtility.ImportData(model.jsonString, worksheet.Cells, 0, 0, options);

            // Save Excel file
            string excelfile = DateTime.Now.ToFileTime() + ".xlsx";
            workbook.Save(excelfile);


            ClientContext context = new ClientContext(siteUrl);

            SecureString passWord = new SecureString();
            foreach (var c in password) passWord.AppendChar(c);
            context.Credentials = new SharePointOnlineCredentials(userName, passWord);
            Web site = context.Web;

            //Get the required RootFolder
            string barRootFolderRelativeUrl = "Shared Documents/ExportExcel";
            Folder barFolder = site.GetFolderByServerRelativeUrl(barRootFolderRelativeUrl);

            //Add file to new Folder
            FileCreationInformation newFile = new FileCreationInformation { Content = System.IO.File.ReadAllBytes(excelfile), Url = Path.GetFileName(excelfile), Overwrite = true };
            barFolder.Files.Add(newFile);
            barFolder.Update();

            context.ExecuteQuery();

            //Return the URL of the new uploaded file
            string newUrl = siteUrl + barRootFolderRelativeUrl + "/" + Path.GetFileName(excelfile);

            return newUrl;
        }
    }
}
