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

        public void ModifyIceCream(int iceCreamPos) {
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
            return $"OrderID: {Id} TimeRecieved: {TimeReceived} TimeFulfilled: {TimeFulfilled}";
        }
    }
}
