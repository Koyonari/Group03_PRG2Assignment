namespace ICTreatsSystem
{
    class Flavour
    {
        public string Type { get; set; }
        public bool Premium { get; set; }
        public int Quantity { get; set; }

        public Flavour() { }

        public Flavour(string input_type, bool input_premium, int input_quantity)
        {
            Type = input_type;
            Premium = input_premium;
            Quantity = input_quantity;
        }

        public override string ToString()
        {
            if (Premium == true) return $" {Quantity} Premium {Type}";
            else return $" {Quantity} Ordinary {Type}";
        }
    }
}
