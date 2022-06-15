﻿using System.Collections;
using BLL.Helpers;
using BLL.Logger;
using DAL.Repositories;
using Entities;
using Entities.Goods;

namespace BLL.Services;

public class ProductService : IProductService
{
    private const string Msg = "User not logged in";
    private readonly ILogger _logger;
    private readonly IRepository<Product> _productRepository;
    private readonly CustomTokenHandler _tokenHandler;

    public ProductService(IRepository<Product> repository, CustomTokenHandler tokenHandler, ILogger logger)
    {
        _productRepository = repository;
        _tokenHandler = tokenHandler;
        _logger = logger;
    }

    public Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        return Task<IEnumerable<Product>>.Factory.StartNew(() => _productRepository.GetAll().Where(x => x.Name == name));
    }
    public Task<Product> GetProductById(string token, int id)
    {
        return Task<Product>.Factory.StartNew(() =>
        {
            if (_tokenHandler.ValidateToken(token))
                return _productRepository.GetById(id);

            _logger.LogException($"{nameof(ProductService)}.{nameof(GetProductById)} throw exception." + Msg);
            throw new ServiceException(nameof(ProductService), Msg);
        });
    }

    public Task<IEnumerable<Product>> GetAllProducts(string token)
    {
        return Task<IEnumerable<Product>>.Factory.StartNew(
            () =>
            {
                if (_tokenHandler.ValidateToken(token)) return _productRepository.GetAll();
                _logger.LogException($"{nameof(ProductService)}.{nameof(GetAllProducts)} throw exception. " + Msg);
                throw new ServiceException(nameof(ProductService), Msg);
            });
    }

    public Task<IEnumerable<Product>> GetProductsFromOrder(string token, Order order)
    {
        return Task<IEnumerable<Product>>.Factory.StartNew(() =>
        {
            var msg = string.Empty;
            if (_tokenHandler.ValidateToken(token))
            {
                var us = _tokenHandler.GetUser(token);
                if (us is { IsAdmin: true })
                {
                    _logger.Log(
                        $"Admin {us.Name} invoked to get products from order id{order.Id}");
                    return order.Products;
                }
                if(order.Owner==us)
                    return order.Products;
                msg = $"{us.Name} do not have permission";
            }

            if (string.IsNullOrEmpty(msg))
                msg = $"Token is bad";
            _logger.LogException($"{nameof(ProductService)}.{nameof(GetProductsFromOrder)} throw exception. " + msg);
            throw new ServiceException(nameof(ProductService), msg);

        });
    }
}