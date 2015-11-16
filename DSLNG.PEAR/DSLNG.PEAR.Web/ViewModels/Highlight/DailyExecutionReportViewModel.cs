﻿

using System;
using System.Collections.Generic;
using DSLNG.PEAR.Data.Enums;

namespace DSLNG.PEAR.Web.ViewModels.Highlight
{
    public class DailyExecutionReportViewModel
    {
        public DailyExecutionReportViewModel() {
            NLSList = new List<NLSViewModel>();
            Highlights = new List<HighlightViewModel>();
            Alert = new AlertViewModel();
            Weather = new WeatherViewModel();
            HighlightGroups = new List<HighlightGroupViewModel>();
            HighlightGroupTemplates = new List<HighlightGroupViewModel>();
        }
        public IList<NLSViewModel> NLSList { get; set; }
        public IList<HighlightViewModel> Highlights { get; set; }
        public IList<HighlightViewModel> PlantOperations { get; set; }
        public IList<HighlightGroupViewModel> HighlightGroups { get; set; }
        public IList<HighlightGroupViewModel> HighlightGroupTemplates { get; set; }
        public AlertViewModel Alert {get;set;}
        public WeatherViewModel Weather {get;set;}
        public class NLSViewModel {
            public int Id { get; set; }
            public string Type { get; set; }
            public string VesselType { get; set; }
            public string Vessel { get; set; }
            public double Capacity { get; set; }
            public DateTime ETA { get; set; }
            public DateTime ETD { get; set; }
            public string Cargo { get; set; }
            public string Remark { get; set; }
            public string Buyer { get; set; }
            public DateTime RemarkDate { get; set; }
            public string Measurement { get; set; }
        }
        public class WeatherViewModel {
            public string Value { get; set; }
            public string Text { get; set; }
            public string Temperature { get; set; }
        }
        public class AlertViewModel {
            public string Message { get; set; }
        }
        public class HighlightGroupViewModel {
            public HighlightGroupViewModel() {
                Highlights = new List<HighlightViewModel>();
                HighlightTypes = new List<HighlightTypeViewModel>();
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }
            public IList<HighlightTypeViewModel> HighlightTypes { get; set; }
            public IList<HighlightViewModel> Highlights { get; set; }
        }
        public class HighlightTypeViewModel{
            public int Id { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }
            public int Order { get; set; }
        }
        public class HighlightViewModel {
            public string Title { get; set; }
            public string Message { get; set; }
            public int Order { get; set; }
            public string Type { get; set; }
            public int TypeId { get; set; }
        }
        public PeriodeType PeriodeType { get; set; }
       
    }
}