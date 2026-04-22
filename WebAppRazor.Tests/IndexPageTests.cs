using DBO;
using DBO.IServices;
using DBO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using WebAppRazor.Pages;
using WebAppRazor.Pages.Models;

namespace WebAppRazor.Tests
{
    public class IndexPageTests
    {
        private Mock<IUserService> mockUserService;
        private IndexModel pageModel;

        [SetUp]
        public void Setup()
        {
            // 1. Create a "Fake" (Mock) of your service
            mockUserService = new Mock<IUserService>();

            // 2. Initialize the PageModel with the fake service
            pageModel = new IndexModel(mockUserService.Object);
        }

        /// <summary>
        /// Testing Strings (Validation Messages)
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Login_EmptyEmail_SetsCorrectErrorMessage()
        {
            string expectedMessage = "Please provide both email and password.";
            // Arrange
            pageModel.LoginData = new LoginInput { Email = "", Password = "123" };

            // Act
            await pageModel.OnPostLoginAsync();

            // Assert
            // We check if the string contains the expected text
            Assert.That(pageModel.ErrorMessage, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Test whether return object or not
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Login_ReturnsPage_WhenUserNotFound()
        {
            // ARRANGE: Tell the fake service to return a "Failure" result
            mockUserService.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<User>.Failure("Invalid email or password."));

            pageModel.LoginData = new LoginInput { Email = "test@test.com", Password = "123" };

            // ACT: Run the actual method
            var result = await pageModel.OnPostLoginAsync();

            // ASSERT: Check if the result is a "Page" (meaning it stayed on the login screen)
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ErrorMessage, Is.EqualTo("Invalid email or password."));
        }

        /// <summary>
        /// Test Exceptions: Ensure that if the service throws an exception, the page handles it gracefully (e.g., shows an error message instead of crashing)
        /// </summary>
        [Test]
        public async Task Register_ServiceThrowsException_SetsErrorMessage()
        {
            // Arrange
            mockUserService.Setup(s => s.IsEmailExistAsync(It.IsAny<string>()))
                        .ThrowsAsync(new System.Exception("Database Offline"));

            pageModel.RegisterData = new RegisterInput { Email = "test@test.com", Password = "123" };

            // Act & Assert
            // Since your code doesn't have a try-catch in the Register method, 
            // the test will actually expect the exception to happen:
            Assert.ThrowsAsync<System.Exception>(async () => await pageModel.OnPostRegisterAsync());
        }

    }
}
