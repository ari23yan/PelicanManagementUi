using Microsoft.AspNetCore.Mvc;

namespace PelicanManagementUi.ViewComponents
{
    public class SideBarViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var categories = await _categoryRepository.GetAllParentCategoriesAsync();
            //var /*viewModel*/ = _mapper.Map<List<GetAllCategoryViewModel>>(categories);
            //return View("Category", viewModel);


            return View("SideBar");
        }
    }
}
