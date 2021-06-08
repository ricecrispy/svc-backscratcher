using System;
using Xunit;
using Moq;
using svc_backscratcher.Models;
using svc_backscratcher.DataAccessLayers;
using AutoMapper;
using svc_backscratcher.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace svc_backscratcher_tests
{
    public class BackScratchersControllerTest
    {
        [Fact]
        public async Task CreateBackScratcherAsync_ValidInputs_CreatesObjectSuccessfully()
        {
            //Arrange
            BackScratcherRest body = new BackScratcherRest
            {
                Name = "bs1",
                Description = "bs1-is-great!",
                Price = "$781.30",
                Sizes = new string[] { "S", "L" }
            };

            BackScratcherDal dalBody = new BackScratcherDal
            {
                Name = body.Name,
                Description = body.Description,
                Price = 781.30,
                Size = new BackScratcherSize[] { BackScratcherSize.S, BackScratcherSize.L }
            };

            Guid expected = Guid.NewGuid();

            var backScratcherRepository = new Mock<IBackScratcherRepository>();
            backScratcherRepository
                .Setup(x => x.CreateBackScratcherAsync(dalBody))
                .ReturnsAsync(expected);
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<BackScratcherDal>(body))
                .Returns(dalBody);

            BackscratchersController underTest = new BackscratchersController(backScratcherRepository.Object, mapper.Object);
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.CreateBackScratcherAsync(body);

            //assert
            Assert.IsType<CreatedResult>(response);
            var result = (CreatedResult)response;
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async Task CreateBackScratcherAsync_BodyAlreadyExists_ReturnsConflict()
        {
            //Arrange
            BackScratcherRest body = new BackScratcherRest
            {
                Name = "bs1",
                Description = "bs1-is-great!",
                Price = "$781.30",
                Sizes = new string[] { "S", "L" }
            };

            BackScratcherDal dalBody = new BackScratcherDal
            {
                Name = body.Name,
                Description = body.Description,
                Price = 781.30,
                Size = new BackScratcherSize[] { BackScratcherSize.S, BackScratcherSize.L }
            };
            var dalBodyList = new List<BackScratcherDal> { dalBody };

            Guid expected = Guid.NewGuid();

            var backScratcherRepository = new Mock<IBackScratcherRepository>();
            backScratcherRepository
                .Setup(x => x.SearchAllBackScraterchersAsync())
                .ReturnsAsync(dalBodyList);
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<BackScratcherDal>(body))
                .Returns(dalBody);

            BackscratchersController underTest = new BackscratchersController(backScratcherRepository.Object, mapper.Object);
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.CreateBackScratcherAsync(body);

            //assert
            Assert.IsType<ConflictResult>(response);
        }

        [Theory]
        [InlineData("", "description", "$5.00", true)]
        [InlineData("name", "", "$5.00", true)]
        [InlineData("name", "description", "five dollars", true)]
        [InlineData("name", "description", "$5.00", false)]
        public async Task CreateBackScratcherAsync_InvalidInputs_ReturnsBadRequest(string name, string description, string price, bool shouldCreateValidSizes)
        {
            //Arrange
            List<string> sizes = new List<string>();
            if (shouldCreateValidSizes)
            {
                sizes.Add("L");
                sizes.Add("S");
                sizes.Add("m");
                sizes.Add("   xL    ");
            }
            else
            {
                sizes.Add("very small");
            }

            BackScratcherRest body = new BackScratcherRest
            {
                Name = name,
                Description = description,
                Price = price,
                Sizes = sizes
            };

            BackscratchersController underTest = new BackscratchersController(Mock.Of<IBackScratcherRepository>(), Mock.Of<IMapper>());

            //Act
            var response = await underTest.CreateBackScratcherAsync(body);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task SearchBackScratchersAsync_RetrievesAllObjectsSuccesfully()
        {
            //Arrange
            BackScratcherDal backScratcherDal = new BackScratcherDal
            {
                Name = "some-name",
                Description = "some-description",
                Identifier = Guid.NewGuid(),
                Price = 77.77,
                Size = new List<BackScratcherSize> { BackScratcherSize.S }
            };
            List<BackScratcherDal> backScratcherDals = new List<BackScratcherDal> { backScratcherDal };
            BackScratcherRest backScratcherRest = new BackScratcherRest
            {
                Name = backScratcherDal.Name,
                Description = backScratcherDal.Description,
                Identifier = backScratcherDal.Identifier,
                Price = $"${backScratcherDal.Price:2F}",
                Sizes = new List<string> { "S" }
            };
            List<BackScratcherRest> expected = new List<BackScratcherRest> { backScratcherRest };
            var backScratcherRepository = new Mock<IBackScratcherRepository>();
            backScratcherRepository
                .Setup(x => x.SearchAllBackScraterchersAsync())
                .ReturnsAsync(backScratcherDals);
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<List<BackScratcherRest>>(backScratcherDals))
                .Returns(expected);

            BackscratchersController underTest = new BackscratchersController(backScratcherRepository.Object, mapper.Object);
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.SearchBackScratchersAsync();

            //Assert
            Assert.Same(expected, response.Value);
        }

        [Theory]
        [InlineData("L", "$$$")]
        [InlineData("L", "two dollars and fifty cents")]
        [InlineData("LLL", "$2.50")]
        [InlineData("small,large", "2.50")]
        public async Task SearchBackScratchersAsync_InvalidInputs_ReturnsBadRequest(string sizes, string price)
        {
            //Arrange
            BackscratchersController underTest = new BackscratchersController(Mock.Of<IBackScratcherRepository>(), Mock.Of<IMapper>());

            //Act
            var response = await underTest.SearchBackScratchersAsync(sizes: sizes, price: price);

            //Assert
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task GetBackScratcherAsync_ValidInput_RetrievesObjectSuccessfully()
        {
            //Arrange
            BackScratcherDal backScratcherDal = new BackScratcherDal
            {
                Name = "some-name",
                Description = "some-description",
                Identifier = Guid.NewGuid(),
                Price = 100.01,
                Size = new List<BackScratcherSize> { BackScratcherSize.XL }
            };

            BackScratcherRest expected = new BackScratcherRest
            {
                Name = backScratcherDal.Name,
                Description = backScratcherDal.Description,
                Identifier = backScratcherDal.Identifier,
                Price = $"${backScratcherDal.Price:2F}",
                Sizes = new List<string> { "XL" }
            };

            var backScratcherRepository = new Mock<IBackScratcherRepository>();
            backScratcherRepository
                .Setup(x => x.GetBackScratcherAsync(backScratcherDal.Identifier))
                .ReturnsAsync(backScratcherDal);
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<BackScratcherRest>(backScratcherDal))
                .Returns(expected);

            BackscratchersController underTest = new BackscratchersController(backScratcherRepository.Object, mapper.Object);
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.GetBackScratcherAsync(backScratcherDal.Identifier);

            //Assert
            Assert.Same(expected, response.Value);
        }

        [Fact]
        public async Task GetBackScratcherAsync_InvalidInput_ReturnsBadRequest()
        {
            //Arrange
            BackscratchersController underTest = new BackscratchersController(Mock.Of<IBackScratcherRepository>(), Mock.Of<IMapper>());
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.GetBackScratcherAsync(default);

            //Assert
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Fact]
        public async Task GetBackScratcherAsync_ItemDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            BackscratchersController underTest = new BackscratchersController(Mock.Of<IBackScratcherRepository>(), Mock.Of<IMapper>());
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.GetBackScratcherAsync(Guid.NewGuid());

            //Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async Task UpdateBackScratcherAsync_ValidInputs_UpdatesObjectSuccessfully()
        {
            //Arrange
            BackScratcherDal backScratcherDal = new BackScratcherDal
            {
                Name = "some-name",
                Description = "some-description",
                Identifier = Guid.NewGuid(),
                Price = 100.01,
                Size = new List<BackScratcherSize> { BackScratcherSize.XL }
            };

            BackScratcherRest body = new BackScratcherRest
            {
                Name = backScratcherDal.Name,
                Description = backScratcherDal.Description,
                Identifier = backScratcherDal.Identifier,
                Price = $"${backScratcherDal.Price}",
                Sizes = new List<string> { "XL" }
            };

            var backScratcherRepository = new Mock<IBackScratcherRepository>();
            backScratcherRepository
                .Setup(x => x.GetBackScratcherAsync(body.Identifier))
                .ReturnsAsync(backScratcherDal);
            backScratcherRepository
                .Setup(x => x.UpdateBackScratcherAsync(backScratcherDal))
                .Verifiable();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(x => x.Map<BackScratcherDal>(body))
                .Returns(backScratcherDal);

            BackscratchersController underTest = new BackscratchersController(backScratcherRepository.Object, mapper.Object);
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.UpdateBackScratcherAsync(body.Identifier, body);

            //Assert
            Assert.IsType<OkResult>(response);
            backScratcherRepository.Verify();
        }

        [Fact]
        public async Task UpdateBackScratcherAsync_ProductNotFound_ReturnsNotFound()
        {
            //Arrange
            BackScratcherRest body = new BackScratcherRest
            {
                Name = "some-name",
                Description = "some-description",
                Identifier = Guid.NewGuid(),
                Price = "$123.45",
                Sizes = new List<string> { "XL" }
            };

            var backScratcherRepository = new Mock<IBackScratcherRepository>();

            var mapper = new Mock<IMapper>();

            BackscratchersController underTest = new BackscratchersController(backScratcherRepository.Object, mapper.Object);
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.UpdateBackScratcherAsync(body.Identifier, body);

            //Assert
            Assert.IsType<NotFoundResult>(response);
        }


        [Fact]
        public async Task DeleteBackScratcherAsync_ValidInputs_DeletesObjectSuccessfully()
        {
            //Arrange
            Guid backScratcherId = Guid.NewGuid();

            var backScratcherRepository = new Mock<IBackScratcherRepository>();
            backScratcherRepository
                .Setup(x => x.GetBackScratcherAsync(backScratcherId))
                .ReturnsAsync(Mock.Of<BackScratcherDal>());
            backScratcherRepository
                .Setup(x => x.DeleteBackScratcherAsync(backScratcherId))
                .Verifiable();
            BackscratchersController underTest = new BackscratchersController(backScratcherRepository.Object, Mock.Of<IMapper>());
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.DeleteBackScratcherAsync(backScratcherId);

            //Assert
            Assert.IsType<OkResult>(response);
            backScratcherRepository.Verify();
        }

        [Fact]
        public async Task DeleteBackScratcherAsync_InvalidInputs_ReturnsBadRequest()
        {
            //Arrange
            BackscratchersController underTest = new BackscratchersController(Mock.Of<IBackScratcherRepository>(), Mock.Of<IMapper>());
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.DeleteBackScratcherAsync(default);

            //Assert
            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async Task DeleteBackScratcherAsync_ItemDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            BackscratchersController underTest = new BackscratchersController(Mock.Of<IBackScratcherRepository>(), Mock.Of<IMapper>());
            underTest.ControllerContext = GetMockContext();

            //Act
            var response = await underTest.DeleteBackScratcherAsync(Guid.NewGuid());

            //Assert
            Assert.IsType<NotFoundResult>(response);
        }

        private ControllerContext GetMockContext()
        {
            var controllerContext = new ControllerContext();
            var httpContext = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            controllerContext.HttpContext = httpContext.Object;
            httpContext.SetupGet(s => s.Request).Returns(request.Object);
            request.SetupGet(s => s.Path).Returns(new PathString("/backscratchers"));
            return controllerContext;
        }
    }
}
