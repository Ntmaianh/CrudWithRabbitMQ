using DemoRabbitMqCRUD.Application;
using DemoRabbitMqCRUD.Domain;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DemoRabbitMqCRUD.RabbitMq
{
    public class ProductProducter
    {
        private readonly RabbitMQ.Client.IModel _channel;
        public ProductProducter()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

        }
        // Create Product
        public void CreateProduct(Product product)
        {
            // khai báo tên queue => để gọi sang bên nhận 
            _channel.QueueDeclare("create-product-queue", false, false, false, null);
            // khai báo Exchange với kiểu là Direct => dùng loại này để nó chỉ gửi message đến cái queue đó theo routing 
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct);
            // Serialize product object to JSON
            var message = JsonConvert.SerializeObject(product); 
            // Publish message to RabbitMQ
            // tên exchange , routing , property , Message body => content send 
            _channel.BasicPublish("product-exchange", "create-product", null, Encoding.UTF8.GetBytes(message)); 
        }
        // Read Product
        public void GetProductById(Guid id)
        {   
            _channel.QueueDeclare("read-product-queue", false, false, false, null);
            // Publish message to RabbitMQ
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct);
            var message = JsonConvert.SerializeObject(id);
            _channel.BasicPublish("product-exchange", "read-product", null, Encoding.UTF8.GetBytes(message));
        }
        // Update Product
        public void UpdateProduct(Product product)
        {
            _channel.QueueDeclare("update-product-queue", false, false, false, null);
            // Serialize product object to JSON
            var message = JsonConvert.SerializeObject(product);
            // Publish message to RabbitMQ
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct);
            _channel.BasicPublish("product-exchange", "update-product", null, Encoding.UTF8.GetBytes(message));
        }
        // Delete Product
        public void DeleteProduct(Guid id)
        {
            _channel.QueueDeclare("delete-product-queue", false, false, false, null);
            // Publish message to RabbitMQ
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct);
            var message = JsonConvert.SerializeObject(id);
            _channel.BasicPublish("product-exchange", "delete-product", null, Encoding.UTF8.GetBytes(message));

        }
    }
}

