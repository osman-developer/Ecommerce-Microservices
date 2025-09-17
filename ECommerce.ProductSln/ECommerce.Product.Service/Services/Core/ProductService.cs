using AutoMapper;
using ECommerce.Common.Interface.Repository;
using ECommerce.Common.Response;
using ECommerce.Product.Domain.DTOs.Core.Product;
using ECommerce.Product.Domain.Entities;
using ECommerce.Product.Domain.Interfaces.Services;

namespace ECommerce.Product.Service.Services.Core
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<ProductItem> _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<ProductItem> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<Response<GetProductDTO>> Get(int id)
        {
            var result = await _productRepo.GetByIdAsync(id);

            if (!result.Success || result.Data == null)
                return Response<GetProductDTO>.Fail(result.Message);

            var dto = _mapper.Map<GetProductDTO>(result.Data);
            return Response<GetProductDTO>.Ok(dto);
        }

        public async Task<Response<List<GetProductDTO>>> GetAll()
        {
            var result = await _productRepo.GetAllAsync();

            var dtos = _mapper.Map<List<GetProductDTO>>(result.Data.ToList());
            return Response<List<GetProductDTO>>.Ok(dtos, result.Message);
        }

        public async Task<Response<GetProductDTO>> Save(AddOrUpdateProductDto productDto)
        {
            var entity = _mapper.Map<ProductItem>(productDto);

            if (productDto.Id is null)
            {
                // Create new product
                var createResult = await _productRepo.CreateAsync(entity);
                if (!createResult.Success || createResult.Data is null)
                    return Response<GetProductDTO>.Fail(createResult.Message);

                var createdDto = _mapper.Map<GetProductDTO>(createResult.Data);
                return Response<GetProductDTO>.Ok(createdDto, createResult.Message);
            }

            // Update existing product
            var updateResult = await _productRepo.UpdateAsync(entity);
            if (!updateResult.Success || updateResult.Data is null)
                return Response<GetProductDTO>.Fail(updateResult.Message);

            var updatedDto = _mapper.Map<GetProductDTO>(updateResult.Data);
            return Response<GetProductDTO>.Ok(updatedDto, updateResult.Message);
        }

        public async Task<Response<Unit>> Delete(int id)
        {
            return await _productRepo.DeleteAsync(id);
        }
    }
}
