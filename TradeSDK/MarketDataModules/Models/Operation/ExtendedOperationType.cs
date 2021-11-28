using System.Text.Json.Serialization;

namespace MarketDataModules.Models.Operation
{
    public enum ExtendedOperationType
    {
        Buy,
        BuyCard,
        Sell,
        BrokerCommission,
        ExchangeCommission,
        ServiceCommission,
        MarginCommission,
        OtherCommission,
        PayIn,
        PayOut,
        Tax,
        TaxLucre,
        TaxDividend,
        TaxCoupon,
        TaxBack,
        Repayment,
        PartRepayment,
        Coupon,
        Dividend,
        SecurityIn,
        SecurityOut
    }
}
