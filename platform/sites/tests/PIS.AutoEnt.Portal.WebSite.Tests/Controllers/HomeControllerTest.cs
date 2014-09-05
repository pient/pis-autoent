using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Portal.WebSite;
using PIS.AutoEnt.Portal.WebSite.Controllers;

namespace PIS.AutoEnt.Portal.WebSite.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeController_Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index(null) as ViewResult;

            // Assert
            // Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }
    }
}
