using DemoRabbitMqCRUD.Application;
using DemoRabbitMqCRUD.Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRabbitMq.Implement
{
    public class Repository : IRepository
    {
        DemoRabbitMqContext context;
        public Repository()
        {
            context = new DemoRabbitMqContext();
        }
        public async Task Create(Product product)
        {
            try
            {
                await SendProductToRabbitMQ(product); // cho product vào hàng đợi 
                await context.products.AddAsync(product);
               await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("lỗi" + ex.Message);
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                Product productDelete = context.products.FirstOrDefault(p => p.Id == id);
                if (productDelete != null)
                {
                    context.products.Remove(productDelete);
                    context.SaveChanges();
                }
                Console.WriteLine("không tìm thấy!");
            }
            catch (Exception ex)
            {
                throw new Exception("lỗi" + ex.Message);
            }
        }

        public async Task<List<Product>> getAll()
        {
            return await context.products.ToListAsync();
        }

        public async Task<Product> getById(Guid id)
        {
            try
            {
                Product product = await context.products.FirstOrDefaultAsync(p => p.Id == id);

                return product;

            }
            catch (Exception ex)
            {

                throw new Exception("lỗi" + ex.Message);
            }
        }

        public async Task Update(Product product)
        {
            try
            {
                Product productUpdate = await context.products.FirstOrDefaultAsync(p => p.Id == product.Id);

                productUpdate.Name = product.Name;
                productUpdate.Description = product.Description;
                context.Update(productUpdate);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("lỗi" + ex.Message);
            }
        }
        private async Task SendProductToRabbitMQ(Product product)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "product_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(product));
                    channel.BasicPublish(exchange: "", routingKey: "product_queue", basicProperties: null, body: body);

                    Console.WriteLine($" [x] Sent {product.Name}");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi thông tin đến RabbitMQ
                Console.WriteLine($"Error sending product to RabbitMQ: {ex.Message}");
            }
        }
    }
}
