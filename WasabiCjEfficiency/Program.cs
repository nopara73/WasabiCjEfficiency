using NBitcoin;
using NBitcoin.RPC;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WasabiCjEfficiency.Helpers;

namespace WasabiCjEfficiency
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Logger.InitializeDefaults();

            ParseArgs(args, out NetworkCredential rpcCred);

            var rpcConf = new RPCCredentialString
            {
                UserPassword = rpcCred
            };
            var client = new RPCClient(rpcConf, Network.Main);

            File.ReadAllLines("CoinJoinsMain.txt");

            using (BenchmarkLogger.Measure(operationName: "Parsing The Blockchain"))
            {
                var cjIndexer = new CoinJoinIndexer(client);
                var days = await cjIndexer.GetDailyStatsAsync();

                foreach (var month in days.GroupBy(x => new DateTimeOffset(x.BlockTimeDay.Year, x.BlockTimeDay.Month, 1, 0, 0, 0, TimeSpan.Zero)).OrderBy(x => x.Key))
                {
                    Console.WriteLine($"{month.Key.Year}-{month.Key.Month}\tFresh bitcoins: {(int)Money.Satoshis(month.Sum(x => x.NonMixedInputSum)).ToDecimal(MoneyUnit.BTC)},\tNonremixed change: {(int)Money.Satoshis(month.Sum(x => x.NonRemixedChangeSum)).ToDecimal(MoneyUnit.BTC)}\t\tNonremixed <2 anons: {(int)Money.Satoshis(month.Sum(x => x.NonRemixed2AnonSum)).ToDecimal(MoneyUnit.BTC)}\t\tNonremixed <5 anons: {(int)Money.Satoshis(month.Sum(x => x.NonRemixed5AnonSum)).ToDecimal(MoneyUnit.BTC)}\t\tNonremixed <10 anons: {(int)Money.Satoshis(month.Sum(x => x.NonRemixed10AnonSum)).ToDecimal(MoneyUnit.BTC)}");
                }

                await File.WriteAllLinesAsync("DailyStats.txt", days.Select(x => x.ToString()));
            }

            Console.WriteLine();
            Console.WriteLine("Press a button to exit...");
            Console.ReadKey();
        }

        private static void ParseArgs(string[] args, out NetworkCredential cred)
        {
            string rpcUser = null;
            string rpcPassword = null;

            var rpcUserArg = "--rpcuser=";
            var rpcPasswordArg = "--rpcpassword=";
            foreach (var arg in args)
            {
                var idx = arg.IndexOf(rpcUserArg, StringComparison.Ordinal);
                if (idx == 0)
                {
                    rpcUser = arg.Substring(idx + rpcUserArg.Length);
                }

                idx = arg.IndexOf(rpcPasswordArg, StringComparison.Ordinal);
                if (idx == 0)
                {
                    rpcPassword = arg.Substring(idx + rpcPasswordArg.Length);
                }
            }

            cred = new NetworkCredential(rpcUser, rpcPassword);
        }
    }
}
