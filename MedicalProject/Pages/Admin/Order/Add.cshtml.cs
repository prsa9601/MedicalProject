using MedicalProject.Models.Product.DTOs;
using MedicalProject.Models.User.DTOs;
using MedicalProject.Services.Product;
using MedicalProject.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MedicalProject.Pages.Admin.Order
{
    public class AddModel : PageModel
    {

        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public AddModel(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }


        [BindProperty(SupportsGet = true)]
        public ProductFilterForIndexPageResult Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserDto? User { get; set; }

        public async Task OnGet(Guid userId)
        {
            User = await _userService.GetUserById(userId);

        }
    }
}
