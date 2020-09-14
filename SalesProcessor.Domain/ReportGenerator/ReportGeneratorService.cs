using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesProcessor.Infrastructure.FileGenerator;

namespace SalesProcessor.Domain.ReportGenerator
{
    public class ReportGeneratorService : IReportGeneratorService
    {

        IFileGenerator _fileGenerator; 

        public ReportGeneratorService(IFileGenerator fileGenerator){
            _fileGenerator = fileGenerator;
        }

        public async Task GenerateReport(Lot.Lot lot)
        {
            try{
                var content = new StringBuilder();
                // Get the customers quantity in the lot
                content.AppendLine(lot.customers.Count + ";");

                //Get the salespersons quantity in the lot
                content.AppendLine(lot.salespersons.Count + ";");
               
                if(lot.sales.Count>0){
                    //Get the mostexpansive sale descending by the sum of the prices x quantity to get the total of each sale, getting the most expansive one
                    var mostExpansiveSale = lot.sales.OrderByDescending(x => x.items.Sum(i => i.price*i.quantity)).Take(1).First();
                    content.AppendLine(mostExpansiveSale.id + ";");

                    //Calculate the results to get the salesperson with minimun results
                    decimal worstResult = 0M;
                    Salesperson.Salesperson worstSalesPerson = null;
                    foreach(Salesperson.Salesperson salesperson in lot.salespersons){
                        var salespersonSales = lot.sales.FindAll(sale => sale.salesManName == salesperson.Name);
                        var salespersonResult = salespersonSales.Sum(sale => sale.items.Sum(i => i.price*i.quantity));

                        if(worstSalesPerson == null || salespersonResult < worstResult){
                            worstResult = salespersonResult;
                            worstSalesPerson = salesperson;
                        }
                    }
                    content.AppendLine(worstSalesPerson.Name + ";" + worstSalesPerson.CPF + ";" + worstSalesPerson.Salary + ";");
                }

                await _fileGenerator.GenerateFile(lot.name, content.ToString());
            }catch(System.Exception e){
                throw e;
            }
        }
    }
}