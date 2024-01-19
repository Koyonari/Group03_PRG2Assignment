namespace ICTreatsSystem
{
    abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; }
        public List<Topping> Toppings { get; set; }

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
            //return $"Option: {Option} Scoops: {Scoops} Flavours: {Flavours} Toppings: {Toppings}";
            switch (Scoops)
            {
                default:
                    return $" Single scoop {Option}";
                case 2:
                    return $" Double scoop {Option}";
                case 3:
                    return $" Triple scoop {Option}";
            }
        }
    }
}
