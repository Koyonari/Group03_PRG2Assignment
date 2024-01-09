namespace ICTreatsSystem
{
    class Waffle : IceCream
    {
        public string waffleFlavour { get; set; }

        public Waffle() { }

        public Waffle(string o, int s, List<Flavour> f, List<Topping> t, string input_waffleFlavour) : base(o, s, f, t)
        {
            waffleFlavour = input_waffleFlavour;
        }

        public override double CalculatePrice()
        {
            return 0;
        }

        public override string ToString()
        {
            return $"{base.ToString()} WaffleFlavour: {waffleFlavour}";
        }
    }
}
