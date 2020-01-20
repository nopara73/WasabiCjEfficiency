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

                foreach (var day in days)
                {
                    Console.WriteLine(day);
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
