﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuanlyCafe.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? dateCheckin, DateTime? dateCheckout, int status, int discount)
        {
            this.ID = id;
            this.dateCheckIn = dateCheckin;
            this.dateCheckOut = dateCheckout;
            this.Status = status;
            this.Discount = discount;
        }

        public Bill(DataRow row)
        {
            this.ID = (int)row["id"];
            this.dateCheckIn = (DateTime?)row["dateCheckin"];
            var dateCheckOutTemp = row["dateCheckOut"];
            if(dateCheckOutTemp.ToString() != "")
                this.dateCheckOut = (DateTime?)dateCheckOutTemp;
            this.Status = (int)row["status"]; 
        }
        private int discount;
        public int Discount 
        { 
            get => discount;
            set => discount = value; 
        }
        private int status;
        public int Status 
        { 
            get => status;
            set => status = value;
        }

        private DateTime? dateCheckOut;
        public DateTime? DateCheckOut 
        { 
            get => dateCheckOut; 
            set => dateCheckOut = value;
        }

        private DateTime? dateCheckIn;
        public DateTime? DateCheckIn 
        { 
            get => dateCheckIn;
            set => dateCheckIn = value; 
        }

        private int iD;
        public int ID 
        { 
            get => iD; 
            set => iD = value; 
        }
    }
}
