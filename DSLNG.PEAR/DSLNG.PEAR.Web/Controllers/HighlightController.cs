﻿using DevExpress.Web.Mvc;
using DSLNG.PEAR.Data.Enums;
using DSLNG.PEAR.Services.Interfaces;
using DSLNG.PEAR.Services.Requests.Highlight;
using DSLNG.PEAR.Web.ViewModels.Highlight;
using System;
using System.Web.Mvc;
using PeriodeType = DSLNG.PEAR.Data.Enums.PeriodeType;
using DSLNG.PEAR.Common.Extensions;

namespace DSLNG.PEAR.Web.Controllers
{
    public class HighlightController : BaseController
    {
        private IHighlightService _highlightService;
        public HighlightController(IHighlightService highlightService)
        {
            _highlightService = highlightService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexPartial()
        {
            var viewModel = GridViewExtension.GetViewModel("gridArtifactIndex");
            if (viewModel == null)
                viewModel = CreateGridViewModel();
            return BindingCore(viewModel);
        }

        PartialViewResult BindingCore(GridViewModel gridViewModel)
        {
            gridViewModel.ProcessCustomBinding(
                GetDataRowCount,
                GetData
            );
            return PartialView("_IndexGridPartial", gridViewModel);
        }

        static GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "Id";
            viewModel.Columns.Add("PeriodeType");
            viewModel.Columns.Add("Type");
            viewModel.Columns.Add("Title");
            viewModel.Columns.Add("Date");
            viewModel.Pager.PageSize = 10;
            return viewModel;
        }

        public ActionResult PagingAction(GridViewPagerState pager)
        {
            var viewModel = GridViewExtension.GetViewModel("gridHighlightIndex");
            viewModel.ApplyPagingState(pager);
            return BindingCore(viewModel);
        }

        public void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e)
        {

            e.DataRowCount = _highlightService.GetHighlights(new GetHighlightsRequest { OnlyCount = true }).Count;
        }

        public void GetData(GridViewCustomBindingGetDataArgs e)
        {
            e.Data = _highlightService.GetHighlights(new GetHighlightsRequest
            {
                Skip = e.StartDataRowIndex,
                Take = e.DataRowCount
            }).Highlights;
        }

        //
        // GET: /Highlight/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Highlight/Create
        public ActionResult Create()
        {
            var viewModel = new HighlightViewModel();
            foreach (var name in Enum.GetNames(typeof(PeriodeType)))
            {
                if (!name.Equals("Hourly") && !name.Equals("Weekly"))
                {
                    viewModel.PeriodeTypes.Add(new SelectListItem { Text = name, Value = name });
                }
            }
            foreach (var name in Enum.GetNames(typeof(HighlightType)))
            {
                viewModel.Types.Add(new SelectListItem { Text = name, Value = name });
            }
            return View(viewModel);
        }

        //
        // POST: /Highlight/Create
        [HttpPost]
        public ActionResult Create(HighlightViewModel viewModel)
        {
            var req = viewModel.MapTo<SaveHighlightRequest>();
            _highlightService.SaveHighlight(req);
            return RedirectToAction("Index");
        }

        //
        // GET: /Highlight/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Highlight/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Highlight/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Highlight/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
