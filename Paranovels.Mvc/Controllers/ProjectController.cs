﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paranovels.Mvc.Controllers
{
    public class ProjectController : SiteController
    {
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }
    }
}