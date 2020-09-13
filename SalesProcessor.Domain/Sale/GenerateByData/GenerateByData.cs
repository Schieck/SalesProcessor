namespace SalesProcessor.Domain.Sale
{
    public class GenerateByData
    {
        public static Sale Generate(string[] data){
            try{
                return new Sale(){
                    id=data[1],
                    items=Item.GenerateByData.GenerateList(data[2]),
                    salesManName=data[3]                    
                };
            }catch{
                return null;
            }
        }
    }
}