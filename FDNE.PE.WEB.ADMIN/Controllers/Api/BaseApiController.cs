using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FDNE.PE.DATA.Context;
using FDNE.PE.DATA.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FDNE.PE.WEB.ADMIN.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        protected FdneContext _context;
        protected ApplicationUserManager _userManager;

        protected string UserId => User.Identity.GetUserId();

        protected Task<ApplicationUser> UserAsync => _userManager.FindByIdAsync(User.Identity.GetUserId()); 

        public BaseApiController()
        {
            _context = new FdneContext();
            var userStore = new UserStore<ApplicationUser>(_context);
            _userManager = new ApplicationUserManager(userStore);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        protected string SaveImage(string allBase64, string foler)
        {
            if (string.IsNullOrEmpty(allBase64))
                return null;
            try
            {
                var base64 = allBase64.Split(',')[1];

                base64 = base64.Replace(" ", "+");
                var imageBytes = Convert.FromBase64String(base64);
                string filePath;
                string fileName;
                using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    //Convert byte[] to Image
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                    fileName = Guid.NewGuid().ToString() + ".jpeg";
                    filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/img/upLoadImage/" + foler + "/"), fileName);
                    image.Save(filePath, ImageFormat.Jpeg);
                }
                return $"{foler}/{fileName}";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        protected string SaveFile (string allBase64, string extension)
        {
            if (string.IsNullOrEmpty(allBase64))
                return null;
            try
            {
                var base64 = allBase64.Split(',')[1];

                base64 = base64.Replace(" ", "+");

                Byte[] bytes = Convert.FromBase64String(base64);

                string fileName = Guid.NewGuid().ToString() + "." + extension;
                string filePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/"), fileName);
                File.WriteAllBytes(filePath, bytes);

                return fileName;
            }
            catch(Exception e)
            {
                return "";
            }
        }

        protected string ToFileStorageUrl(string filePath)
        {
            //return Url.Content($"~/Content/files/${filePath}");
            return Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/files/"), filePath);
        }

        protected string ToImageStorageUrl(string imgPath)
        {
            //return Url.Content($"~/Content/img/upLoadImage/${imgPath}");
            return Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/img/upLoadImage/"), imgPath);
        }

        protected IEnumerable<string> ValidateDocument(DocumentType documentType, string document)
        {
            var errors = new List<string>();
            if (documentType.ExactLength ? document.Length != documentType.Length : document.Length > documentType.Length)
                errors.Add($"El documento debe contener {(documentType.ExactLength ? "exactamente" : "menos de")} {documentType.Length} caracteres.");
            if (documentType.IsNumeric && !document.All(char.IsDigit))
                errors.Add($"El documento debe contener solo dígitos.");
            return errors;
        }

        protected bool DeleteFile(string filePath)
        {
            var absoluteUrl = ToFileStorageUrl(filePath);
            if (File.Exists(absoluteUrl))
            {
                File.Delete(absoluteUrl);
                return true;
            }
            return false;
        }

        protected bool DeleteImage(string imgPath)
        {
            var absoluteUrl = ToImageStorageUrl(imgPath);
            if (File.Exists(absoluteUrl))
            {
                File.Delete(absoluteUrl);
                return true;
            }
            return false;
        }
    }
}
