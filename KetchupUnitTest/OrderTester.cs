using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KetchupApp;


namespace KetchupUnitTest
{
    [TestClass]

    public class OrderTester
    {

        [TestMethod]
        public void FromEmptyKitchenAndEmptyFridgeListAvailableMeals()
        {
            Order currentOrder = new Order();

            List<string> result = currentOrder.AvailableMeals();
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(0, result.Count);
           
        }

        [TestMethod]
        public void FromFullKitchenAndFullFridgeListAvailableMeals()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("VeggieSausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 2.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Order currentOrder = new Order(currentKitchen);

            List<string> result = currentOrder.AvailableMeals();
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void FromFullKitchenAndFullFridgePlaceOrder()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("VeggieSausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 2.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));

            Kitchen currentKitchen = new Kitchen(currentFridge);
            currentKitchen.AddRecipe(newRecipe);

            MealOrder mealOrder = new MealOrder();
            mealOrder.MealName = "SausageStroganoffVeggie";
            mealOrder.NoOfMeals = 1;

            Order currentOrder = new Order(currentKitchen);
            List<MealOrder> mealOrders = new List<MealOrder>();

            mealOrders.Add(mealOrder);
            List<OrderInfo> result = currentOrder.PlaceOrder(mealOrders);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("SausageStroganoffVeggie", result[0].MealName);
            Assert.AreEqual(1, result[0].NoOfMeals);
            Assert.AreEqual(true, result[0].OkFromKitchen);

        }

        [TestMethod]
        public void FromFullKitchenAndNotEnoughFridgePlaceOrder()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("VeggieSausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 2.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));

            Kitchen currentKitchen = new Kitchen(currentFridge);
            currentKitchen.AddRecipe(newRecipe);
            
            MealOrder mealOrder = new MealOrder();
            mealOrder.MealName = "SausageStroganoffVeggie";
            mealOrder.NoOfMeals = 4;

            Order currentOrder = new Order(currentKitchen);
            List<MealOrder> mealOrders = new List<MealOrder>();

            mealOrders.Add(mealOrder);
            List<OrderInfo> result = currentOrder.PlaceOrder(mealOrders);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("SausageStroganoffVeggie", result[0].MealName);
            Assert.AreEqual(4, result[0].NoOfMeals);
            Assert.AreEqual(false, result[0].OkFromKitchen);

        }

        [TestMethod]
        public void FromFullKitchenAndNotFullEnoughFridgePlaceTwoOrders()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("VeggieSausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 2.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            currentFridge.AddIngredientToFridge("Tomato sauce", 10);
            currentFridge.AddIngredientToFridge("Ham", 25);
            currentFridge.AddIngredientToFridge("Cheese", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            newRecipe = new Recipe();
            newRecipe.Name = "Pizza";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato Sauce", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Ham", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cheese", 1));
            currentKitchen.AddRecipe(newRecipe);

            Order currentOrder = new Order(currentKitchen);
            List<MealOrder> mealOrders = new List<MealOrder>();

            MealOrder mealOrder = new MealOrder();
            mealOrder.MealName = "SausageStroganoffVeggie";
            mealOrder.NoOfMeals = 1;
            mealOrders.Add(mealOrder);

            mealOrder = new MealOrder();
            mealOrder.MealName = "Pizza";
            mealOrder.NoOfMeals = 20;
            mealOrders.Add(mealOrder);

            List<OrderInfo> result = currentOrder.PlaceOrder(mealOrders);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(false, result[1].OkFromKitchen);
            Assert.AreEqual(true, result[0].OkFromKitchen);

        }

        [TestMethod]
        public void FromFullKitchenAndFullFridgePlaceZeroOrders()
        {
            Fridge currentFridge = new Fridge();
            currentFridge.AddIngredientToFridge("VeggieSausage", 5);
            currentFridge.AddIngredientToFridge("Cream", 2.5);
            currentFridge.AddIngredientToFridge("Tomato puree", 22);

            Kitchen currentKitchen = new Kitchen(currentFridge);

            Recipe newRecipe = new Recipe();
            newRecipe.Name = "SausageStroganoffVeggie";
            newRecipe.Available = true;
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("VeggieSausage", 1));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Cream", 2.5));
            newRecipe.IngredientsAndQuantity.Add(new KeyValuePair<string, double>("Tomato puree", 2));
            currentKitchen.AddRecipe(newRecipe);

            Order currentOrder = new Order(currentKitchen);
            List<MealOrder> mealOrders = new List<MealOrder>();

            MealOrder mealOrder = new MealOrder();
            mealOrder.MealName = "SausageStroganoffVeggie";
            mealOrder.NoOfMeals = 0;
            mealOrders.Add(mealOrder);

            List<OrderInfo> result = currentOrder.PlaceOrder(mealOrders);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result[0].NoOfMeals);
            Assert.AreEqual(false, result[0].OkFromKitchen);

        }
    }
}
