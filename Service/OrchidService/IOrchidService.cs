using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.OrchidService
{
    public interface IOrchidService
    {
        Task<IEnumerable<OrchidDto>> GetAllOrchidsAsync();
        Task<OrchidDto> GetOrchidByIdAsync(Guid id);
        Task<OrchidDto> AddOrchidAsync(CreateOrchidDto orchidDto);
        Task<OrchidDto> UpdateOrchidAsync(Guid id, CreateOrchidDto orchidDto);
        Task<IEnumerable<OrchidDto>> SearchOrchidsAsync(OrchidSearchDto searchDto);
    }
}