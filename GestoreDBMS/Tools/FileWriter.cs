using System.IO;

namespace GestoreDBMS.Tools
{
    //Permette di creare o sovrascrivere un file inserendoci i dati in formato csv
 
    public static class FileWriter
    {
        public static bool WriteCsv (string path, string tableName, string csv)
        {
            try
            {
                File.WriteAllText($"{path}/{tableName}.csv", csv);
                return true;
            }
            catch (IOException e)
            {
                return false;
            }
            
        }
    }
}
