using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KetchupApp;
using RestSharp;
using System.Threading.Tasks;

namespace KetchupUnitTest
{
    [TestClass]
    public class KitchenTester
    {

        [TestMethod]
        public void ToKitchenAddRecipe()
        {
            Kitchen currentKitchen = new Kitchen();
            Recipe newRecipe = new Recipe();

            newRecipe.Name = "SausageStroganoff";
            newRecipe.Available = true;

            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            
            currentKitchen.AddRecipe(newRecipe);
            Recipe result = currentKitchen.GetRecipe(newRecipe.Name);

            Assert.AreEqual(newRecipe.Name, result.Name);
        }

        [TestMethod]
        public void ToKitchenAddRecipeWithoutName()
        {
            Kitchen currentKitchen = new Kitchen();
            Recipe newRecipe = new Recipe();

            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            
            Assert.AreEqual(false, currentKitchen.AddRecipe(newRecipe));

        }

        [TestMethod]
        public void ToKitchenAddRecipeWithoutIngredientsAndQuantity()
        {
            Kitchen currentKitchen = new Kitchen();
            Recipe newRecipe = new Recipe();

            newRecipe.Name = "PyttIPanna";

            Assert.AreEqual(false, currentKitchen.AddRecipe(newRecipe));

        }

        [TestMethod]
        public void FromKitchenGetRecipeSimilarName()
        {
            Kitchen currentKitchen = new Kitchen();
            Recipe newRecipe = new Recipe();

            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            newRecipe = new Recipe();

            newRecipe.Name = "SausageStroganoff";
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);
            
            Recipe result = currentKitchen.GetRecipe("SausageStroganoff");

            Assert.AreEqual(newRecipe.Name, result.Name);

        }

        [TestMethod]
        public void FromEmptyKitchenGetPossibleMeals()
        {
            Kitchen currentKitchen = new Kitchen();
            List<string> result = currentKitchen.PossibleMeals();
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FromKitchenWithoutAvailabileMealsGetPossibleMeals()
        {
            Kitchen currentKitchen = new Kitchen();

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            List<string> result = currentKitchen.PossibleMeals();
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(0, result.Count);
        }
        

        [TestMethod]
        public void FromKitchenWithAvailabileMealsAndEmptyFridgeGetPossibleMeals()
        {
            Fridge currentFridge = new Fridge();
            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);
            
            List<string> result = currentKitchen.PossibleMeals();
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FromKitchenWithAvailabileMealsAndFullFridgeGetPossibleMeals()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("VeggieSausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 7.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            List<string> result = currentKitchen.PossibleMeals();
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void FromKitchenWithAvailabileMealsAndNotEnoughFullFridgeGetPossibleMeals()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("VeggieSausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 2);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            List<string> result = currentKitchen.PossibleMeals();
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FromKitchenWithAvailabileMealAndFullFridgePrepareMeal()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("Sausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 7.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoff";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Boolean result = currentKitchen.PrepareMeal("SausageStroganoff", 1);
            
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void FromKitchenWithAvailabileMealAndFullFridgePrepareTwoMeals()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("Sausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 7.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoff";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Boolean result = currentKitchen.PrepareMeal("SausageStroganoff", 2);

            Assert.AreEqual(true, result);
            Assert.AreEqual(3, currentFridge.GetInventoryItem("Sausage").Quantity);
            Assert.AreEqual(2.5, currentFridge.GetInventoryItem("Cream").Quantity);
            Assert.AreEqual(18, currentFridge.GetInventoryItem("Tomato puree").Quantity);
            
        }

        [TestMethod]
        public void FromKitchenWithAvailabileMealAndFullFridgePrepareTooManyMeals()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("Sausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 7.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoff";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Boolean result = currentKitchen.PrepareMeal("SausageStroganoff", 4);

            Assert.AreEqual(false, result);
         }


        [TestMethod]
        public void FromKitchenWithAvailabileMealAndFullFridgeIncorrectMealPrepareMeal()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("Sausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 7.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoff";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Boolean result = currentKitchen.PrepareMeal("Squirrel", 1);

            Assert.AreEqual(false, result);
        }

         [TestMethod]
        public void FromKitchenWithNotAvailabileMealPrepareMeal()
        {
          Kitchen currentKitchen = new Kitchen();

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoff";
            newRecipe.Available = false;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Boolean result = currentKitchen.PrepareMeal("SausageStroganoff", 1);

            Assert.AreEqual(false, result);
        }



        //[TestMethod]
        //public void FromKitchenGetPossibleMeals()
        //{
        //    Kitchen currentKitchen = new Kitchen();

        //    Recipe newRecipe = new Recipe();
        //    newRecipe.Name = "SausageStroganoffVeggie";
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
        //    currentKitchen.AddRecipe(newRecipe);

        //    newRecipe = new Recipe();
        //    newRecipe.Name = "SausageStroganoff";
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
        //    currentKitchen.AddRecipe(newRecipe);

        //    newRecipe = new Recipe();
        //    newRecipe.Name = "BeefStroganoff";
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Beef", 1));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
        //    currentKitchen.AddRecipe(newRecipe);

        //    newRecipe = new Recipe();
        //    newRecipe.Name = "CheeseStroganoff";
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cheese", 1));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
        //    newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
        //    currentKitchen.AddRecipe(newRecipe);

        //    List<Recipe> result = currentKitchen.PossibleMeals();

        //    Assert.AreEqual(false, result == null);

        //}

        [TestMethod]
        public void IntegrateWithFridgeTest()
        {

          RestClient  fridgeClient = new RestClient("http://localhost:14589");

            
            var request = new RestRequest("api/fridge/", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new FridgeInventory("Sausage", 5)
            );

            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(fridgeClient, request) as RestResponse;
            }).Wait();

            ////

            request = new RestRequest("api/fridge/", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new FridgeInventory("Cream", 7.5)
            );

            response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(fridgeClient, request) as RestResponse;
            }).Wait();

            ////

            request = new RestRequest("api/fridge/", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new FridgeInventory("Tomato puree", 22)
            );

            response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(fridgeClient, request) as RestResponse;
            }).Wait();



            Kitchen currentKitchen = new Kitchen();

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoff";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Sausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Boolean result = currentKitchen.PrepareMeal("SausageStroganoff", 3);

            Assert.AreEqual(true, result);
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

    }
}
