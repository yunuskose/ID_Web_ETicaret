using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IDETicaret.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using IDETicaret.Manager;

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
            get { 
                return ccManager; 
            }
        }

        public static string Banka = "";
        private Front()
        {
            FrontIslemlerGetir();
        }

        public static ETicaretEntities ocmde = new ETicaretEntities();

        private static void FrontIslemlerGetir()
        {
            List<PosInfo> posInfoList = new List<PosInfo>();

            //İş Bankası
            Bankalar dtIs = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Is" && x.Aktif == true).FirstOrDefault();

            if (dtIs != null)
            {
                posInfoList.Add(new PosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.ISBANKASI,
                    BankName = Convert.ToString(dtIs.BankName),
                    ClientID = Convert.ToString(dtIs.ClientID),
                    Name = Convert.ToString(dtIs.Name),
                    Password = Convert.ToString(dtIs.Password),
                    ekPassword = Convert.ToString(dtIs.ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.EST,
                    PosURL = Convert.ToString(dtIs.PosUrl)
                });
            }
            //Akbank Bankası
         
            Bankalar dtAkbank = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Akbank" && x.Aktif == true).FirstOrDefault();

            if (dtAkbank != null)
            {
                posInfoList.Add(new PosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.AKBANK,
                    BankName = Convert.ToString(dtAkbank.BankName),
                    ClientID = Convert.ToString(dtAkbank.ClientID),
                    Name = Convert.ToString(dtAkbank.Name),
                    Password = Convert.ToString(dtAkbank.Password),
                    ekPassword = Convert.ToString(dtIs.ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.EST,
                    PosURL = Convert.ToString(dtAkbank.PosUrl)
                });
            }
            //Garanti Bankası
            Bankalar dtGaranti = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Garanti" && x.Aktif == true).FirstOrDefault();

            if (dtGaranti != null)
            {
                posInfoList.Add(new GarantiPosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.GARANTI,
                    TerminalID = Convert.ToString(dtGaranti.TerminalID),
                    MerchantID = Convert.ToString(dtGaranti.MarchantID),
                    TerminalUserID = Convert.ToString(dtGaranti.TerminalUserID),
                    BankName = Convert.ToString(dtGaranti.BankName),
                    ClientID = Convert.ToString(dtGaranti.ClientID),
                    Name = Convert.ToString(dtGaranti.Name),
                    Password = Convert.ToString(dtGaranti.Password),
                    ProvisionPassword = Convert.ToString(dtGaranti.ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.GARANTI,
                    PosURL = Convert.ToString(dtGaranti.PosUrl)
                });
            }
            //Vakif
            Bankalar dtYP = ocmde.Bankalar.Where((x) => x.Sirket == "Ozerdem" && x.Banka == "Vakif" && x.Aktif == true).FirstOrDefault();

            if (dtYP != null)
            {
                posInfoList.Add(new YKBPosInfo()
                {
                    Bank = IDETicaret.Banka.Entity.Banks.YAPIKREDI,
                    BankName = Convert.ToString(dtYP.BankName),
                    TerminalID = Convert.ToString(dtYP.TerminalID),
                    MerchantID = Convert.ToString(dtYP.MarchantID),
                    ClientID = Convert.ToString(dtYP.ClientID),
                    Name = Convert.ToString(dtYP.Name),
                    Password = Convert.ToString(dtYP.Password),
                    ekPassword = Convert.ToString(dtYP.ProvisionPassword),
                    PosType = IDETicaret.Banka.Entity.PosType.YAPIKREDI,
                    PosURL = Convert.ToString(dtYP.PosUrl),
                    Key = Convert.ToString(dtYP.ProvisionPassword)
                });
            }

            //posInfoList.Add(new YKBPosInfo() { Bank = MvcIsTakip.Banka.Entity.Banks.YAPIKREDI, TerminalID = "", MerchantID = "", ClientID = "", BankName = "yapikredi", Name = "", Key = "10,10,10,10,10,10,10,10", PosType = MvcIsTakip.Banka.Entity.PosType.YAPIKREDI, PosURL = "http://setmpos.ykb.com/3DSWebService/YKBPaymentService" });

            if (Banka == "Garanti")
                if (dtGaranti != null)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtGaranti.okURL), Convert.ToString(dtGaranti.failURL), Convert.ToString(dtGaranti.receiveURL),
                        posInfoList);
                }

            if (Banka == "Akbank")
                if (dtAkbank != null)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtAkbank.okURL), Convert.ToString(dtAkbank.failURL), Convert.ToString(dtAkbank.receiveURL),
                        posInfoList);
                }

            if (Banka == "IsBankasi")
                if (dtIs != null)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtIs.okURL), Convert.ToString(dtIs.failURL), Convert.ToString(dtIs.receiveURL),
                        posInfoList);
                }

            if (Banka == "Vakif")
                if (dtIs != null)
                {
                    ccManager = CCManager.CreateInstance(
                            Convert.ToString(dtYP.okURL), Convert.ToString(dtYP.failURL), Convert.ToString(dtYP.receiveURL),
                        posInfoList);
                }

        }

    }
}