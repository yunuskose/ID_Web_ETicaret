using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace IDETicaret.Helper
{
    public class GarantiHelper : CCHelperBase
    {
        public override void PaymentRequest(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl)
        {
            byte[] data = new byte[7];
            RandomNumberGenerator.Create().GetBytes(data);
            new Random();
            string str = "PROD";
            string str2 = "v0.01";
            string str3 = "PROVAUT";
            string str4 = "sales";
            string str5 = posForm.Amount.ToString();
            if ((str5.IndexOf(".") == -1) && (str5.IndexOf(",") == -1))
            {
                str5 = str5 + "00";
            }
            else
            {
                str5 = str5.Replace(".", "").Replace(",", "");
            }
            string str6 = "949";
            string str7 = (posForm.Installment == 0) ? "" : posForm.Installment.ToString();
            GarantiPosInfo info1 = (GarantiPosInfo)pos;
            string terminalUserID = info1.TerminalUserID;
            string orderID = posForm.OrderID;
            string terminalID = info1.TerminalID;
            string str11 = "0" + terminalID;
            string merchantID = info1.MerchantID;
            string provisionPassword = info1.ProvisionPassword;
            string password = info1.Password;
            string str15 = okUrl;
            string str16 = failUrl;
            string str17 = this.GetSHA1(password + str11).ToUpper();
            string[] textArray1 = new string[] { terminalID, orderID, str5, str15, str16, str4, str7, provisionPassword, str17 };
            string str18 = this.GetSHA1(string.Concat(textArray1)).ToUpper();
            string posURL = pos.PosURL;
            string str20 = "myForm1";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine(string.Format("<body onload='document.forms[\"{0}\"].submit()'>", str20));
            builder.AppendLine(string.Format("<form id=\"{0}\" method=\"POST\" action=\"{1}\">", str20, posURL));
            builder.AppendLine("<input type=\"hidden\" name=\"mode\" value=\"" + str + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"secure3dsecuritylevel\" value=\"3D_PAY\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"apiversion\" value=\"" + str2 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"terminalprovuserid\" value=\"" + str3 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"orderid\" value=\"" + orderID + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"terminaluserid\" value=\"" + terminalUserID + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"terminalmerchantid\" value=\"" + merchantID + "\"/>");
            builder.AppendLine("<input type=\"hidden\" name=\"txntype\" value=\"" + str4 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"txnamount\" value=\"" + str5 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"txncurrencycode\" value=\"" + str6 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"txninstallmentcount\" value=\"" + str7 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"terminalid\" value=\"" + terminalID + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"successurl\" value=\"" + str15 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"errorurl\" value=\"" + str16 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"motoind\" value=\"N\"/>");
            builder.AppendLine("<input type=\"hidden\" name=\"refreshtime\" value=\"10\"/>");
            builder.AppendLine("<input type=\"hidden\" name=\"txntimestamp\" value=\"" + DateTime.Now.ToString() + "\"/>");
            builder.AppendLine("<input type=\"hidden\" name=\"customeremailaddress\" value=\"" + posForm.Email + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"customeripaddress\" value=\"" + posForm.IPAdress + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"secure3dhash\" value=\"" + str18 + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"cardnumber\" value=\"" + posForm.CcNumber + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"cardexpiredatemonth\" value=\"" + posForm.ExpireMonth + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"cardexpiredateyear\" value=\"" + posForm.ExpireYear.Substring(2, 2) + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"cardcvv2\" value=\"" + posForm.Cvc + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"taksitAdet\" value=\"" + posForm.Installment + "\" />");
            builder.AppendLine("<input type=\"hidden\" name=\"bankaAd\" value=\"garanti\" />");
            builder.AppendLine("</form>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(builder.ToString());
            HttpContext.Current.Response.End();

        }

        public override string PaymentRequest2(HttpContextBase context, PosForm posForm, PosInfo pos, string okUrl, string failUrl)
        {
            byte[] data = new byte[7];
            RandomNumberGenerator.Create().GetBytes(data);
            new Random();
            string str = "PROD";
            string str2 = "v0.01";
            string str3 = "PROVAUT";
            string str4 = "sales";
            string str5 = posForm.Amount.ToString();
            if ((str5.IndexOf(".") == -1) && (str5.IndexOf(",") == -1))
            {
                str5 = str5 + "00";
            }
            else
            {
                str5 = str5.Replace(".", "").Replace(",", "");
            }
            string str6 = "949";
            string str7 = (posForm.Installment == 0) ? "" : posForm.Installment.ToString();
            GarantiPosInfo info1 = (GarantiPosInfo)pos;
            string terminalUserID = info1.TerminalUserID;
            string orderID = posForm.OrderID;
            string terminalID = info1.TerminalID;
            string str11 = "0" + terminalID;
            string merchantID = info1.MerchantID;
            string provisionPassword = info1.ProvisionPassword;
            string password = info1.Password;
            string str15 = okUrl;
            string str16 = failUrl;
            string str17 = this.GetSHA1(password + str11).ToUpper();
            string[] textArray1 = new string[] { terminalID, orderID, str5, str15, str16, str4, str7, provisionPassword, str17 };
            string str18 = this.GetSHA1(string.Concat(textArray1)).ToUpper();
            string posURL = pos.PosURL;
            string str20 = "myForm1";
            StringBuilder builder1 = new StringBuilder();
            builder1.AppendLine("<html>");
            builder1.AppendLine(string.Format("<body onload='document.forms[\"{0}\"].submit()'>", str20));
            builder1.AppendLine(string.Format("<form id=\"{0}\" method=\"POST\" action=\"{1}\">", str20, posURL));
            builder1.AppendLine("<input type=\"hidden\" name=\"mode\" value=\"" + str + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"secure3dsecuritylevel\" value=\"3D_PAY\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"apiversion\" value=\"" + str2 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"terminalprovuserid\" value=\"" + str3 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"orderid\" value=\"" + orderID + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"terminaluserid\" value=\"" + terminalUserID + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"terminalmerchantid\" value=\"" + merchantID + "\"/>");
            builder1.AppendLine("<input type=\"hidden\" name=\"txntype\" value=\"" + str4 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"txnamount\" value=\"" + str5 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"txncurrencycode\" value=\"" + str6 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"txninstallmentcount\" value=\"" + str7 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"terminalid\" value=\"" + terminalID + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"successurl\" value=\"" + str15 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"errorurl\" value=\"" + str16 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"motoind\" value=\"N\"/>");
            builder1.AppendLine("<input type=\"hidden\" name=\"refreshtime\" value=\"10\"/>");
            builder1.AppendLine("<input type=\"hidden\" name=\"txntimestamp\" value=\"" + DateTime.Now.ToString() + "\"/>");
            builder1.AppendLine("<input type=\"hidden\" name=\"customeremailaddress\" value=\"" + posForm.Email + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"customeripaddress\" value=\"" + posForm.IPAdress + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"secure3dhash\" value=\"" + str18 + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"cardnumber\" value=\"" + posForm.CcNumber + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"cardexpiredatemonth\" value=\"" + posForm.ExpireMonth + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"cardexpiredateyear\" value=\"" + posForm.ExpireYear.Substring(2, 2) + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"cardcvv2\" value=\"" + posForm.Cvc + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"taksitAdet\" value=\"" + posForm.Installment + "\" />");
            builder1.AppendLine("<input type=\"hidden\" name=\"bankaAd\" value=\"garanti\" />");
            builder1.AppendLine("</form>");
            builder1.AppendLine("</body>");
            builder1.AppendLine("</html>");
            return builder1.ToString();
        }




    



        public override PaymentResult ConfirmPayment(HttpServerUtilityBase server, HttpRequestBase request, HttpSessionStateBase session, PosInfo gpos)
        {
            GarantiPosInfo pos = (GarantiPosInfo)gpos;
            string strMode = request.Form.Get("mode");
            string strApiVersion = request.Form.Get("apiversion");
            string strTerminalProvUserID = request.Form.Get("terminalprovuserid");
            string strType = request.Form.Get("txntype");
            string strAmount = request.Form.Get("txnamount");
            string strCurrencyCode = request.Form.Get("txncurrencycode");
            string strInstallmentCount = request.Form.Get("txninstallmentcount");
            string strTerminalUserID = request.Form.Get("terminaluserid");
            string strOrderID = request.Form.Get("oid");
            string strCustomeripaddress = request.Form.Get("customeripaddress");
            string strcustomeremailaddress = request.Form.Get("customeremailaddress");
            string strTerminalID = request.Form.Get("clientid");
            string _strTerminalID = "0" + strTerminalID;
            string strTerminalMerchantID = request.Form.Get("terminalmerchantid");
            string strStoreKey = pos.Password;
            //HASH doğrulaması için 3D Secure şifreniz
            string strProvisionPassword = pos.ProvisionPassword;
            //HASH doğrulaması için TerminalProvUserID şifresini tekrar yazıyoruz
            string strSuccessURL = request.Form.Get("successurl");
            string strErrorURL = request.Form.Get("errorurl");
          
            string transid = request.Form.Get("transid");
            string strCardholderPresentCode = "13";
            //3D Model işlemde bu değer 13 olmalı
            string strMotoInd = "N";
            string strNumber = "";
            //Kart bilgilerinin boş gitmesi gerekiyor
            string strExpireDate = "";
            //Kart bilgilerinin boş gitmesi gerekiyor
            string strCVV2 = "";
            //Kart bilgilerinin boş gitmesi gerekiyor
            string strAuthenticationCode = server.UrlEncode(request.Form.Get("cavv"));
            string strSecurityLevel = server.UrlEncode(request.Form.Get("eci"));
            string strTxnID = server.UrlEncode(request.Form.Get("xid"));
            string strMD = server.UrlEncode(request.Form.Get("md"));
            string strMDStatus = request.Form.Get("mdstatus");
            string strMDStatusText = request.Form.Get("mderrormessage");
            string strHostAddress = pos.PosURL;

            //Provizyon için xml'in post edileceği adres
            string SecurityData = GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();
            string HashData = GetSHA1(strOrderID + strTerminalID + strAmount + SecurityData).ToUpper();
            //Daha kısıtlı bilgileri HASH ediyoruz.

            //strMDStatus.Equals(1)
            //"Tam Doğrulama"
            //strMDStatus.Equals(2)
            //"Kart Sahibi veya bankası sisteme kayıtlı değil"
            //strMDStatus.Equals(3)
            //"Kartın bankası sisteme kayıtlı değil"
            //strMDStatus.Equals(4)
            //"Doğrulama denemesi, kart sahibi sisteme daha sonra kayıt olmayı seçmiş"
            //strMDStatus.Equals(5)
            //"Doğrulama yapılamıyor"
            //strMDStatus.Equals(6)
            //"3-D Secure Hatası"
            //strMDStatus.Equals(7)
            //"Sistem Hatası"
            //strMDStatus.Equals(8)
            //"Bilinmeyen Kart No"
            //strMDStatus.Equals(0)
            //"Doğrulama Başarısız, 3-D Secure imzası geçersiz."

            //Hashdata kontrolü için bankadan dönen secure3dhash değeri alınıyor.
            string strHashData = request.Form.Get("secure3dhash");
            string ValidateHashData = GetSHA1(strTerminalID + strOrderID + strAmount + strSuccessURL + strErrorURL + strType + strInstallmentCount + strStoreKey + SecurityData).ToUpper();

            //İlk gönderilen ve bankadan dönen HASH değeri yeni üretilenle eşleşiyorsa;

            if (strHashData == ValidateHashData)
            {
                
                //Tam Doğrulama, Kart Sahibi veya bankası sisteme kayıtlı değil, Kartın bankası sisteme kayıtlı değil
                //Doğrulama denemesi, kart sahibi sisteme daha sonra kayıt olmayı seçmiş responselarını alan
                //işlemler için Provizyon almaya çalışıyoruz
                if (strMDStatus == "1" | strMDStatus == "2" | strMDStatus == "3" | strMDStatus == "4")
                {
                  
                  return new PaymentResult() { Description = "Ödeme başarılı", TransID=transid, Result = true };
                   
                }
                else
                {
                    session["order"] = null;
                    return new PaymentResult() { Description = "İşlem başarısız", Result = false };
                }
            }
            else
            { 
                session["order"] = null;
                return new PaymentResult() { Result = false, Description = "İşlem başarısız. " + strMDStatusText + " Güvenlik Uyarısı. Sayısal Imza Geçerli Degil İşlem Başarısız" };
               
            }
        }

        private string GetSHA1(string SHA1Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            string HashedPassword = SHA1Data;
            byte[] hashbytes = Encoding.GetEncoding("ISO-8859-9").GetBytes(HashedPassword);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            return GetHexaDecimal(inputbytes);
        }
        private string GetHexaDecimal(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();
            int length = bytes.Length;
            for (int n = 0; n <= length - 1; n++)
            {
                s.Append(String.Format("{0,2:x}", bytes[n]).Replace(" ", "0"));
            }
            return s.ToString();
        }
    }
}
