public enum MessageQueue
{
    Invalid = 0x00,
    Event = 0x01,
    Control = 0x02,
    Weenie = 0x03,
    Login = 0x04,
    Database = 0x05,
    SecureControl = 0x06,
    SecureWeenie = 0x07, // Autonomous Position
    SecureLogin = 0x08,
    UI = 0x09,
    Smartbox = 0x0A,
    Observer = 0x0B,
    QueueMax = 0x0C
}
