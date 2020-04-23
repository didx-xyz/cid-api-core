using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Brokers
{
    public interface IClickatellBroker
    {
        Task SendSms(object payload);
    }
}