namespace GestoreDBMS.Models
{
    public class Table
    {
        public string name { get; set; }
        public LinkedList<Column> columns { get; set; }
        public LinkedList<string> primaryKeysNames { get; set; }
        public LinkedList<Index> indexes { get; set; }
        public LinkedList<ForeignKey> foreignKeys { get; set; }

        public Table()
        {
            this.name = "";
            this.columns = new LinkedList<Column>();
            this.primaryKeysNames = new LinkedList<string>();
            this.indexes = new LinkedList<Index>();
            this.foreignKeys = new LinkedList<ForeignKey>();

        }

        // Metodo "addColumn": aggiunge una nuova colonna alla lista delle colonne
        public void addColumn(string name, string dataType, string isNull)
        {
            columns.AddLast(new Column(name, dataType, isNull));
        }

        // Metodo "addPrimaryKey": aggiunge il nome di una colonna chiave primaria alla lista delle chiavi primarie
        public void addPrimaryKey(string primaryKey)
        {
            primaryKeysNames.AddLast(primaryKey);
        }

        // Metodo "addIndex": aggiunge un nuovo indice alla lista degli indici
        public void addIndex(string name, object columns)
        {
            if (columns is string)
            {
                indexes.AddLast(new Index(name, (string)columns));
            }
        }

        // Metodo "addForeignKey": aggiunge una nuova chiave esterna alla lista delle chiavi esterne
        public void addForeignKey(string tableName, string columnName, string referencedTableName)
        {
            foreignKeys.AddLast(new ForeignKey(tableName, columnName, referencedTableName));
        }

        // Classe "Column": rappresenta una colonna della tabella
        public class Column
        {
            public string name { get; set; }
            public string dataType { get; set; }
            public string isNull { get; set; }

            // Costruttore della classe Column
            public Column(string name, string dataType, string isNull)
            {
                this.name = name;
                this.dataType = dataType;
                this.isNull = isNull;
            }
        }

        // Classe "Index": rappresenta un indice della tabella
        public class Index
        {
            public string name { get; set; }
            public LinkedList<string> columns { get; set; }

            public Index(string name, LinkedList<string> columns)
            {
                this.name = name;
                this.columns = columns;
            }

            public Index(string name, string columns)
            {
                this.name = name;
                this.columns = new LinkedList<string>();

                foreach (string str in columns.Split(","))
                {
                    this.columns.AddLast(str.Trim());
                }
            }
        }

        // Classe "ForeignKey": rappresenta una chiave esterna della tabella
        public class ForeignKey
        {
            public string tableName { get; set; }
            public string columnName { get; set; }
            public string referencedTableName { get; set; }

            public ForeignKey(string tableName, string columnName, string referencedTableName)
            {
                this.tableName = tableName;
                this.columnName = columnName;
                this.referencedTableName = referencedTableName;
            }
        }
    }

}
