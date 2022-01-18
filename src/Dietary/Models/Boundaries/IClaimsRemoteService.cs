using System.Threading.Tasks;

namespace LegacyFighter.Dietary.Models.Boundaries
{
    public interface IClaimsRemoteService
    {
        Task<bool> ClientHasNoClaimsAsync(PayerId payerId);
    }
}