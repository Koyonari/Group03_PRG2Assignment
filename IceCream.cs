namespace ICTreatsSystem
{
    abstract class IceCream
    {
        public string option { get; set; }
        public int scoops { get; set; }
        public List<Flavour> flavours { get; set; } = new List<Flavour>();
        public List<Topping> toppings { get; set; } = new List<Topping>();

        public IceCream() { }

        public IceCream(string input_option, int input_scoops, List<Flavour> input_flavours, List<Topping> input_toppings)
        {
            option = input_option;
            scoops = input_scoops;
            flavours = input_flavours;
            toppings = input_toppings;
        }

        public abstract double CalculatePrice();

        public override string ToString()
        {
            return $"Option: {option} Scoops: {scoops}";
        }
    }
}
