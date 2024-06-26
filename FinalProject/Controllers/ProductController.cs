﻿using FinalProject.Helper;
using FinalProject.Interfaces;
using FinalProject.ServiceModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Text.Encodings.Web;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IProductManager _productManager;
        public ProductController(IProductManager productManager,
                                ICompositeViewEngine viewEngine)
        {
            _productManager = productManager;
            _viewEngine = viewEngine;
        }
        public IActionResult List([FromQuery] ProductListQueryModel request)
        {
            if (request == null) request = new ProductListQueryModel();
            return View(request);
        }

        [HttpGet]
        public async Task<JsonResult> Filter([FromQuery] ProductListQueryModel request)
        {
            try
            {
                var vm = await _productManager.GetFilteredProducts(request);

                var html = await RenderPartialView.InvokeAsync("_ProductListPartial",
                ControllerContext,
                                                                _viewEngine,
                                                                ViewData,
                                                                TempData,
                                                                vm);
                return Json(new
                {
                    data = html,
                    status = 200,
                    productCount = vm.ProductCount,
                    totalPage = vm.TotalPage,
                    currentPage = request.Page
                });
            }
            catch (Exception exp)
            {
                return Json(new
                {
                    error = "xeta bas verdi",
                    status = 500
                });
            }

        }
        private string RenderViewToString(IHtmlContent viewContent)
        {
            using (var writer = new StringWriter())
            {
                viewContent.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

    }
}
