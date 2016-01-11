﻿

using System;
using System.Globalization;
using System.Web.Mvc;
namespace DSLNG.PEAR.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString LimitString(this HtmlHelper htmlHelper, string source, int length)
        {
            if (!string.IsNullOrEmpty(source) && source.Length > length)
            {
                var result = source.Substring(0, length);
                result += "... <a class=\"see-more\" href=\"#\" data-toggle=\"modal\" data-target=\"#modalDialog\">See More</a>";
                result += "<div style=\"display:none;color:#fff\" class=\"full-string\">";
                result += source;
                result += "</div>";
                return MvcHtmlString.Create(result);
            }
            return MvcHtmlString.Create(source);
        }

        public static string ParseToDateOrNumber(this HtmlHelper htmlHelper, string val)
        {
            var resultDate = new DateTime();
            bool isValid = false;
            if (string.IsNullOrEmpty(val))
            {
                return val;
            }

            if (val.Length == 4)
            {
                return val;
            }
            if (val.Length == 6 || val.Length == 7)
            {
                if (val.Length == 6)
                {
                    isValid = DateTime.TryParseExact("01-" + val, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                                     DateTimeStyles.AllowWhiteSpaces, out resultDate);
                }
                else if (val.Length == 7)
                {
                    isValid = DateTime.TryParseExact("01-0" + val, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                                     DateTimeStyles.AllowWhiteSpaces, out resultDate);
                }
            }
            else if (val.Length == 21 || val.Length == 22)
            {
                isValid = DateTime.TryParseExact(val, "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDate);
            }
            else
            {
                isValid = DateTime.TryParseExact(val, "d-M-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out resultDate);
            }

            return isValid ? resultDate.ToString("dd MMM yyyy") : ParseToNumber(val);
        }

        public static string ParseToNumber(this HtmlHelper htmlHelper, string val)
        {
            return ParseToNumber(val);
        }

        private static string ParseToNumber(string val)
        {
            double x;
            bool isValidDouble = Double.TryParse(val, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out x);
            return isValidDouble ? x.ToString("#.000") : val;
        }
    }
}