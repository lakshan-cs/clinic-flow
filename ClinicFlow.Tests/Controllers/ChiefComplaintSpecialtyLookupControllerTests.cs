using ClinicFlow.Controllers;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicFlow.Tests.Controllers
{
    public class ChiefComplaintSpecialtyLookupControllerTests
    {
        private readonly Mock<IChiefComplaintSpecialtyLookupService> chiefComplaintSpecialtyLookupServiceMock;
        private readonly ChiefComplaintSpecialtyLookupController controller;

        public ChiefComplaintSpecialtyLookupControllerTests()
        {
            chiefComplaintSpecialtyLookupServiceMock = new Mock<IChiefComplaintSpecialtyLookupService>();
            controller = new ChiefComplaintSpecialtyLookupController(chiefComplaintSpecialtyLookupServiceMock.Object);
        }

        [Fact]
        public void GetChiefComplaintSpecialtyLookups_WithData_ReturnsOkResult()
        {
            var lookups = new List<ChiefComplaintSpecialtyLookup>
            {
                new ChiefComplaintSpecialtyLookup { Id = 1, ChiefComplaint = "Chest pain", Specialty = "Cardiology" }
            };
            chiefComplaintSpecialtyLookupServiceMock.Setup(s => s.GetChiefComplaintSpecialtyLookups()).Returns(lookups);

            var actionResult = controller.GetChiefComplaintSpecialtyLookups();

            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public void GetChiefComplaintSpecialtyLookups_DelegatesToServiceExactlyOnce()
        {
            chiefComplaintSpecialtyLookupServiceMock.Setup(s => s.GetChiefComplaintSpecialtyLookups())
                .Returns(Array.Empty<ChiefComplaintSpecialtyLookup>());

            controller.GetChiefComplaintSpecialtyLookups();

            chiefComplaintSpecialtyLookupServiceMock.Verify(s => s.GetChiefComplaintSpecialtyLookups(), Times.Once);
        }

        [Fact]
        public void GetChiefComplaintSpecialityLookupByChiefComplaint_WithData_ReturnsOkResult()
        {
            var chiefComplaint = "Headache";
            var lookups = new List<ChiefComplaintSpecialtyLookup>
            {
                new ChiefComplaintSpecialtyLookup { Id = 1, ChiefComplaint = "Headache", Specialty = "Neurology" }
            };
            chiefComplaintSpecialtyLookupServiceMock
                .Setup(s => s.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint))
                .Returns(lookups);

            var actionResult = controller.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint);

            Assert.IsType<OkObjectResult>(actionResult.Result);
            chiefComplaintSpecialtyLookupServiceMock.Verify(
                s => s.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint),
                Times.Once);
        }
    }
}