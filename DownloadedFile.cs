using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public class DownloadedFile : IDownloaded
    {
        public string fileName { get; set; }
        public string fileThumbnail { get; set; }
        public string filePath { get; set; }
    }
}
