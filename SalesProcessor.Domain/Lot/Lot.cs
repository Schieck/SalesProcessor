using SalesProcessor.Infrastructure.Configuration.Lot;
using SalesProcessor.Domain.Customer;
using Serilog;
using System.Collections.Generic;

namespace SalesProcessor.Domain.Lot
{
    public class Lot
    {
        private LotSettings _configuration;

        public string name;

        public List<Customer.Customer> customers;

        public List<Sale.Sale> sales;

        public List<Salesperson.Salesperson> salespersons;
        
        public Lot(LotSettings configuration, string fileName){
            _configuration = configuration;
            customers = new List<Customer.Customer>();
            sales = new List<Sale.Sale>();
            salespersons = new List<Salesperson.Salesperson>();
            name = fileName;
        }

        public void AddData(string line)
        {
            var data = line.Split(_configuration.recordSeparator);
            
            //Classify Data
            switch (data[0])
            {
                case "001":
                    var salesPerson = Salesperson.GenerateByData.Generate(data);
                    AddSalesPerson(salesPerson);
                    break;
                case "002":
                    var customer = Customer.GenerateByData.Generate(data);
                    AddCustomer(customer);
                    break;
                case "003":
                    var sale = Sale.GenerateByData.Generate(data);
                    AddSale(sale);
                    break;
                default:
                    throw new System.Exception("Id " + line[0] + " not recognized as a valid code and will not be inserted in the report.");
            }            
        }

        public void AddSale(Sale.Sale sale)
        {
             if(sale != null)
                this.sales.Add(sale);
            else
                throw new System.Exception("This is not a valid sale!");
        }

        public void AddSalesPerson(Salesperson.Salesperson salesperson)
        {
            if(salesperson != null)
                this.salespersons.Add(salesperson);
            else
                throw new System.Exception("This is not a valid SalesPerson!");
        }

        public void AddCustomer(Customer.Customer customer)
        {
            if(customer != null)
                this.customers.Add(customer);
            else
                throw new System.Exception("This is not a valid customer!");
        }
    }
}