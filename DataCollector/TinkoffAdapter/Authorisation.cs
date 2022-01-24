using Serilog;
using System.IO;
using Tinkoff.Trading.OpenApi.Network;

namespace DataCollector.TinkoffAdapter
{
    internal static class Authorisation
    {
        static Context _context;

        /// <summary>
        /// Singltone передача токена.
        /// </summary>
        public static Context Context
        {
            get
            {
                if (_context == null)
                {
                    _context = GetSanboxContext();
                    //_context = GetContext();
                }
                return _context;
            }
        }
        /// <summary>
        /// Получение токена для работы в песочнице
        /// </summary>
        /// <returns></returns>
        static Context GetSanboxContext()
        {
            Log.Information("Start GetSanboxContext");
            string token = File.ReadAllLines("toksan.dll")[0].Trim();
            var connection = ConnectionFactory.GetSandboxConnection(token);
            var context = connection.Context;
            Log.Information("Stop GetSanboxContext");
            return context;
        }

        /// <summary>
        /// Получение токена на реальной бирже
        /// </summary>
        /// <returns></returns>
        static Context GetContext()
        {
            Log.Information("Start GetContext");
            string token = File.ReadAllLines("tokst.dll")[0].Trim();
            var connection = ConnectionFactory.GetConnection(token);
            var context = connection.Context;
            Log.Information("Stop GetContext");
            return context;
        }
        //public static void RemoveInstance()
        //{
        //    _context = null;
        //}
    }
}

