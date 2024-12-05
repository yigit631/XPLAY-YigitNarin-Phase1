using BLL.DAL;
using System;

namespace BLL.Models
{
    public class GameModel
    {
        public Game Record { get; set; }

        public string Name => Record.Name;

        public string photoUrl => Record?.photoUrl?.ToString() ?? "No Photo";  

        public string ReleaseDate => Record.ReleaseDate.HasValue ? Record.ReleaseDate.Value.ToString("MM/dd/yyyy") : "";

        public string Price => Record.Price.ToString("C2");

        public string Publisher => Record.Publisher?.Name;
    }
}
