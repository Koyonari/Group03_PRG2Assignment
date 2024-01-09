//==========================================================
// Student Number : S10255965
// Student Name : Jake Chan Man Lock
// Partner Name : An Yong Shyan
//==========================================================

namespace S10258126_PRG2Assignment
{
    class Customer
    {
        public string name { get; set; }
        public int memberId { get; set; }
        public DateTime dob { get; set; }
        public Order currentOrder { get; set; } //Not Done
        public List<Order> orderHistory { get; set; } = new List<Order>();
        public PointCard rewards { get; set; } = new PointCard();

        public Customer() { }

        public Customer(string input_name, int input_memberId, DateTime input_dob)
        {
            name = input_name;
            memberId = input_memberId;
            dob = input_dob;
        }

        Order MakeOrder()
        {
            orderHistory.Add(currentOrder);
            return currentOrder;
        }

        bool IsBirthday()
        {
            if (DateTime.Today == dob) return true;
            return false;
        }

        public override string ToString()
        {
            return $"Name: {name} MemberID: {memberId} DateOfBirth: {Convert.ToString(dob)}";
        }
    }
}
