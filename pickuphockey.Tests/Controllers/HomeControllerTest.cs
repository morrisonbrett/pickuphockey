using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pickuphockey.Controllers;

namespace pickuphockey.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // TODO Setup Entity framework
            // Arrange
            //var controller = new HomeController();

            // Act
            //var result = controller.Index() as ViewResult;

            // Assert
            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(System.Configuration.ConfigurationManager.AppSettings["SiteTitle"], result.ViewBag.Message);
        }
    }
}
