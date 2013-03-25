using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using tilde.Models;

namespace tilde
{
    public class DatabaseStuff
    {
        private const string ConnectionString = @"Server=.\SQLExpress;Database=TildeContents;Trusted_Connection=True;";

        public void SaveNewFolder(FolderModel folder, IEnumerable<FileModel> files)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    using (var folderCommand = SaveFolderCommand(folder))
                    {
                        folderCommand.Connection = connection;
                        folderCommand.Transaction = transaction;
                        folderCommand.ExecuteNonQuery();
                    }

                    foreach (var file in files)
                    {
                        using (var fileCommand = SaveFileCommand(file))
                        {
                            fileCommand.Connection = connection;
                            fileCommand.Transaction = transaction;
                            fileCommand.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        private SqlCommand SaveFolderCommand(FolderModel folder)
        {
            const string insertFolder = "INSERT INTO [Folders] (FolderId, ParentId, Name) VALUES (@folderId, @parentId, @name )";

            var command = new SqlCommand(insertFolder);
            command.Parameters.AddWithValue("@folderId", folder.FolderId);
            command.Parameters.AddWithValue("@parentId", DBNull.Value); //probably want to change this
            command.Parameters.AddWithValue("@name", folder.Name);

            return command;
        }


        private SqlCommand SaveFileCommand(FileModel file)
        {
            const string insertFile = "INSERT INTO [Files] (FileId, FolderId, Name, Text) VALUES (@fileId, @folderId, @name, @text )";

            var command = new SqlCommand(insertFile);
            command.Parameters.AddWithValue("@fileId", file.FileId);
            command.Parameters.AddWithValue("@folderId", file.FolderId);
            command.Parameters.AddWithValue("@name", file.Name);
            command.Parameters.AddWithValue("@text", file.Text);

            return command;
        }

        public IEnumerable<FileModel> GetFolderContents(Guid folderId)
        {
            

            //get all files with folder = folderId


            return new List<FileModel>();
        }

    }
}