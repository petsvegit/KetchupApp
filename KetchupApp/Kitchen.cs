using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KetchupApp
{
    public class Kitchen
    {
        public List<Recipe> KitchenRecipes = new List<Recipe>();
        private Fridge _currentFridge;
        private RestClient _fridgeClient;

        public Kitchen(Fridge currentFridge)
        {
            this._currentFridge = currentFridge;

            _fridgeClient = new RestClient("http://localhost:14589");

        }

        public Kitchen()
        {
            this._currentFridge = new Fridge();
            _fridgeClient = new RestClient("http://localhost:14589");

        }

        public bool AddRecipe(Recipe newRecipe)
        {
            if (ValidateRecipe(newRecipe) == false) {return false;}
            KitchenRecipes.Add(newRecipe);
            return true;
        }

        private bool ValidateRecipe(Recipe newRecipe)
        {
            if (newRecipe?.Name == null) { return false; }
            return ValidateIngredientsAndQuantity(newRecipe.IngredientsAndQuantity);
        }

        private bool ValidateIngredientsAndQuantity(List<KeyValuePair<string, double>> newRecipeIngredientsAndQuantity)
        {
            if (newRecipeIngredientsAndQuantity.Count == 0) { return false; }
            return newRecipeIngredientsAndQuantity.All(item => item.Key != null);
        }

        public Recipe GetRecipe(string name)
        {
            return KitchenRecipes.FirstOrDefault(recipeToCheck => recipeToCheck.Name.Equals(name));
        }

        private List<Recipe> PossibleRecipes()
        {
            List<Recipe> availableRecipes = KitchenRecipes.FindAll(recipeToReturn => recipeToReturn.Available.Equals(true));
            List<Recipe> possibleRecipes = new List<Recipe>();

            foreach (var recipe in availableRecipes)
            {
                if (IsRecepieAvailableFromFridge(recipe)) { possibleRecipes.Add(recipe);}
            }

            return possibleRecipes;
        }

        public List<string> PossibleMeals()
        {
            List<string> mealNames = new List<string>();
            List<Recipe> possibleRecipes = PossibleRecipes();

            foreach (var recipe in possibleRecipes)
            {
                mealNames.Add(recipe.Name);
            }
            return mealNames;
        }

        private bool IsRecepieAvailableFromFridge(Recipe recipe)
        {
            return IsRecepieAvailableFromFridge(recipe, 1);
        }


        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }


        private bool  IsRecepieAvailableFromFridge(Recipe recipe, int noOfMeals)
        {
            foreach (var ingredientAndQuantity in recipe.IngredientsAndQuantity)
            {

                var client = new RestClient("http://localhost:14589");
                var request = new RestRequest("api/values/{name}/{quantity}", Method.GET);
                request.AddUrlSegment("name", ingredientAndQuantity.Key); // replaces matching token in request.Resource
                request.AddUrlSegment("quantity", ingredientAndQuantity.Value * noOfMeals);
           
                var response = new RestResponse();
                Task.Run(async () =>
                {
                    response = await GetResponseContentAsync(client, request) as RestResponse;
                }).Wait();

                if (JsonConvert.DeserializeObject<bool>(response.Content) == false) return false;
                
            }
            return true;
        }

        public bool PrepareMeal(string meal, int noOfMeals)
        {
            Recipe recipe = GetRecipe(meal);

            if (recipe == null) {return false;}
            if (recipe.Available == false) { return false; }
            if (IsRecepieAvailableFromFridge(recipe, noOfMeals) == false) { return false; }

            foreach (var ingredientAndQuantity in recipe.IngredientsAndQuantity)
            {
                _currentFridge.TakeItemFromFridge(ingredientAndQuantity.Key, ingredientAndQuantity.Value*noOfMeals);
            }

            return true;
        }
    }   
}
