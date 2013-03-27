using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Management;
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

        public IEnumerable<FolderModel> GetFolderList()
        {
            const string selectAllFolders = "SELECT FolderId, ParentId, Name FROM [Folders] ORDER BY Name";

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(selectAllFolders, connection);

            var resultTable = new DataTable();
            try
            {
                using (connection)
                {
                    var adapter = new SqlDataAdapter { SelectCommand = command };
                    adapter.Fill(resultTable);
                }
            }
            catch (Exception exception)
            {
                throw new SqlExecutionException("Failed to get folders from database.", exception);
            }
            

            var folderList = new List<FolderModel>();
            
            using (var reader = resultTable.CreateDataReader())
            {
                while (reader.Read())
                {
                    try
                    {
                        var folder = new FolderModel()
                            {
                                FolderId = reader.GetGuid(0),
                                Name = reader.GetString(2),
                            };

                        if (!reader.IsDBNull(1))
                            folder.ParentId = reader.GetGuid(1);

                        folderList.Add(folder);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Error paring Folder Data.", exception);
                    }
                }
            }

            return folderList;
        }

        public IEnumerable<FileModel> GetFolderContents(Guid folderId)
        {
            const string selectFolder = "SELECT FileId, FolderId, Name, Text FROM [Files] WHERE FolderId=@folderId ORDER BY Name";

            var connection = new SqlConnection(ConnectionString);
            var command = new SqlCommand(selectFolder, connection);
            command.Parameters.AddWithValue("@folderId", folderId);

            var resultTable = new DataTable();
            try
            {
                using (connection)
                {
                    var adapter = new SqlDataAdapter { SelectCommand = command };
                    adapter.Fill(resultTable);
                }
            }
            catch (Exception exception)
            {
                throw new SqlExecutionException("Failed to get files from database.", exception);
            }
            

            var fileList = new List<FileModel>();
            
            using (var reader = resultTable.CreateDataReader())
            {
                while (reader.Read())
                {
                    try
                    {
                        var file = new FileModel
                            {
                                FileId = reader.GetGuid(0),
                                FolderId = reader.GetGuid(1),
                                Name = reader.GetString(2),
                                Text = reader.GetString(3)
                            };

                        fileList.Add(file);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Error paring File Data.", exception);
                    }
                }
            }

            return fileList;
        }
    }
}