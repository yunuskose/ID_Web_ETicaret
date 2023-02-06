using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDETicaret.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace IDETicaret
{
    public class Front
    {
        private static Front controller;
        public static Front Controller
        {
            get
            {
                if (controller == null)
                    controller = new Front();
                FrontIslemlerGetir();
                return controller;
            }
        }

        private static CCManager ccManager;
        public CCManager CCManager
        {
            get
            {
                return ccManager;
            }
        }

        public static string Banka = "";
        private Front()
        {
            FrontIslemlerGetir();
        }

        private static void FrontIslemlerGetir()
        {
            ETicaretEntities ocmde = new ETicaretEntities();
            List<PosInfo> posInfoList = new List<PosInfo>();

            //İş Bankası
            List<Bankalar> dtIs = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Is" && x.Aktif == true).ToList();
            if (dtIs.Count > 0)
            {
                posInfoList.Add(new PosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.ISBANKASI,
                    BankName = Convert.ToString(dtIs[0].BankName),
                    ClientID = Convert.ToString(dtIs[0].ClientID),
                    Name = Convert.ToString(dtIs[0].Name),
                    Password = Convert.ToString(dtIs[0].Password),
                    ekPassword = Convert.ToString(dtIs[0].ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.EST,
                    PosURL = Convert.ToString(dtIs[0].PosUrl)
                });
            }
            //Akbank Bankası
           List<Bankalar> dtAkbank = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Akbank" && x.Aktif == true).ToList();
            if (dtAkbank.Count > 0)
            {
                posInfoList.Add(new PosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.AKBANK,
                    BankName = Convert.ToString(dtAkbank[0].BankName),
                    ClientID = Convert.ToString(dtAkbank[0].ClientID),
                    Name = Convert.ToString(dtAkbank[0].Name),
                    Password = Convert.ToString(dtAkbank[0].Password),
                    ekPassword = Convert.ToString(dtIs[0].ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.EST,
                    PosURL = Convert.ToString(dtAkbank[0].PosUrl)
                });
            }
            //Garanti Bankası
            List<Bankalar> dtGaranti = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Garanti" && x.Aktif == true).ToList();
            if (dtGaranti.Count > 0)
            {
                posInfoList.Add(new GarantiPosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.GARANTI,
                    TerminalID = Convert.ToString(dtGaranti[0].TerminalID),
                    MerchantID = Convert.ToString(dtGaranti[0].MarchantID),
                    TerminalUserID = Convert.ToString(dtGaranti[0].TerminalUserID),
                    BankName = Convert.ToString(dtGaranti[0].BankName),
                    ClientID = Convert.ToString(dtGaranti[0].ClientID),
                    Name = Convert.ToString(dtGaranti[0].Name),
                    Password = Convert.ToString(dtGaranti[0].Password),
                    ProvisionPassword = Convert.ToString(dtGaranti[0].ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.GARANTI,
                    PosURL = Convert.ToString(dtGaranti[0].PosUrl)
                });
            }
            //Yapı Kredi
            List<Bankalar> dtYP = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "YapiKredi" && x.Aktif == true).ToList();
            if (dtYP.Count > 0)
            {
                posInfoList.Add(new YKBPosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.YAPIKREDI,
                    BankName = Convert.ToString(dtYP[0].BankName),
                    TerminalID = Convert.ToString(dtYP[0].TerminalID),
                    MerchantID = Convert.ToString(dtYP[0].MarchantID),
                    ClientID = Convert.ToString(dtYP[0].ClientID),
                    Name = Convert.ToString(dtYP[0].Name),
                    Password = Convert.ToString(dtYP[0].Password),
                    ekPassword = Convert.ToString(dtYP[0].ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.YAPIKREDI,
                    PosURL = Convert.ToString(dtYP[0].PosUrl),
                    Key = Convert.ToString(dtYP[0].ProvisionPassword)
                });
            }

            //posInfoList.Add(new YKBPosInfo() { Bank = MvcIsTakip.Banka.Entity.Banks.YAPIKREDI, TerminalID = "", MerchantID = "", ClientID = "", BankName = "yapikredi", Name = "", Key = "10,10,10,10,10,10,10,10", PosType = MvcIsTakip.Banka.Entity.PosType.YAPIKREDI, PosURL = "http://setmpos.ykb.com/3DSWebService/YKBPaymentService" });

            if (Banka == "Garanti")
                if (dtGaranti.Count > 0)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtGaranti[0].okURL), Convert.ToString(dtGaranti[0].failURL), Convert.ToString(dtGaranti[0].receiveURL),
                        posInfoList);
                }

            if (Banka == "Akbank")
                if (dtAkbank.Count > 0)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtAkbank[0].okURL), Convert.ToString(dtAkbank[0].failURL), Convert.ToString(dtAkbank[0].receiveURL),
                        posInfoList);
                }

            if (Banka == "IsBankasi")
                if (dtIs.Count > 0)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtIs[0].okURL), Convert.ToString(dtIs[0].failURL), Convert.ToString(dtIs[0].receiveURL),
                        posInfoList);
                }

            if (Banka == "YapiKredi")
                if (dtIs.Count > 0)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtYP[0].okURL), Convert.ToString(dtYP[0].failURL), Convert.ToString(dtYP[0].receiveURL),
                        posInfoList);
                }

        }

    }
}