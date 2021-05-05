using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Models
{
    public class Token
    {
        public int Id { get; set; }
        public Guid Key { get; set; }
        public MyUser User { get; set; }
    }
}
