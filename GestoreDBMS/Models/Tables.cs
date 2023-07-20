namespace GestoreDBMS.Models
{

    // Contiene tutte le tabelle del DBMS
    public class Tables
    {
        public LinkedList<Table> tables { get; set; }

        public Tables()
        {
            this.tables = new LinkedList<Table>();
        }

        public void addTable(Table table)
        {
            this.tables.AddLast(table);
        }
    }
}
