﻿//==========================================================
// Student Number : S10255965
// Student Name : Jake Chan Man Lock
// Partner Name : An Yong Shyan
//==========================================================

namespace S10258126_PRG2Assignment
{
    class Customer
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; } = new PointCard();

        public Customer() { }

        public Customer(string input_name, int input_memberId, DateTime input_dob)
        {
            Name = input_name;
            MemberId = input_memberId;
            Dob = input_dob;
        }

        public Order MakeOrder() 
        {
            CurrentOrder = new Order();
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            if (DateTime.Today.AddYears(Dob.Year - DateTime.Today.Year) == Dob) return true;
            return false;
        }

        public override string ToString()
        {
            return $"{Name,-10}{MemberId,-10}{Dob.ToString("dd/MM/yyyy"),-12}{Rewards}";
        }
    }
}
