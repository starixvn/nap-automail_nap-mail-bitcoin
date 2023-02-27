using System;

namespace Fanex.Bitcoin.Business.Models
{
    public class BitcoinModel
    {
        public DateTime? Date { get; set; }

        public string GameType { get; set; }

        public string Currency { get; set; }

        public DateTime? ExpiryTime { get; set; }

        public string BetChoice { get; set; }

        public int BetCount { get; set; }

        public decimal TotalBets { get; set; }

        public decimal TotalWinLoss { get; set; }

        public decimal TotalPayout { get; set; }

        public decimal Margin { get; set; }
    }
}