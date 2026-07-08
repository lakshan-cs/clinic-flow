using ClinicFlow.Exceptions;
using ClinicFlow.Dto;
using ClinicFlow.Models;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services;
using Moq;
using Xunit;

namespace ClinicFlow.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsLoginResponse()
        {
            // Arrange
            var plainPassword = "SecurePass123!";
            var user = new User
            {
                Id = 1,
                Username = "johndoe",
                Email = "john@clinic.com",
                Password = BCrypt.Net.BCrypt.HashPassword(plainPassword),
                Role = "ADMIN"
            };
            var loginRequest = new LoginRequest { Email = user.Email, Password = plainPassword };
            userRepositoryMock.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);

            // Act
            var result = userService.Login(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id.ToString(), result.Id);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public void Login_WithNonExistentEmail_ThrowsInvalidCredentialsException()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "ghost@clinic.com", Password = "any_password" };
            userRepositoryMock.Setup(r => r.GetUserByEmail(loginRequest.Email)).Returns((User)null!);

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => userService.Login(loginRequest));
            Assert.Equal("Invalid email or password.", exception.Message);
        }

        [Fact]
        public void Login_WithWrongPassword_ThrowsInvalidCredentialsException()
        {
            // Arrange
            var user = new User
            {
                Id = 2,
                Username = "janedoe",
                Email = "jane@clinic.com",
                Password = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!"),
                Role = "ADMIN"
            };
            var loginRequest = new LoginRequest { Email = user.Email, Password = "WrongPassword!" };
            userRepositoryMock.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);

            // Act & Assert
            var exception = Assert.Throws<InvalidCredentialsException>(() => userService.Login(loginRequest));
            Assert.Equal("Invalid email or password.", exception.Message);
        }

        [Fact]
        public void Login_WithValidCredentials_MapsUserIdToStringInResponse()
        {
            // Arrange — verifies that numeric Id is correctly projected to a string in the response
            var plainPassword = "Pass@99Word";
            var user = new User
            {
                Id = 42,
                Username = "drsmith",
                Email = "drsmith@clinic.com",
                Password = BCrypt.Net.BCrypt.HashPassword(plainPassword),
                Role = "ADMIN"
            };
            var loginRequest = new LoginRequest { Email = user.Email, Password = plainPassword };
            userRepositoryMock.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);

            // Act
            var result = userService.Login(loginRequest);

            // Assert
            Assert.Equal("42", result.Id);
        }

        [Fact]
        public void Login_WithValidCredentials_DoesNotExposePasswordInResponse()
        {
            // Arrange — LoginResponse must not carry sensitive fields beyond Id, Username, Email
            var plainPassword = "Safe!Pass1";
            var user = new User
            {
                Id = 3,
                Username = "nurseamy",
                Email = "amy@clinic.com",
                Password = BCrypt.Net.BCrypt.HashPassword(plainPassword),
                Role = "ADMIN"
            };
            var loginRequest = new LoginRequest { Email = user.Email, Password = plainPassword };
            userRepositoryMock.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);

            // Act
            var result = userService.Login(loginRequest);

            // Assert
            Assert.IsType<LoginResponse>(result);
            Assert.Equal(3, typeof(LoginResponse).GetProperties().Length);
        }
    }
}
