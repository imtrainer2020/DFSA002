// Ensure the Microsoft.EntityFrameworkCore.InMemory NuGet package is referenced by the test project.
// Example (in the test .csproj):
// <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="7.0.0" />

using DBO.Models;
using DBO.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace DBO.Tests
{
    [TestFixture]
    public class UserRoleServiceTests
    {
        private AppDbContext _context;
        private UserRoleService _service;
        private string roleName = "Admin";

        [SetUp]
        public void Setup()
        {
            // Create a fresh In-Memory Database for EACH test to ensure strict isolation
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new UserRoleService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose the context to satisfy NUnit1032 and release resources after each test
            _context?.Database?.EnsureDeleted();
            _context?.Dispose();
            _context = null;
            _service = null;
        }

        #region AddRoleAsync Tests

        [Test]
        public async Task AddRoleAsync_ValidRole_ReturnsSuccess()
        {
            // Act
            var result = await _service.AddRoleAsync(roleName);

            // Assert
            Assert.That(result.IsSuccess, Is.True); // Assuming Result has IsSuccess=true 

            var savedRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            Assert.That(savedRole, Is.Not.Null);
        }

        [Test]
        public async Task AddRoleAsync_DbException_ReturnsFailure()
        {
            // Arrange
            // Passing a null context to force an exception in the try block
            var brokenService = new UserRoleService(null);

            // Act
            var result = await brokenService.AddRoleAsync(roleName);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.StartWith("Error:"));
        }

        #endregion

        #region DeleteRoleAsync Tests

        [Test]
        public async Task DeleteRoleAsync_ExistingRole_ReturnsSuccess()
        {
            roleName = "Manager";

            // Arrange: Seed the database
            _context.UserRoles.Add(new UserRole { RoleName = roleName });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteRoleAsync(roleName);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo("Role deleted successfully."));
            Assert.That(_context.UserRoles.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteRoleAsync_RoleNotFound_ReturnsFailure()
        {
            // Act
            var result = await _service.DeleteRoleAsync("NonExistentRole");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
        }

        #endregion

        #region GetAllRolesAsync Tests

        [Test]
        public async Task GetAllRolesAsync_ReturnsAllExistingRoles()
        {
            // Arrange
            _context.UserRoles.AddRange(
                new UserRole { RoleName = "Admin" },
                new UserRole { RoleName = "User" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllRolesAsync();

            // Assert
            // Assuming result.Data holds the List<UserRole>
            Assert.That(result.Data, Has.Count.EqualTo(2));
            Assert.That(result.IsSuccess, Is.True);
        }

        #endregion

        #region GetRoleByNameAsync Tests

        [Test]
        public async Task GetRoleByNameAsync_ReturnsExistingRole()
        {
            roleName = "HR";
            // Arrange
            _context.UserRoles.Add(new UserRole { RoleName = roleName });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetRoleByNameAsync(roleName);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data?.RoleName, Is.EqualTo(roleName));
        }

        #endregion

        #region UpdateRoleAsync Tests

        [Test]
        public async Task UpdateRoleAsync_UpdateExistingRole()
        {
            roleName = "OldName";
            string newRoleName = "NewName";

            // Arrange: Seed the database
            _context.UserRoles.Add(new UserRole { RoleName = roleName });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.UpdateRoleAsync(roleName, newRoleName);

            // Assert
            Assert.That(_context.UserRoles.First().RoleName, Is.EqualTo(newRoleName));
        }

        #endregion

    }
}
