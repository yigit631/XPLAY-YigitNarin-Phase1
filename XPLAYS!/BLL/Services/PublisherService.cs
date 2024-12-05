using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BLL.Services
{
    public interface IPublisherService
    {
        IQueryable<PublisherModel> Query();
        ServiceBase Create(Publisher record);
        ServiceBase Update(Publisher record);
        ServiceBase Delete(int id);
    }

    public class PublisherService : ServiceBase, IPublisherService
    {
        public PublisherService(Db db) : base(db)
        {
        }

        public ServiceBase Create(Publisher record)
        {
            if (_db.Publishers.Any(x => x.Name.ToUpper() == record.Name.ToUpper().Trim()))
                return Error("Publisher with this name exists");
            record.Name = record.Name?.Trim();
            _db.Publishers.Add(record);
            _db.SaveChanges();
            return Success("Added successfully");
        }

        public ServiceBase Delete(int id)
        {
            var publisher = _db.Publishers.Include(p => p.Games).SingleOrDefault(p => p.Id == id);
            
            if (publisher == null)
                return Error("Publisher not found");
            if (publisher.Games.Count != 0)
            {

                return Error("Cannot delete publisher with associated games.");
            }

            _db.Publishers.Remove(publisher);
            _db.SaveChanges();
            return Success("Publisher deleted successfully");
        }


        public IQueryable<PublisherModel> Query()
        {
            return _db.Publishers.OrderBy(x => x.Name).Select(x => new PublisherModel() { Record = x });
        }

        public ServiceBase Update(Publisher record)
        {
            if (record == null)
                return Error("Invalid publisher record.");

            var existingPublisher = _db.Publishers.FirstOrDefault(x => x.Id == record.Id);

            if (existingPublisher == null)
                return Error("Publisher not found.");

            if (_db.Publishers.Any(x => x.Id != record.Id && x.Name.ToUpper() == record.Name.ToUpper().Trim()))
                return Error("Another publisher with this name already exists.");

            existingPublisher.Name = record.Name?.Trim();
            _db.Entry(existingPublisher).State = EntityState.Modified;
            _db.SaveChanges();

            return Success("Updated successfully.");
        }

    }

}