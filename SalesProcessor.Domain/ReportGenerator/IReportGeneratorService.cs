using System.Threading.Tasks;

namespace SalesProcessor.Domain.ReportGenerator
{
    public interface IReportGeneratorService
    {
         Task GenerateReport(Lot.Lot lot);
    }
}