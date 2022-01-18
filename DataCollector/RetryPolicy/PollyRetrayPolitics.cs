using System;
using System.Threading.Tasks;
using Polly;
using Serilog;

namespace DataCollector.RetryPolicy
{
    static public class PollyRetrayPolitics
    {
        public static Polly.Retry.AsyncRetryPolicy RetryToManyReq()
        {
            Polly.Retry.AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>(ex => ex.Message.Contains("Too many requests"))
                .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(1.5, retryAttempt)),
                (exception, timespan) =>
                {
                    Log.Warning(exception.Message);
                    Log.Warning("Start retray. Timespan = " + timespan);
                });

            return retryPolicy;
        }

        public static Polly.Retry.AsyncRetryPolicy Retry()
        {
            Polly.Retry.AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>()
                .RetryAsync(3, (exception, attemt) =>
                {
                    Log.Error(exception.Message);
                    Log.Error(exception.StackTrace);
                    Log.Warning("Start retray. attemt = " + attemt);
                });

            return retryPolicy;
        }
    }
}

