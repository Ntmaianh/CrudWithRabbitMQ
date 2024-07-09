using DemoRabbitMqCRUD.Application;
using DemoRabbitMqCRUD.Domain;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRabbitMqCRUD.RabbitMq
{
    public class ProductConsumer
    {
        private readonly RabbitMQ.Client.IModel _channel;
        private readonly IRepository _repository;

        public ProductConsumer(IRepository repository)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _repository = repository;
        }

        public void ConsumeCreateProductMessage()
        {
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct);
            _channel.QueueDeclare("create-product-queue", false, false, false, null);
            _channel.QueueBind("create-product-queue", "product-exchange", "create-product");

            // Tạo consumer và bắt đầu consume
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var product = JsonConvert.DeserializeObject<Product>(message);

                // gửi xuống repo để xử lí 
                _repository.Create(product);

                // Báo với RabbitMQ là đã xử lý xong thông điệp
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("create-product-queue", false, consumer);
        }
        public void ConsumeGetByIdProductMessage()
        {
            // Khai báo exchange và queue
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct); // khai báo exchange
            _channel.QueueDeclare("read-product-queue", false, false, false, null); // khai báo queue 
            _channel.QueueBind("read-product-queue", "product-exchange", "read-product");     // Các consumers đang consume từ "create-product-queue" sẽ nhận được thông điệp này.
                                  // tên  queue      , tên exchange   , tên routing 

            // Tạo consumer và bắt đầu consume
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var productById = JsonConvert.DeserializeObject<Guid>(message);

                // gửi xuống repo để xử lí 
                _repository.getById(productById);

                // Báo với RabbitMQ là đã xử lý xong thông điệp
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("read-product-queue", false, consumer);
        }

        public void ConsumeUpdateProductMessage()
        {
            // Khai báo exchange và queue
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct); // khai báo exchange
            _channel.QueueDeclare("update-product-queue", false, false, false, null); // khai báo queue 
            _channel.QueueBind("update-product-queue", "product-exchange", "update-product");     // Các consumers đang consume từ "create-product-queue" sẽ nhận được thông điệp này.
                                    // tên  queue      , tên exchange   , tên routing 

            // Tạo consumer và bắt đầu consume
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var product = JsonConvert.DeserializeObject<Product>(message);

                // gửi xuống repo để xử lí 
                _repository.Update(product);

                // Báo với RabbitMQ là đã xử lý xong thông điệp
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("update-product-queue", false, consumer);
        }

        public void ConsumeDeleteProductMessage()
        {
            // Khai báo exchange và queue
            _channel.ExchangeDeclare("product-exchange", ExchangeType.Direct); // khai báo exchange
            _channel.QueueDeclare("delete-product-queue", false, false, false, null); // khai báo queue 
            _channel.QueueBind("delete-product-queue", "product-exchange", "delete-product");     // Các consumers đang consume từ "create-product-queue" sẽ nhận được thông điệp này.
                                // tên  queue      , tên exchange   , tên routing 
            // Tạo consumer và bắt đầu consume
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var productDelete = JsonConvert.DeserializeObject<Guid>(message);
                // gửi xuống repo để xử lí 
                _repository.Delete(productDelete);
                // Báo với RabbitMQ là đã xử lý xong thông điệp
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("delete-product-queue", false, consumer);
        }


    }
}
