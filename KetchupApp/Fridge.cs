using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KetchupApp
{
    public class Fridge
    {

        public List<FridgeInventory> InventoryList = new List<FridgeInventory>();

        public bool IsItemAvailable(string ingredient, double quantity)
        {
            return InventoryList.Find(x => x.Name == ingredient && x.Quantity >= quantity) != null;
        }

        public FridgeInventory GetInventoryItem(string ingredient)
        {
          return InventoryList.Find(x => x.Name == ingredient);
        }

        public void AddIngredientToFridge(string inventoryName, double quantity)
        {
            var existingInventoryItem = GetInventoryItem(inventoryName);

            if (existingInventoryItem != null)
            {
                existingInventoryItem.Quantity += quantity;
                return;
            }

            InventoryList.Add(new FridgeInventory(inventoryName, quantity));

        }

        public double TakeItemFromFridge(string inventoryName, double quantity)
        {
            var existingInventoryItem = GetInventoryItem(inventoryName);

            if (existingInventoryItem != null)
            {
                if (existingInventoryItem.Quantity < quantity)
                {
                    return existingInventoryItem.Quantity - quantity;
                }

                existingInventoryItem.Quantity -= quantity;
                return existingInventoryItem.Quantity;
            }
            return -1 * quantity;
        }
    }

    public class FridgeInventory
    {
        public string Name;
        public double Quantity;

        public FridgeInventory(string name, double quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }

}
