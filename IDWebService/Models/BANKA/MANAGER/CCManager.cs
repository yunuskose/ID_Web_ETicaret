using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using IDETicaret.Banka.Entity;
using IDETicaret.Helper;

namespace IDETicaret.Manager
{
    public class CCManager
    {
        private static CCManager instance;

        public static CCManager Instance
        {
            get { return CCManager.instance; }
        }

        Dictionary<Banks, PosInfo> posDic;
        Dictionary<PosType, CCHelperBase> posTypeDic;

        public static CCManager CreateInstance(string okURL, string failURL, string receiveURL, List<PosInfo> posInfoList)
        {
            instance = new CCManager(okURL, failURL, receiveURL, posInfoList);
            return instance;
        }

        string okURL, failURL, receiveURL;
        private CCManager(string okURL, string failURL, string receiveURL, List<PosInfo> posInfoList)
        {
            posTypeDic = new Dictionary<PosType, CCHelperBase>();
            posTypeDic.Add(PosType.EST, new ESTHelper());
            posTypeDic.Add(PosType.GARANTI, new GarantiHelper());
            posTypeDic.Add(PosType.YAPIKREDI, new YKBHelper());

            posDic = new Dictionary<Banks, PosInfo>();
            this.okURL = okURL;
            this.failURL = failURL;
            this.receiveURL = receiveURL;
            for (int i = 0; i < posInfoList.Count; i++)
                AddToPosDic(posInfoList[i]);
        }

        public void SendPayment(HttpContextBase context, PosForm posForm, Banks bank)
        {
            string str;
            PosInfo pos = this.posDic[bank];
            CCHelperBase base2 = this.posTypeDic[pos.PosType];
            if (base2 is IReceiverBank)
            {
                object[] objArray1 = new object[] { this.receiveURL, "?bank=", (int)bank, "&&OrderID=", posForm.OrderID };
                str = string.Concat(objArray1);
            }
            else
            {
                object[] objArray2 = new object[] { this.okURL, "?bank=", (int)bank, "&&OrderID=", posForm.OrderID };
                str = string.Concat(objArray2);
            }
            base2.PaymentRequest(context, posForm, pos, str, this.failURL);

        }

        public void SendPayment(HttpContextBase context, PosForm posForm, string bank)
        {
            if (bank.IndexOf("0") == 0)
            {
                bank = bank.Replace("0", "");
            }
            this.SendPayment(context, posForm, (Banks)Convert.ToInt32(bank));

        }
        public string SendPayment2(HttpContextBase context, PosForm posForm, Banks bank)
        {
            string str;
            PosInfo pos = this.posDic[bank];
            CCHelperBase base2 = this.posTypeDic[pos.PosType];
            if (base2 is IReceiverBank)
            {
                object[] objArray1 = new object[] { this.receiveURL, "?bank=", (int)bank, "&&OrderID=", posForm.OrderID };
                str = string.Concat(objArray1);
            }
            else
            {
                object[] objArray2 = new object[] { this.okURL, "?bank=", (int)bank, "&&OrderID=", posForm.OrderID };
                str = string.Concat(objArray2);
            }
            return base2.PaymentRequest2(context, posForm, pos, str, this.failURL);
        }



        public void ReceivePayment(HttpRequest request, HttpResponse response)
        {
            if ((request.QueryString["bank"] != null) && (request.QueryString["OrderID"] != null))
            {
                string str = request.QueryString["bank"].ToString();
                string str2 = request.QueryString["OrderID"].ToString();
                PosInfo pos = this.posDic[(Banks)Convert.ToInt32(str)];
                string[] textArray1 = new string[] { this.okURL, "?bank=", str, "&&OrderID=", str2 };
                ((IReceiverBank)this.posTypeDic[pos.PosType]).ReceivePayment(request, response, pos, string.Concat(textArray1));
            }

        }

        public PaymentResult ConfirmPayment(HttpServerUtilityBase server, HttpRequestBase request, HttpSessionStateBase session)
        {
            if (request.QueryString["bank"] != null)
            {
                string str = request.QueryString["bank"].ToString();
                PosInfo pos = this.posDic[(Banks)Convert.ToInt32(str)];
                return this.posTypeDic[pos.PosType].ConfirmPayment(server, request, session, pos);
            }
            return new PaymentResult { Result = false, Description = "Ödeme işlemi başarısız." };

        }

        private void AddToPosDic(PosInfo pos)
        {
            posDic.Add(pos.Bank, pos);
        }
    }
}
