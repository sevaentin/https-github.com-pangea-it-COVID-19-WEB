using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covid19Web.Models
{
    public class Images
    {
        public int Id { get; set; }
        public string Session { get; set; }
        public byte[] Bytes { get; set; }
        public DateTime UploadTime { get; set; }
        public string ContentType { get; set; }
    }
}