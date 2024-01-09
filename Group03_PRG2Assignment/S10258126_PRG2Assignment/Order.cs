//==========================================================
// Student Number : S10255965
// Student Name : Jake Chan Man Lock
// Partner Name : An Yong Shyan
//==========================================================

namespace S10258126_PRG2Assignment
{
    class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; } = DateTime.Now;
        public DateTime? TimeFulfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();

        public Order() { }

        public Order(int input_id, DateTime input_timeReceived)
        {
            Id = input_id;
            TimeReceived = input_timeReceived;
        }

        void ModifyIceCream(int iceCreamPos) { }

        void AddIceCream(IceCream add_IceCream)
        {
            IceCreamList.Add(add_IceCream);
        }

        void DeleteIceCream(int del_IceCream) { }

        double CalculateTotal() { return 0; }

        public override string ToString()
        {
            return $"OrderID: {Id} TimeRecieved: {TimeReceived} TimeFulfilled: {TimeFulfilled}";
        }
    }
}
