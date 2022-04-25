using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productService = productService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductWithCategory());
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            var categories = _categoryRepository.GetAll();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");


            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Save(ProductDto productDto)
        {

            if (ModelState.IsValid)
            {
                await _productService.AddAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            var categories = _categoryRepository.GetAll();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

            ViewBag.Categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }
    }
}
