using System;
using System.Data;

namespace PoEAA_TableModule
{
    class RevenueRecognition : TableModule
    {
        private static int _id = 1;
        private static readonly object IdLock = new object();
        public RevenueRecognition(DataSet ds) : base(ds, "RevenueRecognitions")
        {
        }

        public int Insert(int contractId, decimal amount, DateTime date)
        {
            DataRow newRow = Table.NewRow();
            int id = GetNextId();
            newRow["Id"] = id;
            newRow["Contract"] = contractId;
            newRow["Amount"] = amount;
            newRow["RecognizedOn"] = date;
            Table.Rows.Add(newRow);

            return id;
        }

        public decimal RecognizedRevenue(int contractId, DateTime asOf)
        {
            string filter = $"Contract = {contractId} AND RecognizedOn <= #{asOf:d}#";
            DataRow[] rows = Table.Select(filter);
            decimal result = 0m;
            foreach(DataRow row in rows)
            {
                result += (decimal) row["Amount"];
            }

            return result;
        }

        private static int GetNextId()
        {
            lock (IdLock)
            {
                return _id++;
            }
        }
    }
}
