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
        public Money NonMixedInputSum { get; private set; } = Money.Zero;
        public Money NonRemixedChangeSum { get; private set; } = Money.Zero;
        public Money NonRemixed2AnonSum { get; private set; } = Money.Zero;
        public Money NonRemixed5AnonSum { get; private set; } = Money.Zero;
        public Money NonRemixed10AnonSum { get; private set; } = Money.Zero;

        public CoinJoinDay(DateTimeOffset blockTimeDay)
        {
            BlockTimeDay = Guard.NotNull(nameof(blockTimeDay), blockTimeDay);
        }

        public override string ToString()
        {
            return $"{BlockTimeDay.Year}-{BlockTimeDay.Month}-{BlockTimeDay.Day}\tFresh bitcoins: {(int)NonMixedInputSum.ToDecimal(MoneyUnit.BTC)}\t\tNonremixed change: {(int)NonRemixedChangeSum.ToDecimal(MoneyUnit.BTC)}\t\tNonremixed <2 anons: {(int)NonRemixed2AnonSum.ToDecimal(MoneyUnit.BTC)}\t\tNonremixed <5 anons: {(int)NonRemixed5AnonSum.ToDecimal(MoneyUnit.BTC)}\t\tNonremixed <10 anons: {(int)NonRemixed10AnonSum.ToDecimal(MoneyUnit.BTC)}";
        }
        internal void AddNonMixedInputAmount(Money value)
        {
            NonMixedInputSum += value;
        }

        internal void AddRemixedChangeAmount(Money value)
        {
            NonRemixedChangeSum += value;
        }
        internal void AddRemixed2AnonAmount(Money value)
        {
            NonRemixed2AnonSum += value;
        }
        internal void AddRemixed5AnonAmount(Money value)
        {
            NonRemixed5AnonSum += value;
        }
        internal void AddRemixed10AnonAmount(Money value)
        {
            NonRemixed10AnonSum += value;
        }
    }
}
