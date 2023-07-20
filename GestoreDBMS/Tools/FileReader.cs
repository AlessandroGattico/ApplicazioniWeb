namespace GestoreDBMS.Tools
{

     //Permette di leggere i dati presenti nel file del path specificato
  
    public class FileReader
    {
        public static string[]? ReadCsv(string path)
        {
            try
            {
                var csv = File.ReadAllLines(path);
                return csv;
            }
            catch (IOException e)
            {
                return null;
            }

        }
    }
}
