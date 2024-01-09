namespace ICTreatsSystem
{
    class Customer
    {
		public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; } //Not Done
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; } = new PointCard();

        public Customer() { }
       
        public Customer(string input_name, int input_memberId, DateTime input_dob)
        {
            Name = input_name;
            MemberId = input_memberId;
            Dob = input_dob;
        }

        Order MakeOrder()
        {
            OrderHistory.Add(CurrentOrder);
            return CurrentOrder;
        }

        bool IsBirthday()
        {
            if (DateTime.Today ==  Dob) return true;
            return false;
        }

        public override string ToString()
        {
            return $"Name: {Name} MemberID: {MemberId} DateOfBirth: {Convert.ToString(Dob)}";
        }
    }
}
