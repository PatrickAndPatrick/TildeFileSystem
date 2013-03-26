using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using tilde.Models;

namespace tilde.Controllers
{
    public class FolderController : Controller
    {
        public ActionResult Index()
        {
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
            //TestDataBaseThing("randomName");
            folderInfo folderStuff = new folderInfo();
            FolderModel folderEntry = folderStuff.folderInformation("C:/extractThis");

            contentsExtractor uploader = new contentsExtractor();
            IEnumerable<FileModel> filesEntry = uploader.allFilesExtractor("C:/extractThis", folderEntry.FolderId);

            var databaseDoer = new DatabaseStuff();
            databaseDoer.SaveNewFolder(folderEntry, filesEntry);

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





        /*
        private void TestDataBaseThing(string directoryPath)
        {

            var databaseDoer = new DatabaseStuff();

            var folder = new FolderModel
            {
                FolderId = Guid.NewGuid(),
                Name = directoryPath
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
        }*/

    }





    public class folderInfo
    {
        public FolderModel folderInformation(string folderPath){
            var folderName = System.IO.Path.GetFileName(folderPath);
            Guid folderId = Guid.NewGuid();
            var parentId = Guid.NewGuid();//this should ultimately be replaced with the actual parent guid, instead of just mocking up a new one

            FolderModel returnFolder = new FolderModel{
                Name = folderName,
                FolderId = folderId,
                ParentId = parentId
            };

            return (returnFolder);
        }
    }


    public class contentsExtractor
    {
        //need to pass output of all this class's stuff into DatabaseStuff.cs.  TestDatabaseStuff in FolderController has an example of how to use the database class 
        public IEnumerable<FileModel> allFilesExtractor(string folderPath, Guid parentId){
            var folderId = parentId; //the folder model info ultimately needs to be removed from this class and placed one level up.  This should be easy since all that is needed to create the folder model is it's path, which will be available a level up.
            var folderName = System.IO.Path.GetFileName(folderPath);
            var allInfoList = new List<FileModel>();
            foreach (var path in Directory.GetFiles(@folderPath))
            {
                var fileId = Guid.NewGuid();

                FileModel fileInfo = ReadFile(path, folderId);
                allInfoList.Add(fileInfo);
            }
            return (allInfoList);
        }

        private FileModel ReadFile(string filePath, Guid parentId)
        {
            string fileContents = "";
            var fileId = Guid.NewGuid();
            var folderId = parentId;
            var fileName = System.IO.Path.GetFileName(filePath); // file name

            var fileType = System.IO.Path.GetExtension(filePath); // file extension

            if (fileType == ".docx" || fileType == ".doc")
            {
                fileContents = extractFromWordFile(filePath);
            }
            else if (fileType == ".txt")
            {
                fileContents = extractFromTextFile(filePath);
            }

            else if (fileType == ".ppt" || fileType == ".pptx")
            {
                fileContents = extractFromPowerpointFile(filePath);
            }

            else if (fileType == ".xls" || fileType == ".xlsx")
            {
                fileContents = extractFromExcelFile(filePath);
            }

            else
            {
                fileContents = "";
            }

            FileModel returnFile = new FileModel
            {
                FileId = fileId,
                FolderId = folderId,
                Name = fileName,
                Text = fileContents
            };
            return (returnFile);

        }

        private string extractFromTextFile(string filePath)
        {
            //extraction code here
            return ("filler for text");
        }

        private string extractFromWordFile(string filePath)
        {
            //extraction code here
            return ("filler for word");
        }

        private string extractFromExcelFile(string filePath)
        {
            //extraction code here
            return ("filler for excel");
        }

        private string extractFromPowerpointFile(string filePath)
        {
            //extraction code here
            return ("filler for ppt");
        }
    }
}
