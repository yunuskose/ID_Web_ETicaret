using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace IDETicaret.Helper
{
    public abstract class CCHelperBase
    {
        public abstract PaymentResult ConfirmPayment(HttpServerUtilityBase server, HttpRequestBase request, HttpSessionStateBase session, PosInfo pos);
        public abstract void PaymentRequest(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl);
        public abstract string PaymentRequest2(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl);
    }
}
