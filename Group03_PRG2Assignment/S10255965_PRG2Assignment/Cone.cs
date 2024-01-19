using static System.Formats.Asn1.AsnWriter;

namespace ICTreatsSystem
{
    class Cone : IceCream
    {
        public bool Dipped { get; set; }

        public Cone() { }

        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool input_dipped) : base(o, s, f, t)
        {
            Dipped = input_dipped;   
        }

        public override double CalculatePrice()
        {
            double option_price = 0; //Additional cost for dipped cone
            double scoop_price = 0;
            double flavour_price = 0;
            double topping_price = 0;

            //Calculate price for dipped
            if (Dipped == true)
            {
                option_price = 2;
            }

            //Calculate price for number of scoops
            if (Scoops == 1)
            {
                scoop_price = 4;
            }
            else if (Scoops == 2)
            {
                scoop_price = 5.5;
            }
            else if (Scoops == 3)
            {
                scoop_price = 6.5;
            }

            //Calculate price for flavour
            foreach (Flavour f in Flavours)
            {
                if (f.Premium == true)
                {
                    flavour_price += 2;
                }
            }

            //Calculate price for toppings
            foreach (Topping t in Toppings)
            {
                topping_price += 1;
            }

            //Calculate total price
            double price = option_price + scoop_price + flavour_price + topping_price;
            return price;
        }

        public override string ToString()
        {
            if (Dipped == true) return $"{base.ToString()}, dipped.";
            else return $"{base.ToString()}, non-dipped.";
        }
    }
}
