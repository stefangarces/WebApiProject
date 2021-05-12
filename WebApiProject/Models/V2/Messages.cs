using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Models.V2
{
    public class Messages
    {
        [NotMapped]
        public object Message { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
