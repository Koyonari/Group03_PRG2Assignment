namespace ICTreatsSystem
{
    class Topping
    {
        public string type { get; set; }

        public Topping() { }

        public Topping(string input_type)
        {
            type = input_type;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Type: {type}";
        }
    }
}
