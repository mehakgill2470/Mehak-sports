using MehakSports.Controllers;
using MehakSports.Data;
using MehakSports.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MehakSportsTest
{
    [TestClass]
    public class ProductsControllerTest
    {
        ProductsController productsController;

        List<Product> products;

        private ApplicationDbContext _context;

        [TestInitialize]

        public void TestInitialize()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            _context = new ApplicationDbContext(options);
            products = new List<Product>();

            Brand mockBrand = new Brand
            {
                BrandID = 13,
                BrandName = "A Fake Brand"
            };

            products.Add(new Product
            {
                ProductID = 201,
                ProductName = "My Table Tennis",
                MSRP = 1000,
                Brand = mockBrand
            });

            products.Add(new Product
            {
                ProductID = 204,
                ProductName = "My Pool",
                MSRP = 500,
                Brand = mockBrand
            });

            products.Add(new Product
            {
                ProductID = 206,
                ProductName = "Rackett",
                MSRP = 600,
                Brand = mockBrand
            });

            foreach (var p in products)
            {
                // add each product to in-memory db
                _context.Product.Add(p);
            }
            _context.SaveChanges();

            // 3. this is the last step
            productsController = new ProductsController(_context);
        }

        [TestMethod]
        public void IndexLoadsCorrectView()
        {
            // act
            var result = productsController.Index().Result;
            var viewResult = (ViewResult)result;

            // ASSERT
            Assert.AreEqual("Index", viewResult.ViewName);
        }


        [TestMethod]
        public void IndexReturnsProducts()
        {
            // act
            var result = productsController.Index().Result;

            // get the view result
            var viewResult = (ViewResult)result;

            // assert - convert result to list of products & compare to mock product list
            CollectionAssert.AreEqual(products.OrderBy(p => p.ProductID).ToList(), (List<Product>)viewResult.Model);
        }

        [TestMethod]
        public void DetailsMissingId()
        {
            // act
            var result = productsController.Details(null).Result;

            // assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DetailsInvalidId()
        {
            // act
            var result = productsController.Details(1203).Result;

            // assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DetailsValidIdLoadsProduct()
        {
            // act
            var result = productsController.Details(101).Result;
            var viewResult = (ViewResult)result;

            // assert
            Assert.AreEqual(products[1], viewResult.Model);
        }

        [TestMethod]
        public void CreatePostInvalidData()
        {
            var product = new Product
            {
                ProductID = 78,
                MSRP = 34,
                Brand = new Brand { BrandID = 34, BrandName = "New Brand" }
            };

            productsController.ModelState.AddModelError("Error", "Fake model error");

            // act
            var result = productsController.Create(product);
            var viewResult = (ViewResult)result.Result;

            // assert
            Assert.AreEqual("Create", viewResult.ViewName);
        }

        [TestMethod]
        public void CreatePostInvalidDataPopulatesCategories()
        {
            // arrange -> create product object
            var product = new Product
            {
                ProductID = 78,
                ProductName = "Baseball",
                MSRP = 34,
                Brand = new Brand { BrandID = 13, BrandName = "A Fake Brand" }
            };

            // manually create error in model
            productsController.ModelState.AddModelError("Error", "Fake model error");

            // act
            var result = productsController.Create(product);
            var viewResult = (ViewResult)result.Result;

            // assert
            Assert.IsNotNull(viewResult.ViewData["PharmaID"]);
        }

        [TestMethod]
        public void CreatePostAddsProduct()
        {
            // arrange -> create product object
            var product = new Product
            {

                ProductID = 78,
                ProductName = "Baseball",
                MSRP = 34,
                Brand = new Brand { BrandID = 13, BrandName = "A Fake Brand" }
            };

            // act
            var result = productsController.Create(product);

            // assert
            Assert.AreEqual(_context.Product.LastOrDefault(), product);
        }

        [TestMethod]
        public void CreatePostRedirectsToIndex()
        {
            // arrange -> create product object
            var product = new Product
            {
                ProductID = 78,
                ProductName = "Baseball",
                MSRP = 34,
                Brand = new Brand { BrandID = 13, BrandName = "A Fake Brand" }
            };

            // act
            var result = productsController.Create(product);
            var redirectResult = (RedirectToActionResult)result.Result;

            // assert
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
    }





           }

