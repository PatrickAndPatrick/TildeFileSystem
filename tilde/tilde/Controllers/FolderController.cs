using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tilde.Models;

namespace tilde.Controllers
{
    public class FolderController : Controller
    {
        public ActionResult Index()
        {
            //TestDataBaseThing();

            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View("");
        }

        [HttpPost]
        public ActionResult Upload(string directoryPath)
        {

            //perform file upload

            return RedirectToAction("Index");
        }

        public ActionResult DisplayFolderList()
        {

            //get folders

            return View(new List<FolderModel>());
        }

        public ActionResult DisplayFilesFromFolder(Guid folderId)
        {
            return PartialView(new FolderModel());
        }

        public ActionResult PopOutFileText(Guid fileId)
        {
            return PartialView(new FileModel());
        }

        private void TestDataBaseThing()
        {

            var databaseDoer = new DatabaseStuff();

            var folder = new FolderModel
            {
                FolderId = Guid.NewGuid(),
                Name = "FolderTime"
            };

            var files = new List<FileModel>();
            files.Add(new FileModel
            {
                FileId = Guid.NewGuid(),
                FolderId = folder.FolderId,
                Name = "FileTime",
                Text = "Read this!"
            });


            databaseDoer.SaveNewFolder(folder, files);
        }

    }
}
