namespace ICTreatsSystem
{
    class Flavour
    {
        public string type { get; set; }
        public bool premium { get; set; }
        public int quantity { get; set; }

        public Flavour() { }

        public Flavour(string input_type, bool input_premium, int input_quantity)
        {
            type = input_type;
            premium = input_premium;
            quantity = input_quantity;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Type: {type} Premium: {premium} Quantity: {quantity}";
        }
    }
}
