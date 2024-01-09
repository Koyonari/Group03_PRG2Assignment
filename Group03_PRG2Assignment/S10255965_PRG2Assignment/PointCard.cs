using System.Linq.Expressions;

namespace ICTreatsSystem
{
    class PointCard
    {
        public int points { get; set; } = 0;
        public int punchCard { get; set; } = 0;
        public string tier { get; set; } = "Ordinary";

        public PointCard() { }

        public PointCard(int input_points, int input_punchCard)
        {
            points = input_points;
            punchCard = input_punchCard;
        }

        void AddPoints(int add_points)
        {
            points += add_points;
            if (tier == "Ordinary")
            {
                if (points >= 150) tier = "Gold";
                else if (points >= 50) tier = "Silver";
            }
            else if (tier == "Silver")
            {
                if (points >= 100) tier = "Gold";
            }
        }

        void RedeemPoints(int use_points)//Not Done
        {
            if (points - use_points >= 0 && (tier == "Silver" || tier == "Gold"))
            {
                double cost_offset = use_points * 0.02;
                if (tier == "Gold")
                {

                }
            }
        }

        void punch()//Not Done
        {
            punchCard++;
            if (punchCard == 10)
            {
                punchCard = 0;
            }
        }

        public override string ToString()
        {
            return $"Points: {points} PunchCard: {punchCard} Tier: {tier}";
        }
    }
}
