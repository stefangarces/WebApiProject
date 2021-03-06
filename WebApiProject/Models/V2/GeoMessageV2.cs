using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Models.V2
{
    public class GeoMessageV2
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
