namespace Entities;

public enum OrderStatus
{
    New = 0,
    CanceledByTheAdministrator = 1,
    PaymentReceived = 2,
    Sent = 3,
    Completed = 4,
    Received = 5,
    CanceledByUser = 6
}