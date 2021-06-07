using System.Data;

namespace PoEAA_TableModule
{
    abstract class TableModule
    {
        protected DataTable Table;

        protected TableModule(DataSet ds, string tableName)
        {
            Table = ds.Tables[tableName];
        }
    }
}
