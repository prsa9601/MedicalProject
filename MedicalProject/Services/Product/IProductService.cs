using MedicalProject.Models;
using MedicalProject.Models.Product;
using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.User.DTOs;
using Microsoft.AspNetCore.SignalR;
using System;

namespace MedicalProject.Services.Product
{
    public interface IProductService
    {
        Task<ApiResult> Create(CreateProductCommand command);
        Task<ApiResult> Edit(EditProductCommand command);
        Task<ApiResult> Remove(Guid productId);


        Task<ProductDto?> GetById(Guid productId);
        Task<ProductDto?> GetBySlug(string slug);
        Task<ProductFilterResult> GetFilter(ProductFilterParam param);
        Task<ProductFilterForIndexPageResult> GetFilterForIndexPage(ProductFilterParam param);
    }
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        private const string ModuleName = "Product";
        public ProductService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResult> Create(CreateProductCommand command)
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(command.title), "title");
            form.Add(new StringContent(command.description), "description");
            form.Add(new StringContent(command.slug), "slug");
            if (command.Canonical is not null)
                form.Add(new StringContent(command.Canonical), "Canonical");

            if (command.MetaTitle is not null)
                form.Add(new StringContent(command.MetaTitle), "MetaTitle");

            if (command.MetaDescription is not null)
                form.Add(new StringContent(command.MetaDescription), "MetaDescription");

            if (command.Schema is not null)
                form.Add(new StringContent(command.Schema), "Schema");

            if (command.MetaKeyWords is not null)
                form.Add(new StringContent(command.MetaKeyWords), "MetaKeyWords");

            if (command.IndexPage is not null)
                form.Add(new StringContent(command.IndexPage.ToString()), "IndexPage");

            form.Add(new StringContent(command.status.ToString()), "status");
            form.Add(
                    new StreamContent(command.Image.OpenReadStream()),
                    "Image",
                    command.Image.FileName
                );


            var result = await _client.PostAsync($"{ModuleName}/CreateProduct", form);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Edit(EditProductCommand command)
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(command.title), "title");
            form.Add(new StringContent(command.productId.ToString()), "productId");
            form.Add(new StringContent(command.description), "description");
            form.Add(new StringContent(command.slug), "slug");

            if (command.Canonical is not null)
                form.Add(new StringContent(command.Canonical), "Canonical");

            if (command.MetaTitle is not null)
                form.Add(new StringContent(command.MetaTitle), "MetaTitle");

            if (command.MetaDescription is not null)
                form.Add(new StringContent(command.MetaDescription), "MetaDescription");

            if (command.Schema is not null)
                form.Add(new StringContent(command.Schema), "Schema");

            if (command.MetaKeyWords is not null)
                form.Add(new StringContent(command.MetaKeyWords), "MetaKeyWords");

            if (command.IndexPage is not null)
                form.Add(new StringContent(command.IndexPage.ToString()), "IndexPage");
         
            if (command.status != null)
                form.Add(new StringContent(command.status.ToString()), "Status");

            if (command.Image is not null)
            {
                form.Add(
                        new StreamContent(command.Image.OpenReadStream()),
                        "Image",
                        command.Image.FileName
                    );
            }

            var result = await _client.PatchAsync($"{ModuleName}/EditProduct", form);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ProductFilterResult> GetFilter(ProductFilterParam param)
        {
            string url = $"{ModuleName}/GetProductByFilter?take={param.Take}&pageId={param.PageId}";
            if (param.Title is not null)
                url += $"&title={param.Title}";
            if (param.Status is not null)
                url += $"&status={param.Status}";

            var result = await _client.GetFromJsonAsync<ApiResult<ProductFilterResult>>(url);
            return result?.Data;
        }

        public async Task<ProductDto?> GetById(Guid productId)
        {
            var result = await _client.GetFromJsonAsync<ApiResult<ProductDto?>>($"{ModuleName}/GetProductById?productId={productId}");
            return result?.Data;
        }

        public async Task<ApiResult> Remove(Guid productId)
        {
            var result = await _client.DeleteAsync($"{ModuleName}/RemoveProduct?productId={productId}");
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ProductDto?> GetBySlug(string slug)
        {
            var result = await _client.GetFromJsonAsync<ApiResult<ProductDto?>>($"{ModuleName}/GetProductBySlug?slug={slug}");
            return result?.Data;
        }

        public async Task<ProductFilterForIndexPageResult> GetFilterForIndexPage(ProductFilterParam param)
        {
            string url = $"{ModuleName}/GetProductByFilterForIndex?take={param.Take}&pageId={param.PageId}";
            if (param.Title is not null)
                url += $"&title={param.Title}";
            if (param.Status is not null)
                url += $"&status={param.Status}";

            var result = await _client.GetFromJsonAsync<ApiResult<ProductFilterForIndexPageResult>>(url);
            return result?.Data;
        }
    }
}
