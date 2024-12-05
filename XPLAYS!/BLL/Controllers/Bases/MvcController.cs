using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BLL.Controllers.Bases
{
     public abstract class MvcController : Controller
    {
        protected MvcController() { 
        CultureInfo cultureInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;  
            Thread.CurrentThread.CurrentUICulture = cultureInfo;    
        }
    }
}
