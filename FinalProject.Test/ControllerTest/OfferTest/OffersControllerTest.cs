using AutoMapper;
using FinalProject.Api;
using FinalProject.Base;
using FinalProject.Business;
using FinalProject.DTO;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinalProject.Test
{
    public class OffersControllerTest
    {
        private readonly Mock<IOfferService> _offerService;
        private readonly Mock<IProductService> _productService;
        private readonly IMapper _mapper;
        private readonly OffersController _offersController;
        private readonly ProductsController _productsController;

        public OffersControllerTest()
        {
            MapperConfiguration map = new MapperConfiguration(configure =>
            {
                configure.AddProfile(new MapProfile());
            });
            _offerService = new Mock<IOfferService>();
            _productService = new Mock<IProductService>();
            _mapper = map.CreateMapper();
            _offersController = new OffersController(_offerService.Object, _mapper, _productService.Object);
            _productsController = new ProductsController(_productService.Object,_mapper,null,null); 
        }

        List<Offer> offers = new List<Offer>()
        {
            new Offer(){ID=1,Price=110,IsApproved=false,AppUserID=1,ProductID=1,CreatedDate=DateTime.UtcNow,Status=DataStatus.Inserted },
            new Offer(){ID=2,Price=120,IsApproved=false,AppUserID=2,ProductID=2,CreatedDate=DateTime.UtcNow,Status=DataStatus.Inserted },
            new Offer(){ID=3,Price=130,IsApproved=false,AppUserID=3,ProductID=3,CreatedDate=DateTime.UtcNow,Status=DataStatus.Inserted },
        };

        List<Product> products = new List<Product>()
        {
            new Product(){ID=1,Name="laptop",Description="mac pro",UnitPrice=25000,ImageUrl="59cae2de-8e12-23d2-bed5-9d1985ec8c94.png",AppUserID=1,CategoryID=1,BrandID=1,ColorID=1,CreatedDate=DateTime.Now,Status=DataStatus.Inserted},
            new Product(){ID=2,Name="ayakkabı",Description="erkek ayakabısı",UnitPrice=1000,ImageUrl="33cae2de-1e82-42d2-bed5-9d1985ec8c94.png",AppUserID=2,CategoryID=4,BrandID=2,ColorID=1,CreatedDate=DateTime.Now,Status=DataStatus.Inserted},
            new Product(){ID=3,Name="tornavida",Description="yıldız tornavida",UnitPrice=500,ImageUrl="25cae1de-8e82-43d2-bed5-9d1985ec8c94.png",AppUserID=3,CategoryID=2,BrandID=3,ColorID=2,CreatedDate=DateTime.Now,Status=DataStatus.Inserted}
        };


        [Fact]
        public async void GetAll_ActionExecutes_Return200WithOfferListDto()
        {
            _offerService.Setup(x => x.GetAllAsync()).ReturnsAsync(offers);

            IActionResult result =await _offersController.GetAllAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<OfferListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<OfferListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Fact]
        public async void GetActive_ActionExecutes_Return200WithOfferListDto()
        {
            _offerService.Setup(x => x.GetActiveAsync()).ReturnsAsync(offers);

            IActionResult result = await _offersController.GetActiveAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<OfferListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<OfferListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Fact]
        public async void GetPassive_ActionExecutes_Return200WithOfferListDto()
        {
            _offerService.Setup(x => x.GetPassiveAsync()).ReturnsAsync(offers);

            IActionResult result = await _offersController.GetPassiveAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<OfferListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<OfferListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetByID_ActionExecutes_Return200WithOfferDto(int id)
        {
            Offer offer = offers.FirstOrDefault(x => x.ID == id);

            _offerService.Setup(x => x.GetByIDAsync(It.IsAny<int>())).ReturnsAsync(offer);

            IActionResult result = await _offersController.GetByIdAsync(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            Assert.IsAssignableFrom<CustomResponseDto<OfferDto>>(objectResult.Value);
        }


        [Theory]
        [InlineData(1)]
        public async void GetByAppUserProductsOffers_ActionExecutes_Return200WithAppUserProductsOfferListDto(int id)
        {
            _offerService.Setup(x => x.GetByAppUserProductsOffersAsync(It.IsAny<int>())).ReturnsAsync(products);

            IActionResult result = await _offersController.GetByAppUserProductsOffersAsync(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            Assert.IsAssignableFrom<CustomResponseDto<List<AppUserProductsOfferListDto>>>(objectResult.Value);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetByAppUserOffers_ActionExecutes_Return200WithOfferListDto(int id)
        {
            _offerService.Setup(x => x.GetByAppUserOffersAsync(It.IsAny<int>())).ReturnsAsync(offers);

            IActionResult result = await _offersController.GetByAppUserOffersAsync(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

             Assert.IsAssignableFrom<CustomResponseDto<List<OfferListDto>>>(objectResult.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetByOffersProductID_ActionExecutes_Return200WithProductOffersListDto(int id)
        {
            _offerService.Setup(x => x.GetByOffersProductIDAsync(It.IsAny<int>())).ReturnsAsync(offers);

            IActionResult result = await _offersController.GetByOffersProductIDAsync(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            Assert.IsAssignableFrom<CustomResponseDto<List<ProductOffersListDto>>>(objectResult.Value);
        }


        [Fact]
        public async void BuyProduct_ActionExecutes_Return204NoContentDto()
        {
            Offer offer = new Offer() { ID = 4, Price = 140, IsApproved = false, AppUserID = 1, ProductID = 3, CreatedDate = DateTime.UtcNow, Status = DataStatus.Inserted };
            _offerService.Setup(x => x.BuyProductAsync(offer));

            OfferBuyProductDto offerBuyProductDto = _mapper.Map<Offer, OfferBuyProductDto>(offer);

            IActionResult result = await _offersController.BuyProductAsync(offerBuyProductDto);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204, objectResult.StatusCode.Value);
        }

        [Fact]
        public async void OfferApproval_ActionExecutes_Return204NoContentDto()
        {
            Offer offer = new Offer() { ID = 5, Price = 150, IsApproved = false, AppUserID = 2, ProductID = 1, CreatedDate = DateTime.UtcNow, Status = DataStatus.Inserted };
            _offerService.Setup(x => x.OfferApprovalAsync(offer));

            OfferApprovalDto offerApprovalDto = _mapper.Map<Offer, OfferApprovalDto>(offer);

            IActionResult result = await _offersController.OfferApprovalAsync(offerApprovalDto);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204, objectResult.StatusCode.Value);
        }

        [Fact]
        public async void Update_ActionExecutes_Return204NoContentDto()
        {
            Offer offer = new Offer() { ID = 1, Price = 160, IsApproved = false, AppUserID = 2, ProductID = 2, CreatedDate = DateTime.UtcNow, Status = DataStatus.Inserted };
            _offerService.Setup(x => x.UpdateAsync(offer));

            OfferUpdateDto offerUpdateDto = _mapper.Map<Offer, OfferUpdateDto>(offer);

            IActionResult result = await _offersController.UpdateAsync(offerUpdateDto);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204, objectResult.StatusCode.Value);
        }

        [Theory]
        [InlineData(1)]
        public async void Delete_ActionExecutes_Return204NoContentDto(int id)
        {
            _offerService.Setup(x => x.DeleteAsync(It.IsAny<int>()));

            IActionResult result = await _offersController.DeleteAsync(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204, objectResult.StatusCode.Value);
        }

    }
}
