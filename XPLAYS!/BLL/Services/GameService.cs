using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{

    public interface IGameService
    {
        public IQueryable<GameModel> Query();

        public ServiceBase Create(Game record);

        public ServiceBase Update(Game record);

        public ServiceBase Delete(int id);

        Task<ServiceBase> UploadGamePhoto(IFormFile photo, int gameId);
    }
    public class GameService : ServiceBase, IGameService
    {
        public GameService(Db db) : base(db)
        {
        }

        public ServiceBase Create(Game record)
        {
            if (_db.Games.Any(x => x.Name.ToUpper() == record.Name.ToUpper().Trim()))
                return Error("Game with this name exists");
            record.Name = record.Name?.Trim();
            _db.Games.Add(record);
            _db.SaveChanges();
            return Success("Added successfully");

        }

        public ServiceBase Delete(int id)
        {
            // Verilen ID'ye sahip oyun kaydını bul
            var entity = _db.Games.SingleOrDefault(x => x.Id == id);

            // Kayıt bulunamazsa hata döndür
            if (entity == null)
                return Error("Game not found.");

            // Kayıt varsa sil ve değişiklikleri kaydet
            _db.Games.Remove(entity);
            _db.SaveChanges();

            return Success("Game deleted successfully.");
        }


        public IQueryable<GameModel> Query()
        {
            return _db.Games.Include(x => x.Publisher).OrderBy(x => x.Name).Select(x => new GameModel() { Record = x });
        }

        public ServiceBase Update(Game record)
        {
            if (record == null)
                return Error("Invalid game record.");

            // Verilen ID'ye sahip oyun kaydını bul
            var entity = _db.Games.SingleOrDefault(x => x.Id == record.Id);

            // Kayıt bulunamazsa hata döndür
            if (entity == null)
                return Error("Game not found.");

            // Aynı isimde başka bir oyun var mı kontrol et
            if (_db.Games.Any(x => x.Id != record.Id && x.Name.ToUpper() == record.Name.ToUpper().Trim()))
                return Error("Another game with this name already exists.");

            // Güncellemeleri uygula
            entity.Name = record.Name?.Trim();
            entity.ReleaseDate = record.ReleaseDate; // Örnek olarak ek bir alan
            entity.Price = record.Price;
            entity.Publisher = record.Publisher; // Örnek: türü güncelle

            // Değişiklikleri kaydet
            _db.SaveChanges();

            return Success("Game updated successfully.");
        }
        public async Task<ServiceBase> UploadGamePhoto(IFormFile photo, int gameId)
        {
            if (photo == null || photo.Length == 0)
            {
                return Error("No file uploaded.");
            }

            var game = _db.Games.SingleOrDefault(x => x.Id == gameId);
            if (game == null)
            {
                return Error("Game not found.");
            }

            // Fotoğraf adı oluşturma ve dosya yolu belirleme
            var fileName = Path.GetFileName(photo.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // Dosya kaydetme işlemi
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Oyun nesnesinin fotoğraf URL'sini güncelleme
            game.photoUrl = $"/images/{fileName}";

            // Değişiklikleri kaydet
            _db.SaveChanges();

            return Success("Game photo uploaded successfully.");
        }
    }
}
