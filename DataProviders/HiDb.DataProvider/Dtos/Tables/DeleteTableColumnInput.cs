using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Tables
{
    public class DeleteTableColumnInput
    {
        public string DataBase { get; set; }

        public string Mode { get; set; }

        public string Table { get; set; }

        public string Column { get; set; }
    }
}
