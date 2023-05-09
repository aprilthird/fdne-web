using FDNE.PE.CORE.Helpers;
using FDNE.PE.DATA.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNDE.PE.WEB.PORTAL.Controllers
{
    public class BaseController : Controller
    {
        protected FdneContext _context;

        public BaseController()
        {
            _context = new FdneContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        protected string ToFileStorageUrl(string filePath)
        {
            return $"{ConfigurationManager.AppSettings[ConstantHelpers.KEYS.FILE_STORAGE]}/{filePath}";
        }

        protected string ToImageStorageUrl(string imgPath)
        {
            return $"{ConfigurationManager.AppSettings[ConstantHelpers.KEYS.IMAGE_STORAGE]}/{imgPath}";
        }
    }
}