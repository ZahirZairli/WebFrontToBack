using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.Services;

namespace WebFrontToBack.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly LayoutService _layoutService;

        public FooterViewComponent(LayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _layoutService.GetSettings());
        }
    }
}
