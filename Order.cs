using System.Diagnostics.Contracts;

namespace ICTreatsSystem
{
    class Order
    {
        public int id { get; set; }
        public DateTime timeReceived { get; set; } = DateTime.Now;
        public DateTime? timeFulfilled { get; set; }
        public List<IceCream> iceCreamList { get; set; } = new List<IceCream>();

        public Order() { }

        public Order(int input_id, DateTime input_timeReceived)
        {
            id = input_id;
            timeReceived = input_timeReceived;
        }

        void ModifyIceCream(int iceCreamPos) { }

        void AddIceCream(IceCream add_IceCream)
        {
            iceCreamList.Add(add_IceCream);
        }

        void DeleteIceCream(int del_IceCream) { }

        double CalculateTotal() { return 0; }

        public override string ToString()
        {
            return $"OrderID: {id} TimeRecieved: {timeReceived} TimeFulfilled: {timeFulfilled}";
        }
    }
}
