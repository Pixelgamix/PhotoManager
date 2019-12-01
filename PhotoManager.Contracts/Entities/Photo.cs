using System;

namespace PhotoManager.Contracts.Entities
{
    public class Photo
    {
        public virtual Guid PhotoId { get; set; } 
        
        public virtual byte[] Content { get; set; }

        public virtual string FileName { get; set; }
        
        public virtual DateTime CreationTimestamp { get; set; }
    }
}