using System.Collections.Generic;

namespace SalesProcessor.Domain.Sale.Item
{
    public class GenerateByData
    {
        public static List<Item> GenerateList(string itemsList){
            
            List<Item> itemList = new List<Item>();

            itemsList = itemsList.Replace("[", "");
            itemsList = itemsList.Replace("]", "");

            var stringItems = itemsList.Split(","); 
            foreach(string stringItem in stringItems){
                itemList.Add(GenerateItem(stringItem));
            }

            return itemList;
        }

        public static Item GenerateItem(string stringItem){
            var itemProperties = stringItem.Split("-");

            var item = new Item(){
                id = itemProperties[0],
                quantity = int.Parse(itemProperties[1]),
                price = decimal.Parse(itemProperties[2])
            };

            return item;
        }
    }
}