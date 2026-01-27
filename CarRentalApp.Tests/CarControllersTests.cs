using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRentalApp.Controllers;
using CarRentalApp.Models;
using CarRentalApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace CarRentalApp.Tests
{
	public class CarControllerTests
	{
		private AppDbContext GetDatabase()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
				.Options;
			return new AppDbContext(options);
		}

		[Fact]
		public void Index_ReturnsViewWithCars()
		{
			using var context = GetDatabase();
			context.Cars.Add(new Car { Id = 1, Make = "Tesla", Model = "S", Year = 2022 });
			context.SaveChanges();

			var controller = new CarController(context);

			var httpContext = new DefaultHttpContext();
			controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

			var result = controller.Index();

			var viewResult = Assert.IsType<ViewResult>(result);
			var model = Assert.IsAssignableFrom<List<Car>>(viewResult.ViewData.Model);
			Assert.Single(model);
		}

		[Fact]
		public void Details_ReturnsNotFound_WhenCarNotExists()
		{
			using var context = GetDatabase();
			var controller = new CarController(context);

			var result = controller.Details(999);

			Assert.IsType<NotFoundResult>(result);
		}
	}
}
