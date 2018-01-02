namespace Models.OneDriveModels
{
    public class CopyFileModel
    {
        public ParentReference parentReference { get; set; }
        public string name { get; set; }
    }

    public class ParentReference
    {
        public string driveId { get; set; } = "root";
        public string id { get; set; }
    }
}
