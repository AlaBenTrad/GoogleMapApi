using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetC
{
    class Donnes
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Type { get; set; }
        public int Rayon { get; set; }
        public string Transport { get; set; }
        public int Zoom { get; set; }
        public string MapType { get; set; } 
       
        public Donnes()
        {
            Latitude = "";
            Longitude = "";
            Type = "";
            Rayon = 0;
            Transport = "";
            Zoom = 0;
            MapType = "";
        }

    }
}
