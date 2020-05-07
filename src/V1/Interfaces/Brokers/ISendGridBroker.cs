using System.Threading.Tasks;

namespace CoviIDApiCore.V1.Interfaces.Brokers
{
    public interface ISendGridBroker
    {
        Task SendEmail(object payload);
    }
}