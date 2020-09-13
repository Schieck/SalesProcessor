namespace SalesProcessor.Domain.Salesperson
{
    public class GenerateByData
    {
        public static Salesperson Generate(string[] data){
            try{
                return new Salesperson(){
                    CPF = data[1],
                    Name = data[2],
                    Salary = decimal.Parse(data[3])
                };
            }catch{
                return null;
            }
        }
    }
}