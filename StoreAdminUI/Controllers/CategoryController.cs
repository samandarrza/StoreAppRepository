using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StoreAdminUI.ViewModels;

namespace StoreAdminUI.Controllers
{
    public class CategoryController : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            string url = "https://localhost:7163/admin/api/Categories?page="+page;
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();

                PaginatedVM<CategoryListItemVM> model = JsonConvert.DeserializeObject<PaginatedVM<CategoryListItemVM>>(content);
                return View(model);
            }

            return View("error");
        }
    }
}
