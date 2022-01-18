using System.Threading.Tasks;

namespace LegacyFighter.Dietary.Models.Boundaries
{
    public interface IClientAddressRemoteService
    {
        Task<ClientAddress> GetByPayerIdAsync(PayerId payerId);
    }
}