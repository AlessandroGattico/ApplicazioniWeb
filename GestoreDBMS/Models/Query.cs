using static GestoreDBMS.Models.Query;

namespace GestoreDBMS.Models
{
    public class Query
    {
        public Row row { get; set; }
        public LinkedList<Row> rows { get; set; }
        public string status { get; set; }

        public Query()
        {
            this.rows = new LinkedList<Row>();
        }

        // Metodo "addRow": aggiunge una nuova riga alla LinkedList "rows" con l'ID e i valori forniti
        public void addRow(int id, LinkedList<string> list)
        {
            this.rows.AddLast(new Row(id, list));
        }

        // Metodo "addFirstRow": aggiunge una nuova riga in cima alla LinkedList "rows" con l'ID e i valori forniti
        public void addFirstRow(int id, LinkedList<string> list)
        {
            this.rows.AddFirst(new Row(id, list));
        }

        // Metodo "getRowsSize": restituisce il numero di righe presenti nella LinkedList "rows"
        public int getRowsSize()
        {
            return this.rows.Count;
        }

        // Metodo "selectValue": restituisce il valore presente nella colonna indicata per la riga corrente
        public string selectValue(int index)
        {
            return this.row.values.ElementAt(index);
        }

        // Metodo "getColumns": restituisce il numero di colonne presenti nella riga corrente
        public int getColumns()
        {
            return this.rows.First.Value.getSize();
        }

        // Metodo "setCurrentRow": imposta la riga corrente utilizzando l'ID fornito
        public void setCurrentRow(int id)
        {
            foreach (Row row in this.rows)
            {
                if (row.id.Equals(id))
                {
                    this.row = row;
                }
            }
        }

        public class Row
        {
            public int id { get; set; }
            public LinkedList<string> values { get; set; }

            public Row(int id, LinkedList<string> list)
            {
                this.id = id;
                this.values = list;
            }

            // Metodo "getSize": restituisce il numero di valori presenti nella riga
            public int getSize()
            {
                return this.values.Count;
            }
        }
    }
}
