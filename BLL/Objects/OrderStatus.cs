namespace BLL.Objects;

public enum OrderStatus
{
    New = 0,
    PaymentReceived = 1,
    Sent = 2,
    Received = 3,
    Completed = 4,
    CanceledByUser = 5,
    CanceledByTheAdministrator = 6
}