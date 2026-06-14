public class CartService : ICartService
{
    private const decimal ShippingFee = 15.00m;
    private const decimal FreeShippingThreshold = 150.00m;

    public CartResult CalculateTotal(ShoppingCart cart)
    {
        if (cart == null || !cart.Items.Any())
        {
            return new CartResult { TotalPrice = 0, ShippingCost = 0 };
        }

        // 1. Calculate Subtotal (Sum of Price * Quantity)
        decimal subTotal = cart.Items.Sum(item => item.Price * item.Quantity);

        // 2. Apply Dynamic Coupon Discount if available
        decimal discount = 0;
        if (cart.HasActiveCoupon)
        {
            discount = subTotal * (cart.CouponDiscountPercentage / 100m);
        }

        // 3. Determine Shipping Cost based on the threshold
        decimal shippingCost = (subTotal - discount) >= FreeShippingThreshold ? 0 : ShippingFee;

        // 4. Calculate Final Total
        decimal finalTotal = (subTotal - discount) + shippingCost;

        return new CartResult
        {
            SubTotal = subTotal,
            DiscountAmount = discount,
            ShippingCost = shippingCost,
            TotalPrice = finalTotal
        };
    }
}
