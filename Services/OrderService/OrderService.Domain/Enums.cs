namespace OrderService.Domain
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Paid,
        Cancelled,
        Shipped,
        Delivered
    }
}