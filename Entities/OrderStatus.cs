namespace Entities;

public enum OrderStatus
{
    New,
    CanceledByTheAdministrator,
    PaymentReceived,
    Sent,
    Completed,
    Received,
    CanceledByUser
}