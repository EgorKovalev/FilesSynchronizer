using System.Collections.Generic;
using Google.Apis.Drive.v3.Data;

namespace Models.GoogleModels
{
    public class FilesListModel
    {
        public string  kind { get; set; }
        public string nextPageToken { get; set; }
        public bool incompleteSearch { get; set; }
        public IList<File> files { get; set; }
    }
}
