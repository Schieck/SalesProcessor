namespace SalesProcessor.Infrastructure.StreamReader
{
    using System.IO;
    public class StreamReader : IStreamReader
    {
        public System.IO.StreamReader GetStreamReader(string path)
        {
            try{
                return new System.IO.StreamReader(path);
            }catch(IOException e){
                throw e;
            }
        }
    }
}