using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BLL.DAL;

namespace BLL.Models
{
    public class PublisherModel
    {
        public DAL.Publisher Record { get; set; }

        public String Name => Record.Name;

    }
}
