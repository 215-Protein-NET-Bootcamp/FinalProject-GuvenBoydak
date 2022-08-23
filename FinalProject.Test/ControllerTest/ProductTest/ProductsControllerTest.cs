using AutoMapper;
using FinalProject.Api;
using FinalProject.Base;
using FinalProject.Business;
using FinalProject.DTO;
using FinalProject.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Test.ControllerTest.ProductTest
{
    public class ProductsControllerTest
    {
        private readonly Mock<IProductService> _productService;
        private readonly IMapper _mapper;
        private readonly ProductsController _productsController;

        public ProductsControllerTest()
        {
            MapperConfiguration map = new MapperConfiguration(configure =>
            {
                configure.AddProfile(new MapProfile());
            });
            _productService = new Mock<IProductService>();
            _mapper = map.CreateMapper();
            _productsController = new ProductsController(_productService.Object, _mapper,null,null);
        }

        List<Product> products = new List<Product>()
        {
            new Product(){ID=1,Name="laptop",Description="mac pro",UnitPrice=25000,ImageUrl="59cae2de-8e12-23d2-bed5-9d1985ec8c94.png",AppUserID=1,CategoryID=1,BrandID=1,ColorID=1,CreatedDate=DateTime.Now,Status=DataStatus.Inserted},
            new Product(){ID=2,Name="ayakkabı",Description="erkek ayakabısı",UnitPrice=1000,ImageUrl="33cae2de-1e82-42d2-bed5-9d1985ec8c94.png",AppUserID=2,CategoryID=4,BrandID=2,ColorID=1,CreatedDate=DateTime.Now,Status=DataStatus.Inserted},
            new Product(){ID=3,Name="tornavida",Description="yıldız tornavida",UnitPrice=500,ImageUrl="25cae1de-8e82-43d2-bed5-9d1985ec8c94.png",AppUserID=3,CategoryID=2,BrandID=3,ColorID=2,CreatedDate=DateTime.Now,Status=DataStatus.Inserted}
        };


        [Fact]
        public async void GetAll_ActionExecutes_Return200WithProductListDto()
        {
            _productService.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            IActionResult result =await _productsController.GetAllAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<ProductListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<ProductListDto>>>(objectResult.Value);

            Assert.Equal<int>(3,responseDto.Data.Count);
        }


        [Fact]
        public async void GetActive_ActionExecutes_Return200WithProductListDto()
        {
            _productService.Setup(x => x.GetActiveAsync()).ReturnsAsync(products);

            IActionResult result = await _productsController.GetActiveAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<ProductListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<ProductListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Fact]
        public async void GetPassive_ActionExecutes_Return200WithProductListDto()
        {
            _productService.Setup(x => x.GetPassiveAsync()).ReturnsAsync(products);

            IActionResult result = await _productsController.GetPassiveAsync();

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            CustomResponseDto<List<ProductListDto>> responseDto = Assert.IsAssignableFrom<CustomResponseDto<List<ProductListDto>>>(objectResult.Value);

            Assert.Equal<int>(3, responseDto.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetByID_ActionExecutes_Return200WithProductDto(int id)
        {
            Product product = products.FirstOrDefault(x=>x.ID==id);
            _productService.Setup(x => x.GetByIDAsync(It.IsAny<int>())).ReturnsAsync(product);

            IActionResult result =await _productsController.GetByIdAsync(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(200, objectResult.StatusCode.Value);

            Assert.IsAssignableFrom<CustomResponseDto<ProductDto>>(objectResult.Value);
        }

        [Theory]
        [InlineData(1)]
        public void Delete_ActionExecutes_Return2004NoContenDto(int id)
        {
            _productService.Setup(x => x.Delete(It.IsAny<int>()));
            
            IActionResult result = _productsController.Delete(id);

            ObjectResult objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal<int>(204, objectResult.StatusCode.Value);
        }
    }
}
