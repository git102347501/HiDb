using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Search
{
    public class SearchOutput
    {
        public List<dynamic> List { get; set; }

        public long Count { get; set; }
    }
}
