namespace PhotoManager.Contracts.Database
{
    /// <summary>
    /// Settings for the Database.
    /// </summary>
    public class DatabaseSettings
    {
        /// <summary>
        /// Path, where the photos have to be stored. 
        /// </summary>
        public virtual string StoragePath { get; set; }

        /// <summary>
        /// Format for the photo-filename.
        /// </summary>
        public virtual FileNameSettings FileNameSettings { get; set; }
        
        /// <summary>
        /// Format for the directory-structure.
        /// </summary>
        public virtual DirectorySettings DirectoryFormat { get; set; }
    }
}