using ClinicFlow.Controllers;
using ClinicFlow.Dto;
using ClinicFlow.Exceptions;
using ClinicFlow.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicFlow.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> userServiceMock;
        private readonly UserController userController;

        public UserControllerTests()
        {
            userServiceMock = new Mock<IUserService>();
            userController = new UserController(userServiceMock.Object);
        }

        [Fact]
        public void Login_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "john@clinic.com", Password = "SecurePass123!" };
            var loginResponse = new LoginResponse { Id = "1", Username = "johndoe", Email = "john@clinic.com" };
            userServiceMock.Setup(s => s.Login(loginRequest)).Returns(loginResponse);

            // Act
            var actionResult = userController.Login(loginRequest);

            // Assert
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public void Login_WithValidRequest_ResponseBodyContainsLoginData()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "john@clinic.com", Password = "SecurePass123!" };
            var loginResponse = new LoginResponse { Id = "1", Username = "johndoe", Email = "john@clinic.com" };
            userServiceMock.Setup(s => s.Login(loginRequest)).Returns(loginResponse);

            // Act
            var actionResult = userController.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var response = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal(loginResponse.Id, response.Id);
            Assert.Equal(loginResponse.Username, response.Username);
            Assert.Equal(loginResponse.Email, response.Email);
        }

        [Fact]
        public void Login_WithValidRequest_Returns200StatusCode()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "john@clinic.com", Password = "SecurePass123!" };
            var loginResponse = new LoginResponse { Id = "1", Username = "johndoe", Email = "john@clinic.com" };
            userServiceMock.Setup(s => s.Login(loginRequest)).Returns(loginResponse);

            // Act
            var actionResult = userController.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ExceptionPropagatesFromService()
        {
            // Arrange — controller has no try/catch; exception is handled by ExceptionMiddleware
            var loginRequest = new LoginRequest { Email = "wrong@clinic.com", Password = "wrong_password" };
            userServiceMock
                .Setup(s => s.Login(loginRequest))
                .Throws(new InvalidCredentialsException("Invalid email or password."));

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(
                () => userController.Login(loginRequest));
            Assert.Equal("Invalid email or password.", exception.Message);
        }

        [Fact]
        public void Login_WithAnyRequest_DelegatesLoginToServiceExactlyOnce()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "john@clinic.com", Password = "SecurePass123!" };
            var loginResponse = new LoginResponse { Id = "1", Username = "johndoe", Email = "john@clinic.com" };
            userServiceMock.Setup(s => s.Login(loginRequest)).Returns(loginResponse);

            // Act
            userController.Login(loginRequest);

            // Assert
            userServiceMock.Verify(s => s.Login(loginRequest), Times.Once);
        }
    }
}
