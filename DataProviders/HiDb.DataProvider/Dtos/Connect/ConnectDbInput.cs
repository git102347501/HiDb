using HiDb.DataProvider.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Connect
{
    public class ConnectDbInput
    {
        public DbTypeEnum Type { get; set; }

        public string Account { get; set; }

        public string PassWord { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }

        public string Database { get; set; }

        /// <summary>
        /// 是否信任服务端证书
        /// </summary>
        public bool? TrustCert { get; set; }

        /// <summary>
        /// 可信链接
        /// </summary>
        public bool? TrustedConnection { get; set; }

        /// <summary>
        /// 加密
        /// </summary>
        public bool? Encrypt { get; set; }
    }
}
