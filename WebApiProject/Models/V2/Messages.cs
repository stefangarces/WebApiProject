using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Models.V2
{
    public class Messages
    {
        public string Body { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
