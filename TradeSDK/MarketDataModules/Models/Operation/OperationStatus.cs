using System.Text.Json.Serialization;

namespace MarketDataModules.Models.Operation
{
    public enum OperationStatus
    {
        Done,
        Decline,
        Progress
    }
}