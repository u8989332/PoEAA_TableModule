using System;
using System.Data;

namespace PoEAA_TableModule
{
    class Contract : TableModule
    {
        public Contract(DataSet ds) : base(ds, "Contracts")
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

        public void CalculateRecognitions(int contractId)
        {
            DataRow contractRow = this[contractId];
            decimal amount = (decimal) contractRow["Revenue"];
            RevenueRecognition rr = new RevenueRecognition(Table.DataSet);
            Product prod = new Product(Table.DataSet);
            int prodId = GetProductId(contractId);

            var productType = prod.GetProductType(prodId);

            if (productType == ProductType.W)
            {
                rr.Insert(contractId, amount, GetWhenSigned(contractId));
            }
            else if (productType == ProductType.S)
            {
                DateTime signedDate = GetWhenSigned(contractId);
                decimal[] allocation = Allocate(amount, 3);
                rr.Insert(contractId, allocation[0], signedDate);
                rr.Insert(contractId, allocation[1], signedDate.AddDays(60));
                rr.Insert(contractId, allocation[2], signedDate.AddDays(90));
            }
            else if (productType == ProductType.D)
            {
                DateTime signedDate = GetWhenSigned(contractId);
                decimal[] allocation = Allocate(amount, 3);
                rr.Insert(contractId, allocation[0], signedDate);
                rr.Insert(contractId, allocation[1], signedDate.AddDays(30));
                rr.Insert(contractId, allocation[2], signedDate.AddDays(60));
            }
        }

        private decimal[] Allocate(decimal amount, int by)
        {
            decimal lowResult = amount / by;
            lowResult = decimal.Round(lowResult, 2);
            decimal highReult = lowResult + 0.01m;
            decimal[] results = new decimal[by];
            int remainder = (int) amount % by;
            for (int i = 0; i < remainder; ++i)
            {
                results[i] = highReult;
            }

            for (int i = remainder; i < by; ++i)
            {
                results[i] = lowResult;
            }

            return results;
        }

        private DateTime GetWhenSigned(int contractId)
        {
            return (DateTime)this[contractId]["DateSigned"];
        }

        private int GetProductId(int contractId)
        {
            return (int)this[contractId]["Product"];
        }
    }
}
