using System;
using System.Collections.Generic;
using System.Text;

namespace KetchupApp
{
    public class Order
    {
        private Kitchen _currentKitchen;
       
        public Order()
        {
            this._currentKitchen = new Kitchen();
        }

        public Order(Kitchen currentKitchen)
        {
            this._currentKitchen = currentKitchen;
        }

        public List<string> AvailableMeals()
        {
            List<string> availableMealNames = new List<string>();

           availableMealNames =  _currentKitchen.PossibleMeals();
            return availableMealNames;
        }

        public List<OrderInfo> PlaceOrder(List<MealOrder> mealOrders)
        {
            List<OrderInfo> orders = new List<OrderInfo>();
            OrderInfo order; 
            foreach (var mealOrder in mealOrders)
            {
                order = new OrderInfo();
                order.MealName = mealOrder.MealName;
                order.NoOfMeals = mealOrder.NoOfMeals;
                if (mealOrder.NoOfMeals.Equals(0))
                {
                    order.OkFromKitchen = false;
                }
                else
                {
                    order.OkFromKitchen = _currentKitchen.PrepareMeal(mealOrder.MealName, mealOrder.NoOfMeals);
                }
                orders.Add(order);
            }
            return orders;
        }
    }

    public class OrderInfo
    {

        public string MealName;
        public int NoOfMeals;
        public bool OkFromKitchen;

    }

}
