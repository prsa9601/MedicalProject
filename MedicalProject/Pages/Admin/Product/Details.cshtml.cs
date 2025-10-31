using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Product
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        public ProductDto Product { get; set; }

        public async Task<IActionResult> OnGet(Guid productId)
        {
            Product = await _productService.GetById(productId);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        //public async Task<IActionResult> OnPostUpdateInventory(int productId, string totalPrice, int dong, string profit, PaymentTime profitableTime)
        //{
        //    try
        //    {
        //        var result = await _productService.UpdateInventory(productId, new InventoryDto
        //        {
        //            TotalPrice = totalPrice,
        //            Dong = dong,
        //            Profit = profit,
        //            ProfitableTime = profitableTime
        //        });

        //        if (result)
        //        {
        //            TempData["Success"] = "اطلاعات مالی با موفقیت بروزرسانی شد";
        //        }
        //        else
        //        {
        //            TempData["Error"] = "خطا در بروزرسانی اطلاعات مالی";
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        TempData["Error"] = "خطا در بروزرسانی اطلاعات مالی: " + ex.Message;
        //    }

        //    return RedirectToPage(new { productId });
        //}

        //public async Task<IActionResult> OnPostToggleStatus(int productId)
        //{
        //    try
        //    {
        //        var result = await _productService.ToggleStatus(productId);

        //        if (result)
        //        {
        //            TempData["Success"] = "وضعیت محصول با موفقیت تغییر کرد";
        //        }
        //        else
        //        {
        //            TempData["Error"] = "خطا در تغییر وضعیت محصول";
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        TempData["Error"] = "خطا در تغییر وضعیت محصول: " + ex.Message;
        //    }

        //    return RedirectToPage(new { productId });
        //}
    }
}