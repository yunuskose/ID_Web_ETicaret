using _PosnetDotNetTDSOOSModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDETicaret.Helper
{
    public class YKBHelper : CCHelperBase, IReceiverBank
    {

        public override void PaymentRequest(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl)
        {
            YKBPosInfo info = (YKBPosInfo)pos;
            C_PosnetOOSTDS toostds = new C_PosnetOOSTDS();
            toostds.SetURL(info.PosURL);
            toostds.SetTid(info.TerminalID);
            toostds.SetMid(info.MerchantID);
            toostds.SetPosnetID(info.ClientID);
            toostds.SetKey(info.Key);
            string str = posForm.Amount.ToString();
            if (str.IndexOf(",") != -1)
            {
                str = str.Replace(",", "");
            }
            else
            {
                str = str + "00";
            }
            string str2 = "";
            for (int i = 0; i < 20; i++)
            {
                str2 = str2 + "7";
            }
            string responseText = toostds.GetResponseText();
            if (toostds.CreateTranRequestDatas(posForm.CcOwnerName, str, "TL", posForm.Installment.ToString(), str2, "Sale", posForm.CcNumber, posForm.ExpireYear.Substring(2, 2) + posForm.ExpireMonth, posForm.Cvc))
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<html>");
                builder.AppendLine("<head>");
                builder.AppendLine("</head>");
                builder.AppendLine(string.Format("<body onload='document.forms[\"{0}\"].submit()'>", "form"));
                builder.AppendLine(string.Format("<form id=\"form\" method=\"POST\" action=\"{0}\">", pos.PosURL));
                builder.AppendLine("<input type=\"hidden\" name=\"mid\" value=\"" + info.MerchantID + "\" />");
                builder.AppendLine("<input type=\"hidden\" name=\"posnetID\" value=\"" + info.ClientID + "\" />");
                builder.AppendLine("<input type=\"hidden\" name=\"posnetData\" value=\"" + toostds.GetPosnetData() + "\" />");
                builder.AppendLine("<input type=\"hidden\" name=\"posnetData2\" value=\"" + toostds.GetPosnetData2() + "\" />");
                builder.AppendLine("<input type=\"hidden\" name=\"digest\" value=\"" + toostds.GetSign() + "\" />");
                builder.AppendLine("<input type=\"hidden\" name=\"vftCode\" value=\"\"/>");
                builder.AppendLine("<input type=\"hidden\" name=\"merchantReturnURL\" value=\"" + okUrl + "\" />");
                builder.AppendLine("<input type=\"hidden\" name=\"openANewWindow\" value=\"1\" />");
                builder.AppendLine("</form>");
                builder.AppendLine("</body>");
                builder.AppendLine("</html>");
                context.Response.Clear();
                context.Response.Write(builder.ToString());
                context.Response.End();
            }
            else
            {
                context.Response.Clear();
                context.Response.Write(responseText);
                context.Response.End();
            }

        }
        


        public override string PaymentRequest2(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl)
        {
            YKBPosInfo info = (YKBPosInfo)pos;
            C_PosnetOOSTDS toostds = new C_PosnetOOSTDS();
            toostds.SetURL(info.PosURL);
            toostds.SetTid(info.TerminalID);
            toostds.SetMid(info.MerchantID);
            toostds.SetPosnetID(info.ClientID);
            toostds.SetKey(info.Key);
            string str = posForm.Amount.ToString();
            if (str.IndexOf(",") != -1)
            {
                str = str.Replace(",", "");
            }
            else
            {
                str = str + "00";
            }
            string str2 = "";
            for (int i = 0; i < 20; i++)
            {
                str2 = str2 + "7";
            }
            string responseText = toostds.GetResponseText();
            if (toostds.CreateTranRequestDatas(posForm.CcOwnerName, str, "TL", posForm.Installment.ToString(), str2, "Sale", posForm.CcNumber, posForm.ExpireYear.Substring(2, 2) + posForm.ExpireMonth, posForm.Cvc))
            {
                StringBuilder builder1 = new StringBuilder();
                builder1.AppendLine("<html>");
                builder1.AppendLine("<head>");
                builder1.AppendLine("</head>");
                builder1.AppendLine(string.Format("<body onload='document.forms[\"{0}\"].submit()'>", "form"));
                builder1.AppendLine(string.Format("<form id=\"form\" method=\"POST\" action=\"{0}\">", pos.PosURL));
                builder1.AppendLine("<input type=\"hidden\" name=\"mid\" value=\"" + info.MerchantID + "\" />");
                builder1.AppendLine("<input type=\"hidden\" name=\"posnetID\" value=\"" + info.ClientID + "\" />");
                builder1.AppendLine("<input type=\"hidden\" name=\"posnetData\" value=\"" + toostds.GetPosnetData() + "\" />");
                builder1.AppendLine("<input type=\"hidden\" name=\"posnetData2\" value=\"" + toostds.GetPosnetData2() + "\" />");
                builder1.AppendLine("<input type=\"hidden\" name=\"digest\" value=\"" + toostds.GetSign() + "\" />");
                builder1.AppendLine("<input type=\"hidden\" name=\"vftCode\" value=\"\"/>");
                builder1.AppendLine("<input type=\"hidden\" name=\"merchantReturnURL\" value=\"" + okUrl + "\" />");
                builder1.AppendLine("<input type=\"hidden\" name=\"openANewWindow\" value=\"1\" />");
                builder1.AppendLine("</form>");
                builder1.AppendLine("</body>");
                builder1.AppendLine("</html>");
                return builder1.ToString();
            }
            return responseText.ToString();
        }




        public void ReceivePayment(HttpRequest Request, HttpResponse Response, PosInfo pos, string okURL)
        {
            YKBPosInfo info = (YKBPosInfo)pos;
            C_PosnetOOSTDS toostds = new C_PosnetOOSTDS();
            string str = null;
            string str2 = null;
            string str3 = null;
            str = Request.Form.Get("MerchantPacket");
            str2 = Request.Form.Get("BankPacket");
            str3 = Request.Form.Get("Sign");
            Request.Form.Get("TranType");
            toostds.SetURL("http://setmpos.ykb.com/PosnetWebService/XML");
            toostds.SetTid(info.TerminalID);
            toostds.SetMid(info.MerchantID);
            toostds.SetPosnetID(info.ClientID);
            toostds.SetKey(info.Key);
            toostds.SetProxyConfig(false, "", "", "");
            toostds.CheckAndResolveMerchantData(str, str2, str3);
            string totalPointAmount = toostds.GetTotalPointAmount();
            string xID = toostds.GetXID();
            string str6 = this.CurrencyFormat(toostds.GetAmount(), toostds.GetCurrencyCode(), true);
            string responseText = toostds.GetResponseText();
            string str8 = toostds.GetAmount().ToString();
            if (!string.IsNullOrEmpty(str8))
            {
                toostds.SetPointAmount(str8);
            }
            Response.Write("<html>");
            Response.Write("<head>");
            Response.Write("<META HTTP-EQUIV='Content-Type' content='text/html; charset=Windows-1254'>");
            Response.Write("<script language='JavaScript'>");
            Response.Write("function submitForm(form) {");
            Response.Write("\t form.submit();");
            Response.Write("}");
            Response.Write("</script>");
            Response.Write("<title>YKB - POSNET Redirector</title></head>");
            Response.Write("<body bgcolor='#02014E' OnLoad='submitForm(document.form2);' >");
            toostds.ConnectAndDoTDSTransaction(str, str2, str3);
            Response.Write(" <form name='form2' method='post' action='" + okURL + "' >");
            Response.Write("   <input type='hidden' name='XID' value='" + toostds.GetXID() + "'>");
            Response.Write("   <input type='hidden' name='Amount' value='" + toostds.GetAmount() + "'>");
            Response.Write("   <input type='hidden' name='WPAmount' value='" + str8 + "'>");
            Response.Write("   <input type='hidden' name='Currency' value='" + toostds.GetCurrencyCode() + "'>");
            Response.Write("   <input type='hidden' name='ApprovedCode' value='" + toostds.GetApprovedCode() + "'>");
            Response.Write("   <input type='hidden' name='AuthCode' value='" + toostds.GetAuthcode() + "'>");
            Response.Write("   <input type='hidden' name='HostLogKey' value='" + toostds.GetHostlogkey() + "'>");
            Response.Write("   <input type='hidden' name='RespCode' value='" + toostds.GetResponseCode() + "'>");
            Response.Write("   <input type='hidden' name='RespText' value='" + toostds.GetResponseText() + "'>");
            Response.Write("   <input type='hidden' name='Point' value='" + toostds.GetPoint() + "'>");
            Response.Write("   <input type='hidden' name='PointAmount' value='" + toostds.GetPointAmount() + "'>");
            Response.Write("   <input type='hidden' name='TotalPoint' value='" + toostds.GetTotalPoint() + "'>");
            Response.Write("   <input type='hidden' name='TotalPointAmount' value='" + toostds.GetTotalPointAmount() + "'>");
            Response.Write("   <input type='hidden' name='InstalmentNumber' value='" + toostds.GetInstalmentNumber() + "'>");
            Response.Write("   <input type='hidden' name='InstalmentAmount' value='" + toostds.GetInstalmentAmount() + "'>");
            Response.Write("   <input type='hidden' name='VftAmount' value='" + toostds.GetVFTAmount() + "'>");
            Response.Write("   <input type='hidden' name='VftDayCount' value='" + toostds.GetVFTDayCount() + "'>");
            Response.Write(" </form>");
            Response.Write(" </body>");
            Response.Write(" </html>");
            Response.End();


        }

        public double ConvertYTLToYKR(string amount)
        {
            if (amount != null && amount.Length > 0)
            {
                return double.Parse(amount.Replace(".", "")) * 100;
            }
            else
            {
                return 0;
            }
        }


        public string CurrencyFormat(string amount, string currencyCode, bool addCurrency)
        {

            if (amount == null || amount == "")
            {
                return "";
            }

            if (amount == "-1")
            {
                return "";
            }
            else
            {
                if (amount.Length == 2)
                {
                    amount = "0" + amount;
                }
                else if (amount.Length < 2)
                {
                    amount = "00" + amount;
                }


                if (currencyCode == "YT" || currencyCode == "TL" || currencyCode == "US" || currencyCode == "EU")
                {
                    string currencyFormat = amount.Substring(0, amount.Length - 2) + "," + amount.Substring(amount.Length - 2, 2);
                    if (addCurrency)
                    {
                        return currencyFormat + " " + GetCurrencyText(currencyCode);
                    }
                    else
                    {
                        return currencyFormat;
                    }
                }
                else
                {
                    return amount;
                }
            }
        }

        public  string GetCurrencyText(string currencyCode)
        {

            if (currencyCode == "YT" || currencyCode == "TL")
            {
                return "TL";
            }
            else if (currencyCode == "US")
            {
                return "USD";
            }
            else if (currencyCode == "EU")
            {
                return "EUR";
            }
            else
            {
                return "";
            }
        }

        public override PaymentResult ConfirmPayment(HttpServerUtilityBase server, HttpRequestBase request, HttpSessionStateBase session, PosInfo pos)
        {
            PaymentResult result = new PaymentResult();
            for (int i = 0; i <= request.Form.Count - 1; i++)
            {
                result.Description += request.Form.Keys.Get(i) + ": " + request.Form.Get(i) + "\n";
               
            }

            return result;
        }

       
    }
}
