using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tilde.Models
{
    public class FolderModel
    {
        public string Name { get; set; }
        public Guid FolderId { get; set; }
        public Guid? ParentId { get; set; }
    }
}