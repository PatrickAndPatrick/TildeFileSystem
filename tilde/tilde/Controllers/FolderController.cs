using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using tilde.Models;
using System.Data.SqlClient;
using System.Data;

namespace tilde.Controllers
{
    public class FolderController : Controller
    {
        public ActionResult Index()
        {
            var allFolders = DisplayFolderList();

            return View(allFolders);
        }

        DataTable GetDataFromQuery(string query)
        {
            SqlDataAdapter adap = new SqlDataAdapter(query, @"Server=.\SQLExpress;Database=TildeContents;Trusted_Connection=True;");
            DataTable data = new DataTable();
            adap.Fill(data);
            return data;
        }  

        [HttpPost]
        public ActionResult Upload(string uploadInput)
        {
            extractor uploader = new extractor();
            FolderModel folderEntry = uploader.folderInformation(uploadInput);

            IEnumerable<FileModel> filesEntry = uploader.allFilesExtractor(uploadInput, folderEntry.FolderId);

            var databaseDoer = new DatabaseStuff();
            databaseDoer.SaveNewFolder(folderEntry, filesEntry);

            return RedirectToAction("Index");
        }

        public IEnumerable<FolderModel> DisplayFolderList()
        {

            //get folders
            var databaseDoer = new DatabaseStuff();
            var foldersToReturn = databaseDoer.GetFolderList();

            return (foldersToReturn);
        }

        public ActionResult DisplayFilesFromFolder(Guid folderId)
        {
            return PartialView(new FolderModel());
        }

        public ActionResult PopOutFileText(Guid fileId)
        {
            return PartialView(new FileModel());
        }

        public ActionResult ShowFiles(Guid ParentId)
        {
            var databaseDoer = new DatabaseStuff();
            var files = databaseDoer.GetFolderContents(ParentId);

            return View(files);
        }
    }


}
