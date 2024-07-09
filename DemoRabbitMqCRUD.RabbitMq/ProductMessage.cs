using DemoRabbitMqCRUD.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoRabbitMqCRUD.RabbitMq
{
    public class ProductMessage
    {
         public string Action { get; set; }
        public Product Product { get; set; }
    }
}

