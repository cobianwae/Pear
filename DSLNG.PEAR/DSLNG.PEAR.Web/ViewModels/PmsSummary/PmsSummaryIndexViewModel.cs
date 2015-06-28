﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DSLNG.PEAR.Web.ViewModels.PmsSummary
{
    public class PmsSummaryIndexViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public IEnumerable<PmsSummaryViewModel> PmsSummaries { get; set; }
        public IEnumerable<SelectListItem> MonthList
        {
            get
            {
                return DateTimeFormatInfo
                   .InvariantInfo
                   .MonthNames
                   .Where(m => !String.IsNullOrEmpty(m))
                   .Select((monthName, index) => new SelectListItem
                   {
                       Value = (index + 1).ToString(),
                       Text = monthName
                   });
            }
        }
        public IEnumerable<SelectListItem> YearList
        {
            get
            {
                var years = new List<int>();
                for (int i = 0; i <= 5; i++)
                {
                    years.Add(DateTime.Now.AddYears(-i).Year);
                }
                return years.Select(x => new SelectListItem
                    {
                        Value = x.ToString(),
                        Text = x.ToString()
                    });
            }
        }
    }
}