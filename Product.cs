using System;
using System.Data;

namespace PoEAA_TableModule
{
    public enum ProductType
    {
        W,
        S,
        D
    };

    class Product : TableModule
    {
        public Product(DataSet tableDataSet) : base(tableDataSet, "Products")
        {
        }

        public DataRow this[int key]
        {
            get
            {
                string filter = $"Id = {key}";
                return Table.Select(filter)[0];
            }
        }

        public ProductType GetProductType(int prodId)
        {
            string typeCode = (string) this[prodId]["Type"];
            return (ProductType) Enum.Parse(typeof(ProductType), typeCode);
        }
    }
}