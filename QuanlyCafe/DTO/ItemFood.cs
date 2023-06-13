using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuanlyCafe.DTO
{
    class ItemFood
    {
        public ItemFood(int id, string foodName, float price)
        {
            this.Id = id;
            this.FoodName = foodName;
            this.Price = price;
        }

        public ItemFood(DataRow row)
        {
            this.Id = (int)row["id"];
            this.FoodName = row["Name"].ToString();
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }

        private float price;
        public float Price { get => price; set => price = value; }
        private int id;
        public int Id { get => id; set => id = value; }
        private string foodName;
        public string FoodName { get => foodName; set => foodName = value; }
    }
}
