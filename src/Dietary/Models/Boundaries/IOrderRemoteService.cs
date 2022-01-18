using System.Collections.Generic;
using System.Threading.Tasks;

namespace LegacyFighter.Dietary.Models.Boundaries
{
    public interface IOrderRemoteService
    {
        Task<List<ClientOrder>> GetByPayerIdAsync(PayerId payerId);
        Task InformAboutNewOrderWithPaymentAsync(decimal? amount);
    }
}