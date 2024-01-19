using System;

namespace ICTreatsSystem
{ 
    class Cup : IceCream
    {
        public Cup() { }

        public Cup(string o, int s, List<Flavour> f, List<Topping> t) : base(o, s, f, t) { }

        public override double CalculatePrice()
        {
            double cost = 0;
            //Price of cup Cup, Single 4.00 • Toppings(+$1 each), Double 5.50, Triple 6.50
            if (base.Toppings.Count > 0)
            {
                cost += cost + (base.Toppings.Count() * 1);
            }
            foreach (Flavour f in base.Flavours)
            {
                if (f.Premium == true) cost += 2;
            }
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
            return cost;
        }

        public override string ToString()
        {
            return $"{base.ToString()}.";
        }
    }
}
