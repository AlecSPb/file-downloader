using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownloader
{
    public interface IDownloaded
    {
        string fileName { get; set; }
        string fileThumbnail { get; set; }
        string filePath { get; set; }
    }
}
