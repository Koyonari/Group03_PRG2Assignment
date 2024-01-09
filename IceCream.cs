namespace ICTreatsSystem
{
    abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; } = new List<Flavour>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();

        public IceCream() { }

        public IceCream(string input_option, int input_scoops, List<Flavour> input_flavours, List<Topping> input_toppings)
        {
            Option = input_option;
            Scoops = input_scoops;
            Flavours = input_flavours;
            Toppings = input_toppings;
        }

        public abstract double CalculatePrice();

        public override string ToString()
        {
            return $"Option: {Option} Scoops: {Scoops}";
        }
    }
}
