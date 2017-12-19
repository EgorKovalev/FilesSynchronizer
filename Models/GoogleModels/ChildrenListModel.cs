using System.Collections.Generic;

namespace Models.GoogleModels
{
    public class ChildrenListModel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }
        public string nextPageToken { get; set; }
        public string nextLink { get; set; }
        public IList<ChildrenModel> items { get; set; }
    }

    public class ChildrenModel
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string selfLink { get; set; }
        public string childLink { get; set; }
    }
}
