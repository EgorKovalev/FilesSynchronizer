using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.OneDriveModels
{
    public class FilesListModel
    {
        public IList<OneDriveItem> value { get; set; }
    }

    public class OneDriveItem
    {
        public string id { get; set; }
        public string name { get; set; }

        [JsonProperty("@microsoft.graph.downloadUrl")]
        public string DownloadUrl { get; set; }
    }
}
