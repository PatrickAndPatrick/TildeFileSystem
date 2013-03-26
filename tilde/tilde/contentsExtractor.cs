using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tilde
{
    public class contentsExtractor
    {
        //need to pass output of all this class's stuff into DatabaseStuff.cs.  TestDatabaseStuff in FolderController has an example of how to use the database class 
        public IEnumerable<FileModel> allFilesExtractor(string folderPath){
            var folderId = Guid.NewGuid(); //the folder model info ultimately needs to be removed from this class and placed one level up.  This should be easy since all that is needed to create the folder model is it's path, which will be available a level up.
            var folderName = System.IO.Path.GetFileName(folderPath);
            var allInfoList = new List<FileModel>;
            foreach (var path in Directory.GetFiles(@folderPath))
            {
                var fileId = Guid.NewGuid();

                //Console.WriteLine(path); // full path
                FileModel fileInfo = ReadFile(path);
            }
        }

        private FileModel ReadFile(string filePath)
        {
            var fileName = System.IO.Path.GetFileName(filePath); // file name
            var fileType = System.IO.Path.GetExtension(filePath); // file extension

            if (fileType == ".docx" || fileType == ".doc")
            {
                var fileContents = extractFromWordFile(filePath);
            }
            else if (fileType == ".txt")
            {
                var fileContents = extractFromTextFile(filePath);
            }
            
            else if(fileType == ".ppt" || fileType == ".pptx")
            {
                var fileContents = extractFromPowerpointFile(filePath);
            }

            else if (fileType == ".xls" || fileType == ".xlsx")
            {
                var fileContents = extractFromExcelFile(filePath);
            }

            else
            {
                fileContents = "";
            }
        }

        private string extractFromTextFile(string filePath){
            //extraction code here
        }

        private string extractFromWordFile(string filePath){
            //extraction code here
        }

        private string extractFromExcelFile(string filePath){
            //extraction code here
        }

        private string extractFromPowerpointFile(string filePath){
            //extraction code here
        }
    }
}