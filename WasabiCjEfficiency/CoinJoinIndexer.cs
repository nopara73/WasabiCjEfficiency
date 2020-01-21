using NBitcoin;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasabiCjEfficiency.Helpers;

namespace WasabiCjEfficiency
{
    public class CoinJoinIndexer
    {
        private RPCClient Client { get; }
        private uint256[] coinJoinHashes;

        public CoinJoinIndexer(RPCClient client)
        {
            Client = client;
            coinJoinHashes = File.ReadAllLines("CoinJoinsMain.txt").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new uint256(x)).ToArray();
        }

        private decimal PercentageDone { get; set; } = 0;
        private decimal PreviousPercentageDone { get; set; } = -1;

        public async Task<IEnumerable<CoinJoinDay>> GetDailyStatsAsync()
        {
            Console.WriteLine($"{coinJoinHashes.Length} coinjoins will be analyzed.");

            var days = new List<CoinJoinDay>();
            int processedCoinJoinCount = 0;
            var allInputs = new HashSet<OutPoint>();
            foreach (var batch in coinJoinHashes.Batch(8))
            {
                var txqueries = new List<Task<RawTransactionInfo>>();
                foreach (var cjHash in batch)
                {
                    txqueries.Add(Client.GetRawTransactionInfoWithCacheAsync(cjHash));
                }

                foreach (var query in txqueries)
                {
                    var txi = await query;
                    if (txi.Confirmations == 0) continue;
                    if (!txi.BlockTime.HasValue) continue; // Idk, shouldn't happen.

                    var blockTime = txi.BlockTime.Value;
                    var blockTimeDay = new DateTimeOffset(blockTime.Year, blockTime.Month, blockTime.Day, 0, 0, 0, TimeSpan.Zero);
                    var day = days.FirstOrDefault(x => x.BlockTimeDay == blockTimeDay);
                    if (day is null)
                    {
                        day = new CoinJoinDay(blockTimeDay);
                        days.Add(day);
                    }

                    foreach (var input in txi.Transaction.Inputs.Select(x => x.PrevOut))
                    {
                        allInputs.Add(input);
                        var inputHash = input.Hash;
                        if (IsCoinJoin(inputHash)) continue;

                        var inputTxi = await Client.GetRawTransactionInfoWithCacheAsync(inputHash);

                        day.AddNonMixedInputAmount(inputTxi.Transaction.Outputs[input.N].Value);
                    }

                    decimal coinJoinHashesPer100 = coinJoinHashes.Length / 100m;
                    processedCoinJoinCount++;
                    PercentageDone = processedCoinJoinCount / coinJoinHashesPer100;
                    bool displayProgress = (PercentageDone - PreviousPercentageDone) >= 1;
                    if (displayProgress)
                    {
                        Console.WriteLine($"Progress: {(int)PercentageDone}%");
                        PreviousPercentageDone = PercentageDone;
                    }
                }
            }

            foreach (var cjHash in coinJoinHashes)
            {
                // It should be always getting the tx out of memory cache, so shouldn't be a performance issue.
                var txi = await Client.GetRawTransactionInfoWithCacheAsync(cjHash);
                if (txi.Confirmations == 0) continue;
                if (!txi.BlockTime.HasValue) continue; // Idk, shouldn't happen.

                var blockTime = txi.BlockTime.Value;
                var blockTimeDay = new DateTimeOffset(blockTime.Year, blockTime.Month, blockTime.Day, 0, 0, 0, TimeSpan.Zero);
                var day = days.First(x => x.BlockTimeDay == blockTimeDay);

                var mixedValues = txi.Transaction.GetIndistinguishableOutputs(includeSingle: false).Select(x => x.value);
                var mixedValues2AnonTreshold = txi.Transaction.GetIndistinguishableOutputs(includeSingle: false).Where(x => x.count > 2).Select(x => x.value);
                var mixedValues5AnonTreshold = txi.Transaction.GetIndistinguishableOutputs(includeSingle: false).Where(x => x.count > 5).Select(x => x.value);
                var mixedValues10AnonTreshold = txi.Transaction.GetIndistinguishableOutputs(includeSingle: false).Where(x => x.count > 10).Select(x => x.value);

                TxOutList outputs = txi.Transaction.Outputs;
                for (int i = 0; i < outputs.Count; i++)
                {
                    var output = outputs[i];
                    if (mixedValues10AnonTreshold.Contains(output.Value)) continue; // This is mixed.

                    var outPoint = new OutPoint(cjHash, i);

                    if (allInputs.Contains(outPoint)) continue;

                    var txoutResponse = await Client.GetTxOutAsync(cjHash, i, includeMempool: false);
                    if (txoutResponse is { }) continue; // In this case the tx is unspent so we don't know what will happen with it.

                    day.AddRemixed10AnonAmount(output.Value);
                    if (!mixedValues5AnonTreshold.Contains(output.Value))
                    {
                        day.AddRemixed5AnonAmount(output.Value);
                        if (!mixedValues2AnonTreshold.Contains(output.Value))
                        {
                            day.AddRemixed2AnonAmount(output.Value);
                            if (!mixedValues.Contains(output.Value))
                            {
                                day.AddRemixedChangeAmount(output.Value);

                            }
                        }
                    }
                }
            }

            return days.OrderBy(x => x.BlockTimeDay);
        }

        public bool IsCoinJoin(uint256 txid)
        {
            return coinJoinHashes.Contains(txid);
        }
    }
}
