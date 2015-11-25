﻿using DSLNG.PEAR.Data.Enums;
using DSLNG.PEAR.Services.Interfaces;
using DSLNG.PEAR.Services.Requests.Measurement;
using DSLNG.PEAR.Services.Requests.OutputCategory;
using DSLNG.PEAR.Services.Requests.OutputConfig;
using DSLNG.PEAR.Web.Grid;
using DSLNG.PEAR.Web.ViewModels.OutputConfig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSLNG.PEAR.Common.Extensions;

namespace DSLNG.PEAR.Web.Controllers
{
    public class OutputConfigController : BaseController
    {
        private readonly IMeasurementService _measurementService;
        private readonly IOutputCategoryService _outputCategoryService;
        private readonly IOutputConfigService _outputConfigService;

        public OutputConfigController(IMeasurementService measurementService,IOutputCategoryService outputCategoryService,
            IOutputConfigService outputConfigService) {
            _measurementService = measurementService;
            _outputCategoryService = outputCategoryService;
            _outputConfigService = outputConfigService;
        }
        //
        // GET: /OutputConfig/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create() {
            var viewModel = new OutputConfigViewModel();
            viewModel.Measurements = _measurementService.GetMeasurements(new GetMeasurementsRequest { 
                Take = -1,
                SortingDictionary = new SortedDictionary<string,SortOrder>{{"Name",SortOrder.Ascending}}
            }).Measurements.Select(x => new SelectListItem{Text = x.Name, Value = x.Id.ToString()}).ToList();

            viewModel.OutputCategories = _outputCategoryService.GetOutputCategories(new GetOutputCategoriesRequest
            {
                Take = -1,
                SortingDictionary = new SortedDictionary<string, SortOrder> {{"Order", SortOrder.Ascending} }
            }).OutputCategories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            foreach (var name in Enum.GetNames(typeof(Formula)))
            {
                viewModel.Formulas.Add(new SelectListItem { Text = name, Value = name });
            }
            viewModel.IsActive = true;
            return View(viewModel);
        }
        
        [HttpPost]
        public ActionResult Create(OutputConfigViewModel viewModel) {
            var request = viewModel.MapTo<SaveOutputConfigRequest>();
            var resp = _outputConfigService.Save(request);
            TempData["IsSuccess"] = resp.IsSuccess;
            TempData["Message"] = resp.Message;
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var viewModel = _outputConfigService.Get(new GetOutputConfigRequest { Id = id }).MapTo<OutputConfigViewModel>();
            viewModel.Measurements = _measurementService.GetMeasurements(new GetMeasurementsRequest
            {
                Take = -1,
                SortingDictionary = new SortedDictionary<string, SortOrder> { { "Name", SortOrder.Ascending } }
            }).Measurements.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            viewModel.OutputCategories = _outputCategoryService.GetOutputCategories(new GetOutputCategoriesRequest
            {
                Take = -1,
                SortingDictionary = new SortedDictionary<string, SortOrder> { { "Order", SortOrder.Ascending } }
            }).OutputCategories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            foreach (var name in Enum.GetNames(typeof(Formula)))
            {
                viewModel.Formulas.Add(new SelectListItem { Text = name, Value = name });
            }
            viewModel.IsActive = true;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(OutputConfigViewModel viewModel)
        {
            var request = viewModel.MapTo<SaveOutputConfigRequest>();
            var resp = _outputConfigService.Save(request);
            TempData["IsSuccess"] = resp.IsSuccess;
            TempData["Message"] = resp.Message;
            return RedirectToAction("Index");
        }

        public ActionResult EconomicKpis(string term) {
            return Json(new { results = _outputConfigService.GetKpis(new GetKpisRequest { Term = term }).KpiList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult KeyAssumptions(string term) {
            return Json(new { results = _outputConfigService.GetKeyAssumptions(new GetKeyAssumptionsRequest { Term = term }).KeyAssumptions }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Grid(GridParams gridParams)
        {
            var outputConfig = _outputConfigService.GetOutputConfigs(new GetOutputConfigsRequest
                {
                    Skip = gridParams.DisplayStart,
                    Take = gridParams.DisplayLength,
                    Search = gridParams.Search,
                    SortingDictionary = gridParams.SortingDictionary
                });
            var data = new
            {
                sEcho = gridParams.Echo + 1,
                iTotalDisplayRecords = outputConfig.TotalRecords,
                iTotalRecords = outputConfig.OutputConfigs.Count,
                aaData = outputConfig.OutputConfigs.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Category,
                    x.Measurement,
                    Formula = x.Formula.ToString(),
                    x.Order,
                    x.Remark,
                    x.IsActive
                    
                })
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}