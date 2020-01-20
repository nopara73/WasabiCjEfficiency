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
            coinJoinHashes = File.ReadAllLines("CoinJoinsMain.txt").Select(x => new uint256(x)).ToArray();
        }

        public async Task<IEnumerable<CoinJoinDay>> GetDailyStatsAsync()
        {
            Console.WriteLine($"{coinJoinHashes.Length} coinjoins will be analyzed.");

            var days = new List<CoinJoinDay>();

            foreach (var cjHash in coinJoinHashes)
            {
                var txi = await Client.GetRawTransactionInfoWithCacheAsync(cjHash);
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
                    var inputHash = input.Hash;
                    if (IsCoinJoin(inputHash)) continue;

                    var inputTxi = await Client.GetRawTransactionInfoWithCacheAsync(inputHash);
                    day.AddNonMixedInputAmount(inputTxi.Transaction.Outputs[input.N].Value);
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
