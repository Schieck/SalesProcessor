namespace SalesProcessor.Domain.Customer
{
    public class GenerateByData
    {
        public static Customer Generate(string[] data){
            try{
                return new Customer(){
                    CNPJ = data[1],
                    Name = data[2],
                    BusinessArea = data[3]
                };
            }catch{
                return null;
            }
        }
    }
}