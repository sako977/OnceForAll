using Fortius.CodingChallenge;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ConstructionLine.CodingChallenge
{
   public class SearchEngine
   {
      private readonly List<KeysValueShirts> _shirts;

      public SearchEngine(List<Shirt> shirts)
      {
         _shirts = new List<KeysValueShirts>();
         foreach (var color in Color.All)
         {
            foreach (var size in Size.All)
            {
               var temp = new KeysValueShirts();
               temp.Shirts = shirts.Where(s => s.Size.Id == size.Id && s.Color.Id == color.Id).ToList();
               temp.ColorId = color.Id;
               temp.SizeId = size.Id;
               _shirts.Add(temp);
            }
         }
      }

      public SearchResults Search(SearchOptions options)
      {
         SearchResults searchResults = new SearchResults
         {
            ColorCounts = new List<ColorCount>(),
            SizeCounts = new List<SizeCount>(),
            Shirts = new List<Shirt>()
         };

         if (!options.Colors.Any())
            options.Colors = Color.All;

         if (!options.Sizes.Any())
            options.Sizes = Size.All;

         // Size
         foreach (Size size in Size.All)
         {
            // Populate size count for each size
            searchResults.SizeCounts.Add(new SizeCount() { Size = size });
            // get matched shirts
            var matchedShirts = _shirts.Where(s => s.SizeId == size.Id && options.Colors.Select(c => c.Id).Contains(s.ColorId)).SelectMany(x => x.Shirts);

            foreach (Shirt shirt in matchedShirts)
            {
               // add to search results
               searchResults.Shirts.Add(shirt);
               searchResults.SizeCounts.First(x => x.Size == shirt.Size).Count++; // increase counter
            }
         }

         // Colour
         foreach (Color color in Color.All)
         {
            // populate colour count for each colour
            searchResults.ColorCounts.Add(new ColorCount() { Color = color });
            // get matched shirts
            var matchedShirts = _shirts.Where(c => c.ColorId == color.Id && options.Sizes.Select(s => s.Id).Contains(c.SizeId)).SelectMany(x => x.Shirts);

            foreach (Shirt shirt in matchedShirts)
            {
               // add to search results
               searchResults.Shirts.Add(shirt);
               searchResults.ColorCounts.First(x => x.Color == shirt.Color).Count++; // increase counter
            }
         }
         return searchResults;
      }
   }
}