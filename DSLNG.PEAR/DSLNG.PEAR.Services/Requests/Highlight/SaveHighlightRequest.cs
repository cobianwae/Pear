﻿
using DSLNG.PEAR.Data.Enums;
using System;
namespace DSLNG.PEAR.Services.Requests.Highlight
{
    public class SaveHighlightRequest
    {
        public int Id { get; set; }
        public PeriodeType PeriodeType { get; set; }
        public DateTime Date { get; set; }
        public HighlightType Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}