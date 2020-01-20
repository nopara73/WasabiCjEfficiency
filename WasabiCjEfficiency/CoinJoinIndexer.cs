using NBitcoin;
using NBitcoin.RPC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal async Task<IEnumerable<CoinJoinDay>> GetDailyStatsAsync()
        {
            Console.WriteLine($"{coinJoinHashes.Length} coinjoins will be analyzed.");

            var days = new List<CoinJoinDay>();
            var daysLock = new object();

            foreach(var cjHash in coinJoinHashes)
            {
                var txi = await Client.GetRawTransactionInfoAsync(cjHash);
                if (txi.Confirmations == 0) continue;


            }

            return days;
        }

        public bool IsCoinJoin(uint256 txid)
        {
            return coinJoinHashes.Contains(txid);
        }
    }
}
