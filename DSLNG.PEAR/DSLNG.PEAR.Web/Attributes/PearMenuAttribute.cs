﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DSLNG.PEAR.Services;
using DSLNG.PEAR.Services.Interfaces;
using DSLNG.PEAR.Web.DependencyResolution;
using DSLNG.PEAR.Services.Requests.Menu;

namespace DSLNG.PEAR.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class PearMenuAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.HttpContext.Request.RequestContext.RouteData.Values["Controller"].ToString().ToLower();
            var action = filterContext.HttpContext.Request.RequestContext.RouteData.Values["Action"].ToString().ToLower();

            var _menuService = ObjectFactory.Container.GetInstance<IMenuService>();
            var rootMenuActive = _menuService.GetSiteMenuActive(new GetSiteMenuActiveRequest() { Action = action, Controller = controller });
            filterContext.Controller.TempData.Add("RootMenuActive", rootMenuActive);

            base.OnActionExecuting(filterContext);
        }
    }
}