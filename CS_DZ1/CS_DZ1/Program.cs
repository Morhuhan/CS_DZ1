using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_DZ1
{
    internal class Program
    {
        // Делегат для представления события "Покупка"
        delegate void PurchaseEventHandler(object sender, BuyEventArgs e);

        // Событие "Покупка"
        event PurchaseEventHandler Purchase;

        // Обработчик для события "Покупка"
        static void Handler1(object sender, BuyEventArgs e)
        {
            Console.WriteLine($"{e.customerName} купил {e.productName} за {e.productPrice} рублей.");
            Console.WriteLine($"На прилавке осталось:");

            if (((Seller)sender).store.Products.Count == 0) 
            {
                Console.WriteLine("Ничего");
            }

            foreach (Product p in (((Seller)sender).store).Products)
            {
                Console.WriteLine(p.Name);
            }
        }

        // Класс, содержащий параметры для события "Покупка"
        class BuyEventArgs : EventArgs
        {
            public string customerName;
            public string productName;
            public double productPrice;

            public BuyEventArgs(Customer c, Product p)
            {
                this.customerName = c.Name;
                this.productName = p.Name;
                this.productPrice = p.Price;
            }
        }

        class Store
        {
            public string Name { get; set; }

            public List<Product> Products;

            public Store(string name)
            {
                Name = name;
                Products = new List<Product>();
            }
        }

        // Класс, представляющий товары в магазине
        class Product
        {
            public string Name { get; set; }
            public double Price { get; set; }

            public Product(string name, double price)
            {
                Name = name;
                Price = price;
            }
        }

        // Класс продавца
        class Seller
        {
            public string Name { get; set; }
            public Store store { get; set; }

            public event PurchaseEventHandler onSell;

            public Seller(string name, Store store)
            {
                Name = name;
                onSell += new PurchaseEventHandler(Handler1);
                this.store = store;
            }

            public Product Sell(Order order)
            {
                store.Products.Remove(order.Product);
                onSell(this, new BuyEventArgs(order.Customer, order.Product));
                return order.Product;
            }
        }

        // Класс, представляющий клиентов магазина
        class Customer
        {
            public string Name { get; set; }

            public Customer(string name)
            {
                Name = name;
            }

            public Order MakeOrder(Product p)
            {
                return new Order(this, p);
            }
        }

        // Класс, представляющий заказы в магазине
        class Order
        {
            public Customer Customer { get; set; }
            public Product Product { get; set; }

            public Order(Customer customer, Product product)
            {
                Customer = customer;
                Product = product;
            }
        }

        static void Main(string[] args)
        {
            // Создаем покупателя
            Customer customer = new Customer("Oleg");

            // Создаем магазин, товары и персонал
            Store store = new Store("Пятерочка");

            Product product1 = new Product("Колбаса", 54);
            Product product2 = new Product("Пельмени", 124);
            Product product3 = new Product("Конфеты", 34);

            store.Products.Add(product1);
            store.Products.Add(product2);
            store.Products.Add(product3);

            Seller seller = new Seller("Анатолий", store);

            // Покупатель создает заказ
            Order order1 = customer.MakeOrder(product1);

            Order order2 = customer.MakeOrder(product2);

            Order order3 = customer.MakeOrder(product3);

            // Продовец продает указанный в заказе товар
            seller.Sell(order1);

            seller.Sell(order2);

            seller.Sell(order3);
        }
    }
}
