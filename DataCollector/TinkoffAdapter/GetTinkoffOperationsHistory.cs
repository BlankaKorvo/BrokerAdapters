using DataCollector.RetryPolicy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapter
{
    internal class GetTinkoffOperationsHistory
    {
        private readonly string figi;
        private readonly DateTime dateFrom;
        private readonly DateTime dateTo;

        /// <summary>
        /// Получение списка операций инструмента(figi) с dateFrom по dateTo
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="dateFrom"></param> Начальная дата периода
        /// <param name="dateTo"></param> Конечная дата периода
        public GetTinkoffOperationsHistory(string figi, DateTime dateFrom, DateTime dateTo)
        {
            this.figi = figi;
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
        }

        /// <summary>
        /// Получение списка операций.
        /// </summary>
        /// <returns></returns>
        internal async Task<List<Operation>> GetOperations()
        {
            List<Operation> operations = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.OperationsAsync(dateFrom, dateTo, figi)));
            return operations;
        }
    }
}
