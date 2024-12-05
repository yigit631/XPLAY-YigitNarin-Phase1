using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService _gameService;

        public HomeController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // Tüm oyunlarý listeleyen bir action
        public IActionResult Index()
        {
            var games = _gameService.Query().ToList(); // Tüm oyunlarý al
            return View(games); // View'e gönder
        }

        // Yeni bir oyun eklemek için action
        [HttpPost]
        public IActionResult Create(GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                var result = _gameService.Create(gameModel.Record);
                if (result.IsSuccess)
                    return RedirectToAction(nameof(Index)); // Baþarýlýysa listeye dön
                else
                    ModelState.AddModelError("", result.Message); // Hata varsa göster
            }

            return View(gameModel); // Hatalarý yeniden göster
        }

        // Oyun silmek için action
        public IActionResult Delete(int id)
        {
            var result = _gameService.Delete(id);
            if (result.IsSuccess)
                return RedirectToAction(nameof(Index)); // Baþarýlýysa listeye dön

            TempData["ErrorMessage"] = result.Message; // Hata mesajýný TempData ile View'e taþý
            return RedirectToAction(nameof(Index));
        }

        // Güncelleme sayfasý
        public IActionResult Edit(int id)
        {
            var game = _gameService.Query().FirstOrDefault(x => x.Record.Id == id);
            if (game == null)
                return NotFound(); // Oyun bulunamadýysa hata döndür

            return View(game);
        }

        [HttpPost]
        public IActionResult Edit(GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                var result = _gameService.Update(gameModel.Record);
                if (result.IsSuccess)
                    return RedirectToAction(nameof(Index)); // Baþarýlýysa listeye dön
                else
                    ModelState.AddModelError("", result.Message); // Hata varsa göster
            }

            return View(gameModel); // Hatalarý yeniden göster
        }
    }
}
