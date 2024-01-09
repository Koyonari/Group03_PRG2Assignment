namespace ICTreatsSystem
{ 
    class Cup : IceCream
    {
        public Cup() { }

        public Cup(string o, int s, List<Flavour> f, List<Topping> t) : base(o, s, f, t) { }

        public override double CalculatePrice()
        {
            return 0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
