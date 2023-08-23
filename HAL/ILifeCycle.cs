namespace AttendanceService.HAL
{

    public interface ILifeCycle
    {
        Status State { get; }
        bool Connect();
        void Disconnect();

    }
}
