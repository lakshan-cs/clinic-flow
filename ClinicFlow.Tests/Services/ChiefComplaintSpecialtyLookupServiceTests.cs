using ClinicFlow.Models;
using ClinicFlow.Exceptions;
using ClinicFlow.Repositories.Interfaces;
using ClinicFlow.Services;
using Moq;
using Xunit;

namespace ClinicFlow.Tests.Services
{
    public class ChiefComplaintSpecialtyLookupServiceTests
    {
        private readonly Mock<IChiefComplaintSpecialtyLookupRepository> chiefComplaintSpecialtyLookupRepositoryMock;
        private readonly ChiefComplaintSpecialtyLookupService service;

        public ChiefComplaintSpecialtyLookupServiceTests()
        {
            chiefComplaintSpecialtyLookupRepositoryMock = new Mock<IChiefComplaintSpecialtyLookupRepository>();
            service = new ChiefComplaintSpecialtyLookupService(chiefComplaintSpecialtyLookupRepositoryMock.Object);
        }

        [Fact]
        public void GetChiefComplaintSpecialtyLookups_ReturnsRepositoryResults()
        {
            var lookups = new List<ChiefComplaintSpecialtyLookup>
            {
                new ChiefComplaintSpecialtyLookup { Id = 1, ChiefComplaint = "Headache", Specialty = "Neurology" }
            };
            chiefComplaintSpecialtyLookupRepositoryMock.Setup(r => r.GetChiefComplaintSpecialtyLookups()).Returns(lookups);

            var result = service.GetChiefComplaintSpecialtyLookups();

            Assert.Single(result);
            Assert.Equal("Headache", result.First().ChiefComplaint);
        }

        [Fact]
        public void GetChiefComplaintSpecialityLookupByChiefComplaint_WithMatches_ReturnsResults()
        {
            var chiefComplaint = "Headache";
            var lookups = new List<ChiefComplaintSpecialtyLookup>
            {
                new ChiefComplaintSpecialtyLookup { Id = 1, ChiefComplaint = "Headache", Specialty = "Neurology" }
            };
            chiefComplaintSpecialtyLookupRepositoryMock
                .Setup(r => r.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint))
                .Returns(lookups);

            var result = service.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint);

            Assert.Single(result);
            Assert.Equal("Neurology", result.First().Specialty);
        }

        [Fact]
        public void GetChiefComplaintSpecialityLookupByChiefComplaint_WithoutMatches_ThrowsNotFoundException()
        {
            var chiefComplaint = "Unknown";
            chiefComplaintSpecialtyLookupRepositoryMock
                .Setup(r => r.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint))
                .Returns(Array.Empty<ChiefComplaintSpecialtyLookup>());

            var exception = Assert.Throws<NotFoundException>(
                () => service.GetChiefComplaintSpecialityLookupByChiefComplaint(chiefComplaint));

            Assert.Equal(
                "Chief complaint specialty lookup not found for chief complaint: Unknown",
                exception.Message);
        }
    }
}