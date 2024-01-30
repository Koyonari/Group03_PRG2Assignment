//==========================================================
// Student Number : S10258126
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    class Cup : IceCream
    {
        public Cup() { }

        public Cup(string o, int s, List<Flavour> f, List<Topping> t) : base(o, s, f, t) { }

        public override double CalculatePrice()
        {
            double cost = 0;
            //Price of cup Cup, Single 4.00 • Toppings(+$1 each), Double 5.50, Triple 6.50
            switch (base.Scoops)
            {
                case 1:
                    cost = 4.00;
                    break;
                case 2:
                    cost = 5.50;
                    break;
                case 3:
                    cost = 6.50;
                    break;
            }
            foreach (Flavour f in Flavours)
            {
                if (f.Premium == true) cost += 2;
            }
            if (Toppings.Count > 0)
            {
                cost += (Toppings.Count * 1);
            }
            return cost;
        }

        public override string ToString()
        {
            return $"{base.ToString()}.";
        }
    }
}
