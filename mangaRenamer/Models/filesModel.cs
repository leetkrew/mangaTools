using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mangaRenamer.Models
{
    public class filesModel
    {
        public int pageNo { get; set; }
        public string sorting { get; set; }
        public string chapterTitle { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }

    public class directoryModel
    {
        public string sorting { get; set; }
        public string directoryName { get; set; }
        public string chapterTitle { get; set; }
    }

}
