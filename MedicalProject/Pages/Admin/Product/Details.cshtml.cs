using MedicalProject.Models.Product.DTOs;
using MedicalProject.Services.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedicalProject.Pages.Admin.Product
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IProductInventoryService _inventoryService;

        public DetailsModel(IProductService productService, IProductInventoryService inventoryService)
        {
            _productService = productService;
            _inventoryService = inventoryService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostUpdateInventory(Guid productId, string totalPrice/*, int dong*/, string profit, PaymentTime profitableTime)
        {
            try
            {
                var result = await _inventoryService.Edit(new Models.Inventory.EditInventoryCommand
                {
                    dong = 6,
                    paymentTime = profitableTime,
                    productId = productId,
                    totalPrice = totalPrice,
                    profit = profit,
                });

                if (result.IsSuccess)
                {
                    TempData["Success"] = "اطلاعات مالی با موفقیت بروزرسانی شد";
                }
                else
                {
                    TempData["Error"] = "خطا در بروزرسانی اطلاعات مالی";
                }
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "خطا در بروزرسانی اطلاعات مالی: " + ex.Message;
            }

            return RedirectToPage(new { productId });
        }

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