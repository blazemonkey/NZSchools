using System;
using System.Collections.Generic;
using System.Text;

namespace NZSchools.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string ImagePath { get; set; }

        public static IEnumerable<Region> CreateList()
        {
            var regions = new List<Region>();
            var northland = new Region() { Id = 1, Name = "northland", Order = 1, ImagePath = "northland.png" };
            var auckland = new Region() { Id = 2, Name = "auckland", Order = 2, ImagePath = "auckland.png" };
            var waikato = new Region() { Id = 3, Name = "waikato", Order = 3, ImagePath = "waikato.png" };
            var bayOfPlenty = new Region() { Id = 4, Name = "bay of plenty", Order = 4, ImagePath = "bayofplenty.png" };
            var gisborne = new Region() { Id = 5, Name = "gisborne", Order = 5, ImagePath = "gisborne.png" };
            var hawkesbay = new Region() { Id = 6, Name = "hawke's bay", Order = 6, ImagePath = "hawkesbay.png" };
            var taranaki = new Region() { Id = 7, Name = "taranaki", Order = 7, ImagePath = "taranaki.png" };
            var manawatu = new Region() { Id = 8, Name = "manawatu-wanganui", Order = 8, ImagePath = "manawatu.png" };
            var wellington = new Region() { Id = 9, Name = "wellington", Order = 9, ImagePath = "wellington.png" };
            var tasman = new Region() { Id = 10, Name = "tasman", Order = 10, ImagePath = "tasman.png" };
            var nelson = new Region() { Id = 11, Name = "nelson", Order = 11, ImagePath = "nelson.png" };
            var marlborough = new Region() { Id = 12, Name = "marlborough", Order = 12, ImagePath = "marlborough.png" };
            var westCoast = new Region() { Id = 13, Name = "west coast", Order = 13, ImagePath = "westcoast.png" };
            var canterbury = new Region() { Id = 14, Name = "canterbury", Order = 14, ImagePath = "canterbury.png" };
            var otago = new Region() { Id = 15, Name = "otago", Order = 15, ImagePath = "otago.png" };
            var southland = new Region() { Id = 16, Name = "southland", Order = 16, ImagePath = "southland.png" };

            regions.Add(northland);
            regions.Add(auckland);
            regions.Add(waikato);
            regions.Add(bayOfPlenty);
            regions.Add(gisborne);
            regions.Add(hawkesbay);
            regions.Add(taranaki);
            regions.Add(manawatu);
            regions.Add(wellington);
            regions.Add(tasman);
            regions.Add(nelson);
            regions.Add(marlborough);
            regions.Add(westCoast);
            regions.Add(canterbury);
            regions.Add(otago);
            regions.Add(southland);
            return regions;
        }
    }
}
