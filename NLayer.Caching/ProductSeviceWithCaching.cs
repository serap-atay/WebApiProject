using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    public class ProductSeviceWithCaching : IProductService
    {
        private const string CacheKey = "ProductsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductSeviceWithCaching(IProductRepository productRepository, IMemoryCache memoryCache, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
            _mapper = mapper;

            if (_memoryCache.TryGetValue(CacheKey, out _))
            {
                _memoryCache.Set(CacheKey, _productRepository.GetProductWithCategory());
            }
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;

        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();

        }

        public  Task<IEnumerable<Product>> GetAllAsync()
        {
           return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheKey).FirstOrDefault(x => x.Id == id);
            if (product == null)
                throw new NotFoundException($"{typeof(Product).Name}({id}) not found");
            return Task.FromResult(product);
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory()
        {
            var products = _memoryCache.Get<List<Product>>(CacheKey);
            var productWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(productWithCategoryDto , 200));
        
        }

        public async Task RemoveAsync(Product entity)
        {
             _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CacheKey).Where(expression.Compile()).AsQueryable();
        }
        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheKey, await _productRepository.GetAll().ToListAsync());
        }
    }
}
