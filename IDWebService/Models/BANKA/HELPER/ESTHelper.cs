using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace IDETicaret.Helper
{
    public class ESTHelper : CCHelperBase
    {
        public override void PaymentRequest(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl)
        {
            string orderID = posForm.OrderID;
            string str2 = "";
            posForm.Installment.ToString();
            if (posForm.Installment != 0)
            {
                str2 = posForm.Installment.ToString();
            }
            string str3 = DateTime.Now.ToString();
            string password = pos.Password;
            if ((pos.ekPassword != null) && (Convert.ToString(pos.ekPassword).Trim().Length > 0))
            {
                password = pos.ekPassword;
            }
            string str5 = "3d_pay";
            string[] textArray1 = new string[] { pos.ClientID, orderID, posForm.Amount.ToString(), okUrl, failUrl, "Auth", str2, str3, password };
            string s = string.Concat(textArray1);
            string str7 = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.GetEncoding("ISO-8859-9").GetBytes(s)));
            string str8 = "myForm1";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine(string.Format("<body onload='document.forms[\"{0}\"].submit()'>", str8));
            builder.AppendLine(string.Format("<form id=\"{0}\" method=\"POST\" action=\"{1}\">", str8, pos.PosURL));
            builder.AppendLine("<input type=\"hidden\" name=\"clientid\" value=\"" + pos.ClientID + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"amount\" value=\"" + posForm.Amount.ToString() + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"oid\" value=\"" + orderID + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"okUrl\" value=\"" + okUrl + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"failUrl\" value=\"" + failUrl + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"rnd\" value=\"" + str3 + "\"/>");
            builder.AppendLine("<input type=\"hidden\" name=\"hash\" value=\"" + str7 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"storetype\" value=\"" + str5 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"lang\" value=\"tr\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"islemtipi\" value=\"Auth\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"total1\" value=\"" + posForm.Amount.ToString() + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"price1\" value=\"" + posForm.Amount.ToString() + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"currency\" value=\"949\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"pan\" value=\"" + posForm.CcNumber + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"Ecom_Payment_Card_ExpDate_Year\" value=\"" + posForm.ExpireYear + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"Ecom_Payment_Card_ExpDate_Month\" value=\"" + ((posForm.ExpireMonth.Length == 1) ? ("0" + posForm.ExpireMonth) : posForm.ExpireMonth) + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"cv2\" value=\"" + posForm.Cvc + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"taksit\" value=\"" + str2 + "\" />");
            builder.AppendLine("</form>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            context.Response.Clear();
            context.Response.Write(builder.ToString());
            context.Response.End();


        }


        public override string PaymentRequest2(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl)
        {
            string orderID = posForm.OrderID;
            string str2 = "";
            posForm.Installment.ToString();
            if (posForm.Installment != 0)
            {
                str2 = posForm.Installment.ToString();
            }
            string str3 = DateTime.Now.ToString();
            string password = pos.Password;
            if ((pos.ekPassword != null) && (Convert.ToString(pos.ekPassword).Trim().Length > 0))
            {
                password = pos.ekPassword;
            }
            string str5 = "3d_pay";
            string[] textArray1 = new string[] { pos.ClientID, orderID, posForm.Amount.ToString(), okUrl, failUrl, "Auth", str2, str3, password };
            string s = string.Concat(textArray1);
            string str7 = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.GetEncoding("ISO-8859-9").GetBytes(s)));
            string str8 = "myForm1";
            StringBuilder builder1 = new StringBuilder();
            builder1.AppendLine("<html>");
            builder1.AppendLine(string.Format("<body onload='document.forms[\"{0}\"].submit()'>", str8));
            builder1.AppendLine(string.Format("<form id=\"{0}\" method=\"POST\" action=\"{1}\">", str8, pos.PosURL));
            builder1.AppendLine("<input type=\"hidden\" name=\"clientid\" value=\"" + pos.ClientID + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"amount\" value=\"" + posForm.Amount.ToString() + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"oid\" value=\"" + orderID + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"okUrl\" value=\"" + okUrl + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"failUrl\" value=\"" + failUrl + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"rnd\" value=\"" + str3 + "\"/>");
            builder1.AppendLine("<input type=\"hidden\" name=\"hash\" value=\"" + str7 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"storetype\" value=\"" + str5 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"lang\" value=\"tr\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"islemtipi\" value=\"Auth\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"total1\" value=\"" + posForm.Amount.ToString() + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"price1\" value=\"" + posForm.Amount.ToString() + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"currency\" value=\"949\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"pan\" value=\"" + posForm.CcNumber + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"Ecom_Payment_Card_ExpDate_Year\" value=\"" + posForm.ExpireYear + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"Ecom_Payment_Card_ExpDate_Month\" value=\"" + ((posForm.ExpireMonth.Length == 1) ? ("0" + posForm.ExpireMonth) : posForm.ExpireMonth) + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"cv2\" value=\"" + posForm.Cvc + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"taksit\" value=\"" + str2 + "\" />");
            builder1.AppendLine("</form>");
            builder1.AppendLine("</body>");
            builder1.AppendLine("</html>");
            return builder1.ToString();
        }


        public override PaymentResult ConfirmPayment(HttpServerUtilityBase server, HttpRequestBase request, HttpSessionStateBase session, PosInfo pos)
        {
            string str = "";
            string[] strArray = new string[] { "AuthCode", "Response", "HostRefNum", "ProcReturnCode", "TransId", "ErrMsg" };
            IEnumerator enumerator = request.Form.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = (string)enumerator.Current;
                string str11 = request.Form.Get(current);
                bool flag = true;
                for (int j = 0; j < strArray.Length; j++)
                {
                    if (current.Equals(strArray[j]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag && (current == "oid"))
                {
                    str = str11;
                }
            }
            string str2 = request.Form.Get("HASHPARAMS");
            string str3 = request.Form.Get("HASHPARAMSVAL");
            string password = pos.Password;
            string str5 = "";
            int startIndex = 0;
            int index = 0;
            do
            {
                index = str2.IndexOf(":", startIndex);
                string str12 = (request.Form.Get(str2.Substring(startIndex, index - startIndex)) == null) ? "" : request.Form.Get(str2.Substring(startIndex, index - startIndex));
                str5 = str5 + str12;
                startIndex = index + 1;
            }
            while (startIndex < str2.Length);
            string s = str5 + password;
            string str7 = request.Form.Get("HASH");
            string str8 = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.GetEncoding("ISO-8859-9").GetBytes(s)));
            if (!str5.Equals(str3) || !str8.Equals(str7))
            {
                return new PaymentResult { Result = false, Description = "İmza ge\x00e7erli değil." };
            }
            string str9 = request.Form.Get("mdStatus");
            if ((!str9.Equals("1") && !str9.Equals("2")) && (!str9.Equals("3") && !str9.Equals("4")))
            {
                return new PaymentResult { Description = "\x00d6deme işlemi başarısız.", Result = false };
            }
            string str13 = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                string name = strArray[i];
                string str15 = request.Form.Get(name);
                if (name == "TransId")
                {
                    str13 = str15;
                    break;
                }
            }
            session["OrderID"] = null;
            return new PaymentResult { Description = "\x00d6deme işlemi başarılı.", Result = true, TransID = str13, OrderID = str };
        }



    }
}
