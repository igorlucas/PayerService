using API.Data;
using API.Entities;
using API.Models;

namespace API.Services
{
    public interface ICustomerService : IService<Customer>
    {
        bool CustomerExists(Guid id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository) => _customerRepository = customerRepository;

        public async Task<GenericApiResponseEntityList<Customer>> ReadAllAsync()
        {
            try
            {
                var customers = await _customerRepository.ReadAllAsync();
                var statusCode = 200;
                var message = GlobalUtilsMessages.SuccessApiResponse;
                var response = new GenericApiResponseEntityList<Customer>(statusCode, message, customers);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GenericApiResponseEntity<Customer>> ReadByIdAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.ReadByIdAsync(id);
                var statusCode = customer == null ? 404 : 200;
                var message = GlobalUtilsMessages.SuccessApiResponse;
                var response = new GenericApiResponseEntity<Customer>(statusCode, message, customer);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAsync(Customer customer)
        {
            _customerRepository.Update(customer);
            var updatedRows = await _customerRepository.CommitAsync();
            return updatedRows;
        }

        public async Task<int> CreateAsync(Customer customer)
        {
            _customerRepository.Create(customer);
            var updatedRows = await _customerRepository.CommitAsync();
            return updatedRows;
        }

        public async Task<int> DeleteAsync(Customer customer)
        {
            _customerRepository.Delete(customer);
            var updatedRows = await _customerRepository.CommitAsync();
            return updatedRows;
        }
        public bool CustomerExists(Guid id) => _customerRepository.GetDbSet().Any(customer => customer.Id == id);
    }
}