using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRentalApp.Controllers;
using CarRentalApp.Models;
using CarRentalApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CarRentalApp.Tests
{
    public class ReservationControllerTests
    {
        private AppDbContext GetDatabase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public void Create_Post_ShouldFail_WhenDatesOverlap()
        {
            using var context = GetDatabase();
            var carId = 1;

            context.Reservations.Add(new Reservation
            {
                Id = 1,
                CarId = carId,
                StartDate = new DateTime(2025, 1, 10),
                EndDate = new DateTime(2025, 1, 15),
                UserId = "user1"
            });
            context.SaveChanges();

            var newReservation = new Reservation
            {
                CarId = carId,
                StartDate = new DateTime(2025, 1, 12),
                EndDate = new DateTime(2025, 1, 18)
            };

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var controller = new ReservationController(context, userManagerMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "user1")
            }, "mock"));
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var result = controller.Create(newReservation);

            Assert.False(controller.ModelState.IsValid);
            Assert.Equal("Wybrany zakres koliduje z istniej�c� rezerwacj�.",
                controller.ModelState[""]?.Errors[0].ErrorMessage);
        }
    }
}
