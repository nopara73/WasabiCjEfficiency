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

        public CoinJoinIndexer(RPCClient client)
        {
            Client = client;
        }

        internal async Task<IEnumerable<CoinJoinDay>> GetDailyStatsAsync()
        {
            var allCoinJoinHashes = (await File.ReadAllLinesAsync("CoinJoinsMain.txt")).Select(x => new uint256(x));



            return new CoinJoinDay[] { };
        }
    }
}
