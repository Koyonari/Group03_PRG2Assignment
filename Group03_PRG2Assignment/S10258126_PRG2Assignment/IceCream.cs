//==========================================================
// Student Number : S10258126
// Student Name : An Yong Shyan
// Partner Name : Jake Chan Man Lock
//==========================================================

namespace S10258126_PRG2Assignment
{
    abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; } = new List<Flavour>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();
        public IceCream() { }
        public IceCream(string o, int s, List<Flavour> f, List<Topping> t)
        {
            Option = o;
            Scoops = s;
            Flavours = f;
            Toppings = t;
        }
        public abstract double CalculatePrice();
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
