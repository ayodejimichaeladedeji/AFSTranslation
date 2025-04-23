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
        private ITranslatorService _translationService;
        private readonly IConfiguration _configuration;
        private readonly ITranslatorFactory _translatorFactory;
        private readonly ITranslationLogService _translationLogService;

        public TranslateController(ITranslatorFactory translatorFactory, IConfiguration configuration, ITranslationLogService translationLogService)
        {
            _configuration = configuration;
            _translatorFactory = translatorFactory;
            _translationLogService = translationLogService;
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
        {
            if (!ModelState.IsValid)
            {
                model.TranslationModes = _translationService.Modes;
                return View(model);
            }
            
            var result = await _translationService.Translate(model.SelectedMode, model.TextToTranslate!);

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

        [HttpGet]
        public IActionResult TranslationLogs() => View();

        [HttpGet]
        public async Task<IActionResult> GetUserTranslationLogs()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.PrimarySid)?.Value!);
            var logs = await _translationLogService.GetUserTranslationLogs(userId);
            return Json(logs);
        }
    }
}