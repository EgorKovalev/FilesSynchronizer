namespace Models.DropboxModels
{
    public class LinkToDownloadModel
    {
        public Metadata metadata { get; set; }
        public string link { get; set; }
    }

    public class Metadata
    {
        public string name { get; set; }
        public string path_lower { get; set; }
        public string path_display { get; set; }
        public string parent_shared_folder_id { get; set; }
        public string id { get; set; }
        public string client_modified { get; set; }
        public string server_modified { get; set; }
        public string rev { get; set; }
        public int size { get; set; }
    }    
}
