using System.Collections.Generic;

namespace PhotoManager.Contracts.Database
{
    public class FileNameSettings
    {
        public bool SeparateYearMonthDay { get; set; }
        
        public bool SeparateTimeFromDate { get; set; }
        
        public bool IncludesYear { get; set; }
        
        public bool IncludesMonth { get; set; }
        
        public bool IncludesDay { get; set; }
        
        public bool IncludesTime { get; set; }
        
        public bool IncludesOriginFileName { get; set; }
        
        public bool IncludesUploadDate { get; set; }
        
        public char Separator { get; set; }
        
        public List<string> FileNameOrder { get; set; }
        
    }
}