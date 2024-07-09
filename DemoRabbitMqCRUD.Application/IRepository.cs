using DemoRabbitMqCRUD.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRabbitMqCRUD.Application
{
    
    public interface IRepository
    {
        public Task<List<Product>> getAll();
        public Task<Product> getById(Guid id);
        public Task Create(Product product);
        public Task Update(Product product);
        public Task Delete(Guid id);
    }
}
