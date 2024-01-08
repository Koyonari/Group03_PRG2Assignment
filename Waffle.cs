//==========================================================
// Student Number : S10258126B
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    public class Waffle : IceCream
    {
        public string WaffleFlavour { get; set; }
        public Waffle() { }
        public Waffle(string o, int s, List<Flavour> f, List<Topping> t, string wf)
        {
            Option = o;
            Scoops = s;
            Flavours = f;
            Toppings = t;
            WaffleFlavour = wf;
        }
        public double CalculatePrice()
        {
            //Initialise prices for each component
            double option_price = 0; //Additional cost for dipped cone
            double scoop_price = 0;
            double flavour_price = 0;
            double topping_price = 0;

            //Calculate price for flavour and cone
            option_price += 3; //Additional cost for waffle
            if (WaffleFlavour == "Red Velvet")
            {
                option_price += 3;
            }
            else if (WaffleFlavour == "Charcoal")
            {
                option_price += 3;
            }
            else if (WaffleFlavour == "Pandan Waffle")
            {
                option_price += 3;
            }

            //Calculate price for number of scoops
            if (Scoops == 1)
            {
                scoop_price = 7;
            }
            else if (Scoops == 2)
            {
                scoop_price = 8.5;
            }
            else if (Scoops == 3)
            {
                scoop_price = 9.5;
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
