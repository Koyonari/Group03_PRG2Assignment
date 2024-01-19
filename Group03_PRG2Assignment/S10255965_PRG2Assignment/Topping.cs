namespace ICTreatsSystem
{
    class Topping
    {
        public string Type { get; set; }

        public Topping() { }

        public Topping(string input_type)
        {
            Type = input_type;
        }

        public override string ToString()
        {
            return $" + {Type}";
        }
    }
}
