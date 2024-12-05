using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Controllers.Bases;
using BLL.Services;
using BLL.Models;
using Newtonsoft.Json;
using BLL.DAL;

namespace MVC.Controllers
{
    public class GamesController : MvcController
    {
        // Service injections:
        private readonly IGameService _gameService;
        private readonly IPublisherService _publisherService;
        //private readonly IManyToManyRecordService _manyToManyRecordService; // Use this service for many-to-many relationships

        public GamesController(
            IGameService gameService,
            IPublisherService publisherService
           // IManyToManyRecordService manyToManyRecordService // Inject ManyToManyRecordService
        )
        {
            _gameService = gameService;
            _publisherService = publisherService;
            //_manyToManyRecordService = manyToManyRecordService;
        }

        // GET: Games
        public IActionResult Index()
        {
            var list = _gameService.Query().ToList();
            SetViewData(); // Populate view data for dropdowns or other dynamic data
            return View(list);
        }

        // GET: Games/Details/5
        public IActionResult Details(int id)
        {
            var item = _gameService.Query().SingleOrDefault(q => q.Record.Id == id);
            if (item == null)
            {
                return NotFound("Game not found.");
            }
            return View(item);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GameModel game)
        {
            if (ModelState.IsValid)
            {
                var result = _gameService.Create(game.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = game.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }

            SetViewData();
            return View(game);
        }

        // GET: Games/Edit/5
        public IActionResult Edit(int id)
        {
            var item = _gameService.Query().SingleOrDefault(q => q.Record.Id == id);
            if (item == null)
            {
                return NotFound("Game not found.");
            }
            SetViewData();
            return View(item);
        }

        // POST: Games/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GameModel game)
        {
            if (ModelState.IsValid)
            {
                var result = _gameService.Update(game.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = game.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }

            SetViewData();
            return View(game);
        }

        // GET: Games/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _gameService.Query().SingleOrDefault(q => q.Record.Id == id);
            if (item == null)
            {
                return NotFound("Game not found.");
            }
            return View(item);
        }

        // POST: Games/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _gameService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // Utility method to set view data for dropdowns and other related data
        private void SetViewData()
        {
            // Publisher select list
            ViewData["PublisherId"] = new SelectList(_publisherService.Query().ToList(), "Record.Id", "Name");

            // Serialize publishers to JSON for other uses (e.g., Ajax calls)
            var publisherList = _publisherService.Query().ToList();
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Prevent self-referencing loops
            };
            TempData["PublisherList"] = JsonConvert.SerializeObject(publisherList, settings);

            // Many-to-many service, if applicable
            // ViewBag.ManyToManyRecordIds = new MultiSelectList(_manyToManyRecordService.Query().ToList(), "Record.Id", "Name");
        }
    }

  //  public interface IManyToManyRecordService
    //{
      //  object Query();
    //}
}
