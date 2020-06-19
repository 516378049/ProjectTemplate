using WebAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Core.Models
{
    public class SignatureHead:ModelBase
    {
        private String httpMethod;
        private String accept;
        private String contentMD5;
        private String contentType;
        private String gmtDate;
        private String version;
        private String nonce;

        public string HttpMethod { get => httpMethod; set => httpMethod = value; }
        public string Accept { get => accept; set => accept = value; }
        public string ContentMD5 { get => contentMD5; set => contentMD5 = value; }
        public string ContentType { get => contentType; set => contentType = value; }
        public string GmtDate { get => gmtDate; set => gmtDate = value; }
        public string Version { get => version; set => version = value; }
        public string Nonce { get => nonce; set => nonce = value; }
    }
}
