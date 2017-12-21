using System.Collections.Generic;

namespace Models.DropboxModels
{
    public class FilesListModel
    {
        public IList<Item> entries { get; set; }
        public string cursor { get; set; }
        public bool has_more { get; set; }
    }

    public class Item
    {
        public string tag { get; set; }
        public string name { get; set; }
        public string path_lower { get; set; }
        public string path_display { get; set; }
        public string id { get; set; }        
    }
}
