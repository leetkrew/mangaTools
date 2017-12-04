using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mangaRenamer.Models
{
    public class filesModel
    {
        public string from { get; set; }
        public string to { get; set; }
        public int pageNo { get; set; }
    }

    public class directoryModel
    {
        public string directoryName { get; set; }
    }

}
