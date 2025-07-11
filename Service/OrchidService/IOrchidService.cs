
using Service.DTOs;

namespace Service.OrchidService
{
    public interface IOrchidService
    {
        Task<IEnumerable<OrchidDto>> GetAllOrchidsAsync();
        Task<OrchidDto> GetOrchidByIdAsync(Guid id);
        Task<OrchidDto> AddOrchidAsync(CreateOrchidDto orchidDto);
        Task<OrchidDto> UpdateOrchidAsync(Guid id, CreateOrchidDto orchidDto);
    }
}