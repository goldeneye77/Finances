namespace Umbriel.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public Investor Investor { get; set; }

        public Security Security { get; set; }

        public Account Account { get; set; }

        public int TransactionType { get; set; }

        public string TransactionTypeName
        {
            get
            {
                return this.TransactionType == 1 ? "Buy" : "Sell";
            }
        }

        public System.DateTime TransactionDate { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}