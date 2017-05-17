using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace KetchupApp
{
    public class Recipe
    {
        public Recipe()
        {
            IngredientsAndQuantity = new List<KeyValuePair<string, double>>();
        }


        public string Name;
        public bool Available;

        public List<KeyValuePair<string, double>> IngredientsAndQuantity { get; set; }

    }
}


