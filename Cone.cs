//==========================================================
// Student Number : S10258126B
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    public class Cone : IceCream
    {
        public bool Dipped { get; set; }
        public Cone() { }
        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool d)
        {
            Option = o;
            Scoops = s;
            Flavours = f;
            Toppings = t;
            Dipped = d;
        }
        public double CalculatePrice()
        {
            //Initialise prices for each component
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
            string description = $"Ice Cream: {Option} ({Scoops} scoops)\n";
            description += $"Flavors: {string.Join(", ", Flavours.Select(f => f.Type))}\n";
            description += $"Toppings: {string.Join(", ", Toppings.Select(t => t.Type))}\n";
            description += $"Price: ${CalculatePrice():0.00}";
            return description;
        }
    }
}
