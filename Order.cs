namespace ICTreatsSystem
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

        void DeleteIceCream(int del_IceCream)
        {

        }

        double CalculateTotal() 
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
