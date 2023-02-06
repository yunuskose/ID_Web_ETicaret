using IDETicaret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDETicaret.Helpers
{
    public class VendorHelper
    {
        private static ETicaretEntities ocmde = new ETicaretEntities();

        public static bool checkExpires(int vendorId)
        {
            try
            {
                var lastMemberShip = ocmde.MemberShipVendor.Where(m => m.VendorId == vendorId).OrderByDescending(m => m.Id).First();
                return lastMemberShip.EndDate >= DateTime.Now;
            }
            catch
            {
                return false;
            }
        }
    }
}