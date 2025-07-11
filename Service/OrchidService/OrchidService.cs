using Repository.Orchid;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.OrchidService
{
    public class OrchidService : IOrchidService
    {
        private readonly IOrchidRepository _orchidRepository;

        public OrchidService(IOrchidRepository orchidRepository)
        {
            _orchidRepository = orchidRepository;
        }

        public async Task<IEnumerable<OrchidDto>> GetAllOrchidsAsync()
        {
            var orchids = await _orchidRepository.GetAllOrchidsWithCategoriesAsync();

            return orchids.Select(o => new OrchidDto
            {
                Id = o.OrchidId,
                Name = o.OrchidName,
                Description = o.OrchidDescription,
                Price = o.Price,
                CategoryName = o.Category?.CategoryName ?? "Uncategorized",
                ImageUrl = o.OrchidUrl,
                IsNatural = o.IsNatural
            });
        }

        public async Task<OrchidDto> GetOrchidByIdAsync(Guid id)
        {
            var orchid = await _orchidRepository.GetOrchidByIdAsync(id);
            if (orchid == null)
                return null;

            return new OrchidDto
            {
                Id = orchid.OrchidId,
                Name = orchid.OrchidName,
                Description = orchid.OrchidDescription,
                Price = orchid.Price,
                CategoryName = orchid.Category?.CategoryName ?? "Uncategorized",
                ImageUrl = orchid.OrchidUrl,
                IsNatural = orchid.IsNatural
            };
        }

        public async Task<OrchidDto> AddOrchidAsync(CreateOrchidDto orchidDto)
        {
            var orchid = new BusinessObject.Orchid
            {
                OrchidId = Guid.NewGuid(),
                OrchidName = orchidDto.OrchidName,
                OrchidDescription = orchidDto.OrchidDescription,
                OrchidUrl = orchidDto.OrchidUrl,
                Price = orchidDto.Price,
                CategoryId = orchidDto.CategoryId,
                IsNatural = orchidDto.IsNatural
            };

            var success = await _orchidRepository.AddOrchidAsync(orchid);
            if (!success)
                return null;
             
            var addedOrchid = await _orchidRepository.GetOrchidByIdAsync(orchid.OrchidId);

            return new OrchidDto
            {
                Id = addedOrchid.OrchidId,
                Name = addedOrchid.OrchidName,
                Description = addedOrchid.OrchidDescription,
                Price = addedOrchid.Price,
                CategoryName = addedOrchid.Category?.CategoryName ?? "Uncategorized",
                ImageUrl = addedOrchid.OrchidUrl,
                IsNatural = addedOrchid.IsNatural
            };
        }

        public async Task<OrchidDto> UpdateOrchidAsync(Guid id, CreateOrchidDto orchidDto)
        {
            var existingOrchid = await _orchidRepository.GetOrchidByIdAsync(id);
            if (existingOrchid == null)
                return null;
                
            var orchid = new BusinessObject.Orchid
            {
                OrchidId = id,
                OrchidName = orchidDto.OrchidName,
                OrchidDescription = orchidDto.OrchidDescription,
                OrchidUrl = orchidDto.OrchidUrl,
                Price = orchidDto.Price,
                CategoryId = orchidDto.CategoryId,
                IsNatural = orchidDto.IsNatural
            };
            
            var success = await _orchidRepository.UpdateOrchidAsync(orchid);
            if (!success)
                return null;
                
            var updatedOrchid = await _orchidRepository.GetOrchidByIdAsync(id);
            
            return new OrchidDto
            {
                Id = updatedOrchid.OrchidId,
                Name = updatedOrchid.OrchidName,
                Description = updatedOrchid.OrchidDescription,
                Price = updatedOrchid.Price,
                CategoryName = updatedOrchid.Category?.CategoryName ?? "Uncategorized",
                ImageUrl = updatedOrchid.OrchidUrl,
                IsNatural = updatedOrchid.IsNatural
            };
        }
    }
}