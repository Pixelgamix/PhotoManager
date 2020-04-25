namespace PhotoManager.Contracts.Database
{
    public class DirectorySettings
    {
        public string Directory { get; set; }
        public DirectorySettings SubDirectory { get; set; }
    }
}