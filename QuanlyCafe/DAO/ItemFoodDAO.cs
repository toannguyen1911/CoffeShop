using QuanlyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuanlyCafe.DAO
{
    class ItemFoodDAO
    {
        private static ItemFoodDAO instance;
        public static ItemFoodDAO Instance
        {
            get { if (instance == null) instance = new ItemFoodDAO(); return ItemFoodDAO.instance; }
            private set { ItemFoodDAO.instance = value; }
        }

        private ItemFoodDAO() { }

        public List<ItemFood> GetListItemFood()
        {
            List<ItemFood> listItemFood = new List<ItemFood>();

            string query = "SELECT f.id, f.name, f.price FROM dbo.Food AS f";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                ItemFood ItemFood = new ItemFood(item);
                listItemFood.Add(ItemFood);
            }
            return listItemFood;
        }
    }
}
