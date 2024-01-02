using Microsoft.AspNetCore.Mvc;
using WebFrontToBack.Services;

namespace WebFrontToBack.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly LayoutService _layoutService;

        public HeaderViewComponent(LayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _layoutService.GetSettings());
        }
    }
}
