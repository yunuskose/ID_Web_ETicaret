using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IDETicaret.Helper
{
    public interface IReceiverBank
    {
        void ReceivePayment(HttpRequest Request, HttpResponse Response, PosInfo pos, string okURL);
    }
}
