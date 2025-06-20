using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Xunit;
using Domain.Entities;
using Domain.Entities.Enums;
using Application.Services;
using Application.DTOs;
using Application.Interfaces;
using Infra.Interfaces;

namespace Application.Tests
{
    public class DonationServiceTests
    {
        private readonly Mock<IDonationRepository> _donationRepo = new();
        private readonly Mock<IDonorRepository> _donorRepo = new();
        private readonly Mock<IOrgRepository> _orgRepo = new();
        private readonly DonationService _service;

        public DonationServiceTests()
        {
            _service = new DonationService(_donationRepo.Object, _donorRepo.Object, _orgRepo.Object);
        }

        [Fact]
        public async Task GetAllDonationsAsync_ReturnsDtos()
        {
            var list = new List<Donation> { new Donation { DonationId = 1, DonationAmount = 50 } };
            _donationRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(list);
            var result = await _service.GetAllDonationsAsync();
            Assert.Single(result);
            Assert.Equal(1, result.First().DonationId);
        }

        [Fact]
        public async Task GetDonationByIdAsync_ReturnsNull_WhenNotFound()
        {
            _donationRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Donation)null!);
            var result = await _service.GetDonationByIdAsync(1);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetDonationByIdAsync_ReturnsDto_WhenFound()
        {
            var donation = new Donation { DonationId = 2, DonationAmount = 100 };
            _donationRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(donation);
            var result = await _service.GetDonationByIdAsync(2);
            Assert.NotNull(result);
            Assert.Equal(2, result.DonationId);
            Assert.Equal(100, result.DonationAmount);
        }

        [Fact]
        public async Task GetDonationsByDonorAsync_ReturnsDtos()
        {
            var list = new List<Donation> { new Donation { DonationId = 3, DonorId = 5 } };
            _donationRepo.Setup(r => r.GetByDonorIdAsync(5)).ReturnsAsync(list);
            var result = await _service.GetDonationsByDonorAsync(5);
            Assert.Single(result);
            Assert.Equal(3, result.First().DonationId);
        }

        [Fact]
        public async Task GetDonationsByOrgAsync_ReturnsDtos()
        {
            var list = new List<Donation> { new Donation { DonationId = 4, OrgId = 7 } };
            _donationRepo.Setup(r => r.GetByOrgIdAsync(7)).ReturnsAsync(list);
            var result = await _service.GetDonationsByOrgAsync(7);
            Assert.Single(result);
            Assert.Equal(4, result.First().DonationId);
        }

        [Fact]
        public async Task CreateDonationAsync_Throws_WhenDonorNotFound()
        {
            _donorRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Donor)null!);
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.CreateDonationAsync(new DonationCreateDto { DonorId = 1, OrgId = 1 }));
        }

        [Fact]
        public async Task CreateDonationAsync_Throws_WhenOrgNotFound()
        {
            _donorRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Donor());
            _orgRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Domain.Entities.Org)null!);
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.CreateDonationAsync(new DonationCreateDto { DonorId = 1, OrgId = 2 }));
        }

        [Fact]
        public async Task CreateDonationAsync_CreatesDonation()
        {
            var donor = new Donor { UserId = 1 };
            var org = new Domain.Entities.Org { UserId = 2 };
            _donorRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(donor);
            _orgRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(org);
            Donation created = null!;
            _donationRepo.Setup(r => r.AddAsync(It.IsAny<Donation>()))
                .Callback<Donation>(d => created = d)
                .Returns(Task.CompletedTask);
            _donationRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var dto = new DonationCreateDto
            {
                DonationAmount = 500,
                DonorId = 1,
                OrgId = 2,
                DonationMethod = "Card",
                DonationDate = DateTime.Today,
                Status = DonationStatusEnum.Pendente,
                DonationIsAnonymous = false
            };
            var result = await _service.CreateDonationAsync(dto);
            Assert.Equal(500, result.DonationAmount);
            _donationRepo.Verify(r => r.AddAsync(It.IsAny<Donation>()), Times.Once);
            _donationRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateDonationAsync_UpdatesDonation()
        {
            var donation = new Donation { DonationId = 8, DonationAmount = 100 };
            _donationRepo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(donation);
            _donationRepo.Setup(r => r.UpdateAsync(donation)).Returns(Task.CompletedTask);
            _donationRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var dto = new DonationCreateDto { DonationAmount = 200 };
            var result = await _service.UpdateDonationAsync(8, dto);
            Assert.Equal(200, result.DonationAmount);
            _donationRepo.Verify(r => r.UpdateAsync(donation), Times.Once);
        }

        [Fact]
        public async Task DeleteDonationAsync_ReturnsTrue_WhenFound()
        {
            var donation = new Donation { DonationId = 3 };
            _donationRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(donation);
            _donationRepo.Setup(r => r.DeleteAsync(3)).Returns(Task.CompletedTask);
            _donationRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var result = await _service.DeleteDonationAsync(3);
            Assert.True(result);
            _donationRepo.Verify(r => r.DeleteAsync(3), Times.Once);
        }
    }

    public class DonorServiceTests
    {
        private readonly Mock<IDonorRepository> _donorRepo = new();
        private readonly Mock<ILocalFileStorageService> _storage = new();
        private readonly DonorService _service;

        public DonorServiceTests() => _service = new DonorService(_donorRepo.Object, _storage.Object);

        [Fact]
        public async Task GetAllDonorsAsync_ReturnsProfiles()
        {
            var donors = new List<Donor> { new Donor { UserId = 5, Name = "Test", Document = "D", Phone = "P", BirthDate = DateTime.Today, ImageUrl = "img" } };
            _donorRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(donors);
            _storage.Setup(s => s.GetFileUrl("img")).Returns("url");
            var result = await _service.GetAllDonorsAsync();
            Assert.Single(result);
            Assert.Equal(5, result.First().UserId);
            Assert.Equal("url", result.First().ImageUrl);
        }

        [Fact]
        public async Task GetDonorByUserIdAsync_ReturnsProfile()
        {
            var donor = new Donor { UserId = 10, Name = "X", Document = "D", Phone = "P", BirthDate = DateTime.Today, ImageUrl = "img" };
            _donorRepo.Setup(r => r.GetByUserIdAsync(10)).ReturnsAsync(donor);
            _storage.Setup(s => s.GetFileUrl("img")).Returns("path");
            var result = await _service.GetDonorByUserIdAsync(10);
            Assert.Equal(10, result.UserId);
            Assert.Equal("path", result.ImageUrl);
        }
    }

    public class LocalFileStorageServiceTests : IDisposable
    {
        private readonly string _basePath;
        private readonly LocalFileStorageService _service;
        private readonly Mock<IHttpContextAccessor> _accessor = new();

        public LocalFileStorageServiceTests()
        {
            _basePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var env = Mock.Of<IWebHostEnvironment>(e => e.ContentRootPath == _basePath);
            var cfg = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["FileStorageSettings:LocalPath"] = "Store",
                    ["FileStorageSettings:PublicBaseUrlPath"] = "/img"
                })
                .Build();
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = new HostString("host");
            _accessor.Setup(a => a.HttpContext).Returns(context);
            _service = new LocalFileStorageService(cfg, env, _accessor.Object);
        }

        [Fact]
        public async Task SaveFileAsync_SavesFile()
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes("x"));
            IFormFile f = new FormFile(ms, 0, ms.Length, "d", "f.txt");
            var path = await _service.SaveFileAsync(f, "sub");
            Assert.True(File.Exists(Path.Combine(_basePath, "Store", path.Replace('/', Path.DirectorySeparatorChar))));
        }

        public void Dispose() { if (Directory.Exists(_basePath)) Directory.Delete(_basePath, true); }
    }

    public class OrgServiceTests
    {
        private readonly Mock<IOrgRepository> _repo = new();
        private readonly Mock<ILocalFileStorageService> _storage = new();
        private readonly OrgService _service;

        public OrgServiceTests() => _service = new OrgService(_repo.Object, _storage.Object);

        [Fact]
        public async Task GetAllOrgsAsync_ReturnsDtos()
        {
            var orgs = new List<Domain.Entities.Org> { new Domain.Entities.Org { UserId = 1, OrgName = "Name", Phone = "123", Document = "Doc", Address = "Addr", Description = "Desc", AdminName = "Admin", ImageUrl = "img", OrgWebsiteUrl = "web", OrgFoundationDate = DateTime.Today, AdminPhone = "321" } };
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(orgs);
            _storage.Setup(s => s.GetFileUrl("img")).Returns("url");
            var result = (await _service.GetAllOrgsAsync()).ToList();
            Assert.Single(result);
            Assert.Equal(1, result.First().UserId);
            Assert.Equal("url", result.First().ImageUrl);
        }

        [Fact]
        public async Task GetOrgByUserIdAsync_ReturnsDto_WhenFound()
        {
            var org = new Domain.Entities.Org { UserId = 2, ImageUrl = "path" };
            _repo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(org);
            _storage.Setup(s => s.GetFileUrl("path")).Returns("url");
            var result = await _service.GetOrgByUserIdAsync(2);
            Assert.NotNull(result);
            Assert.Equal(2, result.UserId);
            Assert.Equal("url", result.ImageUrl);
        }
    }

    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _repo = new();
        private readonly Mock<IOrgRepository> _orgRepo = new();
        private readonly ProjectService _service;

        public ProjectServiceTests() => _service = new ProjectService(_repo.Object, _orgRepo.Object);

        [Fact]
        public async Task GetAllProjectsAsync_ReturnsDtos()
        {
            var projects = new List<Project> { new Project { Id = 1, Name = "N", Address = "A", Description = "D", Image_Url = "img", OrgId = 1, Org = new Domain.Entities.Org { OrgName = "OrgName" } } };
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(projects);
            var result = (await _service.GetAllProjectsAsync()).ToList();
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task CreateProjectAsync_CreatesProject()
        {
            var dto = new ProjectCreateDto { OrgId = 1, Name = "n", Address = "a", Description = "d", Image_Url = "img" };
            _orgRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Domain.Entities.Org { UserId = 1 });
            Project created = null!;
            _repo.Setup(r => r.AddAsync(It.IsAny<Project>())).Callback<Project>(p => created = p).Returns(Task.CompletedTask);
            _repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(() => { created.Id = 5; return created; });
            var result = await _service.CreateProjectAsync(dto);
            Assert.Equal(5, result.Id);
        }
    }

    public class ReportServiceTests
    {
        private readonly Mock<IReportRepository> _repo = new();
        private readonly ReportService _service;

        public ReportServiceTests() => _service = new ReportService(_repo.Object);

        [Fact]
        public async Task GetAllReportsAsync_ReturnsDtos()
        {
            var reports = new List<Report> { new Report { ReportId = 1, ReportDate = DateTime.Today, ReportContent = "c" } };
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(reports);
            var result = (await _service.GetAllReportsAsync()).ToList();
            Assert.Single(result);
            Assert.Equal(1, result.First().ReportId);
        }

        [Fact]
        public async Task CreateReportAsync_CreatesReport()
        {
            Report created = null!;
            _repo.Setup(r => r.AddAsync(It.IsAny<Report>())).Callback<Report>(r => created = r).Returns(Task.CompletedTask);
            _repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var dto = new ReportCreateDto { ReportDate = DateTime.Today, ReportContent = "c" };
            var result = await _service.CreateReportAsync(dto);
            Assert.Equal("c", result.ReportContent);
        }
    }

    public class TestesUnitarios
    {
        private readonly Mock<IUserRepository> _ur = new();
        private readonly Mock<IDonorRepository> _dr = new();
        private readonly Mock<IOrgRepository> _or = new();
        private readonly Mock<ILocalFileStorageService> _st = new();
        private readonly IConfiguration _cfg;
        private readonly UserService _svc;

        public TestesUnitarios()
        {
            var settings = new Dictionary<string, string?>
            {
                ["JwtSettings:SecretKey"] = "secret",
                ["JwtSettings:Issuer"] = "iss",
                ["JwtSettings:Audience"] = "aud",
                ["JwtSettings:ExpiresInHours"] = "1"
            };
            _cfg = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            _svc = new UserService(_ur.Object, _dr.Object, _or.Object, _st.Object, _cfg);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsUsers()
        {
            var users = new List<User> { new User { Id = 1, UserEmail = "a@a.com", Role = UserRole.Admin, UserDateCreated = DateTime.UtcNow } };
            _ur.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            var result = await _svc.GetAllUsersAsync();
            Assert.Single(result);
            Assert.Equal("a@a.com", result.First().UserEmail);
        }

        [Fact]
        public async Task CreateAdminUserAsync_Throws_OnWrongRole()
        {
            var dto = new UserCreateDto { UserEmail = "b@b.com", UserPassword = "pass", Role = UserRole.Donor };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAdminUserAsync(dto));
        }

        [Fact]
        public async Task ValidateUserAsync_ReturnsFalse_WhenInvalid()
        {
            _ur.Setup(r => r.GetByEmailAsync("x@x.com")).ReturnsAsync((User)null!);
            var (success, token, user) = await _svc.ValidateUserAsync("x@x.com", "pass");
            Assert.False(success);
            Assert.Equal(string.Empty, token);
            Assert.Null(user);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsFalse_WhenNotFound()
        {
            _ur.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null!);
            var result = await _svc.DeleteUserAsync(1);
            Assert.False(result);
        }
    }
}
