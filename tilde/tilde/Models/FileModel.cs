using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tilde.Models
{
    public class FileModel
    {
        public Guid FileId { get; set; }
        public Guid FolderId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}