using FDNE.PE.WEB.PORTAL.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace FDNE.PE.WEB.TEST.Controllers
{
    public class HomeControllerTest
    {
        [Trait("Categoria", "Critico")]
        [Fact]
        public void Index()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index().Result as ViewResult;

            Assert.NotNull(result);
        }

        [Trait("Categoria", "Critico")]
        [Fact]
        public void About()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Test() as ViewResult;

            Assert.NotNull(result);
        }

        [Trait("Categoria", "Critico")]
        [Fact]
        public void Contact()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Layout() as ViewResult;

            Assert.NotNull(result);
        }
    }
}
