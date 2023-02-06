using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDETicaret
{
    public class YKBPosInfo : PosInfo
    {
        private string terminalID;
        public string TerminalID
        {
            get { return terminalID; }
            set { terminalID = value; }
        }

        private string merchantID;
        public string MerchantID
        {
            get { return merchantID; }
            set { merchantID = value; }
        }

        private string key;
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        private string successUrl;
        public string SuccessUrl
        {
            get { return key; }
            set { key = value; }
        }

    }
}
