using API.Data;
using API.Entities;
using API.Models;

namespace API.Services
{
    public interface IAddressService : IService<Address>
    {
        bool AddressExists(Guid id);
    }

    public class AddressService : IAddressService
    {
        private readonly IRepository<Address> _addressRepository;

        public AddressService(IRepository<Address> addressRepository) => _addressRepository = addressRepository;

        public bool AddressExists(Guid id) => _addressRepository.GetDbSet().Any(address => address.Id == id);

        public async Task<int> CreateAsync(Address address)
        {
            _addressRepository.Create(address);
            var updatedRows = await _addressRepository.CommitAsync();
            return updatedRows;
        }
        public async Task<int> UpdateAsync(Address address)
        {
            _addressRepository.Update(address);
            var updatedRows = await _addressRepository.CommitAsync();
            return updatedRows;
        }

        public async Task<int> DeleteAsync(Address address)
        {
            _addressRepository.Delete(address);
            var updatedRows = await _addressRepository.CommitAsync();
            return updatedRows;
        }

        public async Task<GenericApiResponseEntityList<Address>> ReadAllAsync()
        {
            try
            {
                var addresses = await _addressRepository.ReadAllAsync();
                var statusCode = 200;
                var message = GlobalUtilsMessages.SuccessApiResponse;
                var response = new GenericApiResponseEntityList<Address>(statusCode, message, addresses);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GenericApiResponseEntity<Address>> ReadByIdAsync(Guid id)
        {
            try
            {
                var address = await _addressRepository.ReadByIdAsync(id);
                var statusCode = address == null ? 404 : 200;
                var message = GlobalUtilsMessages.SuccessApiResponse;
                var response = new GenericApiResponseEntity<Address>(statusCode, message, address);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}