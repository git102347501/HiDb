using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Tables
{
    public class TableColumnInput
    {
        [Required]
        public string DataBase { get; set; }

        public string Mode { get; set; }

        [Required]
        public string Table { get; set; }
    }
}
