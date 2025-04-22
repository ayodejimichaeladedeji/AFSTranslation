using AFSTranslator.Models;
using Microsoft.AspNetCore.Mvc;
using AFSTranslator.Entities.Enums;
using AFSTranslator.Interfaces.Factory;
using Microsoft.AspNetCore.Authorization;
using AFSTranslator.Interfaces.Services;
using AFSTranslator.Entities;

namespace AFSTranslator.Controllers
{
    [Authorize]
    public class TranslateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ITranslatorFactory _translatorFactory;
        private ITranslatorService _translationService;

        public TranslateController(ITranslatorFactory translatorFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _translatorFactory = translatorFactory;
            _translationService = _translatorFactory.GetTranslatorService();
        }

        public IActionResult Translate()
        {
            var model = new TranslateViewModel
            {
                TranslationModes = _translationService.Modes
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Translate(TranslateViewModel model)
        // public IActionResult Translate(TranslateViewModel model)
        {
            var result = await _translationService.Translate(model.SelectedMode, model.TextToTranslate!);

            // Result<string> result = new();

            // result.Content = "wh3r3 7]-[IS 15 l0tz 0F lOv3 7h3RE !z lOTs Of ph1gH7|ng.";
            // result.IsSuccess = true;

            if (result.IsSuccess)
            {
                model.TranslatedText = result.Content;
                model.TranslationModes = _translationService.Modes;
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", result.ErrorMessage);
                model.TranslationModes = _translationService.Modes;
                return View(model);
            }
        }
    }
}