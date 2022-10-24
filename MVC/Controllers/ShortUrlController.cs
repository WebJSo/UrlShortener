using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Models;

namespace MVC.Controllers
{
    /// <summary>
    /// Allows CRUD operations for short urls using IShortUrlServices
    /// Performs redirects from short url input to long url output
    /// </summary>
    public class ShortUrlController : Controller
    {
        private readonly IShortUrlService _shortUrlService;
        private readonly string _errorNotValidUrl = "Please enter valid URL";
        private readonly string _redirectUrl = "/" + nameof(ShortUrlController).Replace("Controller", string.Empty).ToLower();

        public ShortUrlController(IShortUrlService shortUrlService) => _shortUrlService = shortUrlService;

        [HttpGet]
        public IActionResult RedirectToUrl(string shortUrl)
        {
            // get redirect url from short url in db and redirect, if no results redirect to default redirect url
            string redirectUrl = _shortUrlService.GetRedirectUrl(shortUrl, _redirectUrl);
            
            return Redirect(redirectUrl);
        }

        [HttpGet]
        public IActionResult AddUrl(int? id)
        {
            // if valid id, load url results to display after creating
            CreateUrlViewModel viewModel = _shortUrlService.GetUrlById(id ?? 0, Request.Host.Value);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddUrl(CreateUrlViewModel viewModel)
        {
            int id = _shortUrlService.AddUrl(viewModel.Url);

            // success, added, redirect to create view with id to display item
            if (id != 0)
                return RedirectToAction(nameof(ShortUrlController.AddUrl), new { id });

            // failed to add, output error and return view
            ModelState.AddModelError(string.Empty, _errorNotValidUrl);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ManageUrls(bool error)
        {
            // get all short urls from the database and combine with host into full redirect url
            ManageUrlViewModel viewModel = _shortUrlService.GetAllUrls(Request.Host.Value);

            // add error to display
            if (error)
                ModelState.AddModelError(string.Empty, _errorNotValidUrl);

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateUrl(int id, string url)
        {
            // return status of update to view
            bool error = !_shortUrlService.UpdateUrl(id, url);

            return RedirectToAction(nameof(ShortUrlController.ManageUrls), new { error });
        }

        [HttpPost]
        public IActionResult RemoveUrl(int id)
        {
            // return status of remove to view
            bool error = !_shortUrlService.RemoveUrl(id);

            return RedirectToAction(nameof(ShortUrlController.ManageUrls), new { error });
        }
    }
}
