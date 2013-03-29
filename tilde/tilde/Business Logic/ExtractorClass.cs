using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tilde.Models;
using System.IO;

namespace tilde
{
    public class extractor
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

        //need to pass output of all this class's stuff into DatabaseStuff.cs.  TestDatabaseStuff in FolderController has an example of how to use the database class 
        public IEnumerable<FileModel> allFilesExtractor(string folderPath, Guid parentId)
        {
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
            //return ("filler for text");
            StreamReader reader = new StreamReader(filePath);
            string allContents = reader.ReadToEnd();
            return (allContents);
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