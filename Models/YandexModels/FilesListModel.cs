using System;
using System.Collections.Generic;

namespace Models.YandexModels
{
    public class FilesListModel
    {
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string resource_id { get; set; }
        public UInt64 revision { get; set; }
        public string type { get; set; }
        public Embedded _embedded { get; set; }
    }

    public class Embedded
    {
        public IList<Item> items { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string path { get; set; }
        public int total { get; set; }
    }

    public class Item
    {
        public CommentIds comment_ids { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string  name { get; set; }        
        public string path { get; set; }
        public string resource_id { get; set; }
        public UInt64 revision { get; set; }
        public string type { get; set; }
    }

    public class CommentIds
    {
        public string private_resource { get; set; }
        public string public_resource { get; set; }
    }
}
