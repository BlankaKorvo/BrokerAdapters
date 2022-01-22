using DataCollector.RetryPolicy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapter
{
    public class GetTinkoffOperationsHistory
    {
        private readonly string figi;
        private readonly DateTime dateFrom;
        private readonly DateTime dateTo;

        public GetTinkoffOperationsHistory(string figi, DateTime dateFrom, DateTime dateTo)
        {
            this.figi = figi;
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
        }
        internal async Task<List<Operation>> GetOperations()
        {
            List<Operation> operations = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.OperationsAsync(dateFrom, dateTo, figi)));
            return operations;
        }
    }
}
