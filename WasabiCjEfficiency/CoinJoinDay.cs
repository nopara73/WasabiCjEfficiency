using NBitcoin;
using System;
using System.Collections.Generic;
using System.Text;
using WasabiCjEfficiency.Helpers;

namespace WasabiCjEfficiency
{
    public class CoinJoinDay
    {
        public DateTimeOffset BlockTimeDay { get; }
        public Money NonMixedInputAmount { get; private set; } = Money.Zero;

        public CoinJoinDay(DateTimeOffset blockTimeDay)
        {
            BlockTimeDay = Guard.NotNull(nameof(blockTimeDay), blockTimeDay);
        }

        public override string ToString()
        {
            return $"{BlockTimeDay.Year}-{BlockTimeDay.Month}-{BlockTimeDay.Day},\tFresh bitcoins: {(int)NonMixedInputAmount.ToDecimal(MoneyUnit.BTC)}";
        }
        internal void AddNonMixedInputAmount(Money value)
        {
            NonMixedInputAmount += value;
        }
    }
}
