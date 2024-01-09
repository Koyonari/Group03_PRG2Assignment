namespace ICTreatsSystem
{
    class Cone : IceCream
    {
        public bool dipped { get; set; }

        public Cone() { }

        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool input_dipped) : base(o, s, f, t)
        {
            dipped = input_dipped;   
        }

        public override double CalculatePrice()
        {
            return 0;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Dipped: {dipped}";
        }
    }
}
