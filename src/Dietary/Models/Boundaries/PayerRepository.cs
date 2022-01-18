using System.Threading.Tasks;

namespace LegacyFighter.Dietary.Models.Boundaries
{
    public interface IPayerRepository
    {
        Task<Payer> FindByIdAsync(PayerId payerId);
    }
}