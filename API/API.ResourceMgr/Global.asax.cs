﻿using System;
using System.Web.Http;
using System.Web.Mvc;

namespace API.ResourceMgr
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); 
        }
         
    }
}