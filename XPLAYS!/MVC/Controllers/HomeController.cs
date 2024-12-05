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

        // T�m oyunlar� listeleyen bir action
        public IActionResult Index()
        {
            var games = _gameService.Query().ToList(); // T�m oyunlar� al
            return View(games); // View'e g�nder
        }

        // Yeni bir oyun eklemek i�in action
        [HttpPost]
        public IActionResult Create(GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                var result = _gameService.Create(gameModel.Record);
                if (result.IsSuccess)
                    return RedirectToAction(nameof(Index)); // Ba�ar�l�ysa listeye d�n
                else
                    ModelState.AddModelError("", result.Message); // Hata varsa g�ster
            }

            return View(gameModel); // Hatalar� yeniden g�ster
        }

        // Oyun silmek i�in action
        public IActionResult Delete(int id)
        {
            var result = _gameService.Delete(id);
            if (result.IsSuccess)
                return RedirectToAction(nameof(Index)); // Ba�ar�l�ysa listeye d�n

            TempData["ErrorMessage"] = result.Message; // Hata mesaj�n� TempData ile View'e ta��
            return RedirectToAction(nameof(Index));
        }

        // G�ncelleme sayfas�
        public IActionResult Edit(int id)
        {
            var game = _gameService.Query().FirstOrDefault(x => x.Record.Id == id);
            if (game == null)
                return NotFound(); // Oyun bulunamad�ysa hata d�nd�r

            return View(game);
        }

        [HttpPost]
        public IActionResult Edit(GameModel gameModel)
        {
            if (ModelState.IsValid)
            {
                var result = _gameService.Update(gameModel.Record);
                if (result.IsSuccess)
                    return RedirectToAction(nameof(Index)); // Ba�ar�l�ysa listeye d�n
                else
                    ModelState.AddModelError("", result.Message); // Hata varsa g�ster
            }

            return View(gameModel); // Hatalar� yeniden g�ster
        }
    }
}
