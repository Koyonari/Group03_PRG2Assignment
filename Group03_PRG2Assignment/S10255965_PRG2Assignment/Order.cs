﻿//==========================================================
// Student Number : S10255965
// Student Name : Chan Man Lock Jake
// Partner Name : An Yong Shyan
//==========================================================
namespace ICTreatsSystem
{
    class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFulfilled { get; set; } = null;
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

        public Order() { }

        public Order(int input_id, DateTime input_timeReceived)
        {
            Id = input_id;
            TimeReceived = input_timeReceived;
        }

        public void ModifyIceCream(int iceCreamPos) //Function Not Used
        {
            IceCream modify_IceCream = IceCreamList[iceCreamPos];
        }

        public void AddIceCream(IceCream add_IceCream)
        {
            IceCreamList.Add(add_IceCream);
        }

        public void DeleteIceCream(int del_IceCream)
        {
            IceCreamList.RemoveAt(del_IceCream);
        }

        public double CalculateTotal() 
        {
            double total = 0;
            for (int i = 0; i < IceCreamList.Count; i++)
            {
                total += IceCreamList[i].CalculatePrice();
            }
            return total; 
        }

        public override string ToString()
        {
            return $"OrderID: {Id}\n    TimeReceived: {TimeReceived}\n    TimeFulfilled: {TimeFulfilled}";
        }
    }
}
