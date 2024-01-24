//==========================================================
// Student Number : S10255965
// Student Name : Chan Man Lock Jake
// Partner Name : An Yong Shyan
//==========================================================
namespace ICTreatsSystem
{
    class Customer
    {
	    public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; } //Not Done
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; }

        public Customer() { }
       
        public Customer(string input_name, int input_memberId, DateTime input_dob)
        {
            Name = input_name;
            MemberId = input_memberId;
            Dob = input_dob;
        }

        public Order MakeOrder()
        {
            CurrentOrder = new Order(Convert.ToInt32(DateTime.Now.ToString("HHss")),DateTime.Now);
            OrderHistory.Add(CurrentOrder);
            return CurrentOrder;
        }

        public bool IsBirthday()
        {
            if (DateTime.Today ==  Dob) return true;
            return false;
        }

        public override string ToString()
        {
            return $"{Name,-10}{MemberId,-10}{Dob.ToString("dd/MM/yyyy"),-12}{Rewards}";
        }
    }
}
