using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Connect
{
    public class ConnectDbInput
    {
        public string Account { get; set; }

        public string PassWord { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }
    }
}
