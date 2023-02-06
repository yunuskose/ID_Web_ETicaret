using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace IDETicaret.Models
{
    public static class SeoFriendlyLink
    {
        public static string SeoLink(this UrlHelper urlHelper, Product entity)
        {
            string title = entity.Name.ToLower();
            title = FriendlyURLTitle(title);
            return string.Format("/Product/Detail/{0}/{1}", entity.Id.ToString(), title);
        }

        public static string SeoLinkCategories(this UrlHelper urlHelper, Product entity, string categori1, string categori2, string categori3, string categori4)
        {
            string title = entity.Name.ToLower();
            title = FriendlyURLTitle(title);
            return string.Format("/Product/Detail/{0}/{1}/{2}/{3}/{4}/{5}", entity.Id.ToString(), title, categori1,categori2,categori3,categori4);
        }

        public static string FriendlyURLTitle(string pTitle)
        {
            pTitle = pTitle.Replace(" ", "-");
            pTitle = pTitle.Replace(".", "-");
            pTitle = pTitle.Replace("ı", "i");
            pTitle = pTitle.Replace("İ", "I");

            pTitle = String.Join("", pTitle.Normalize(NormalizationForm.FormD) // türkçe karakterleri ingilizceye çevir.
                    .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

            pTitle = HttpUtility.UrlEncode(pTitle);
            return System.Text.RegularExpressions.Regex.Replace(pTitle, @"\%[0-9A-Fa-f]{2}", "");
        }
    }
}