using System.Collections.Generic;

namespace PhotoManager.Contracts.Database
{
    public class FileNameSettings
    {
        public bool SeparateYearMonthDay { get; set; }
        
        public bool SeparateTimeFromDate { get; set; }
        
        public char Separator { get; set; }
        
        public List<string> FileNameOrder { get; set; }
        
    }
}