using System.Collections.Generic;

namespace SalesProcessor.Domain.Sale
{
    public class Sale
    {
        public string id {get; set;}

        public string salesManName {get; set;}

        public List<Item.Item> items {get; set;} 
                
    }
}