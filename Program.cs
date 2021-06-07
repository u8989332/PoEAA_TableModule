using System;
using System.Data;

namespace PoEAA_TableModule
{
    class Program
    {
        static void Main(string[] args)
        {
            // mock Result Set
            DataSet ds = new DataSet();

            DataTable productTable = new DataTable("Products");
            productTable.Columns.Add("Id", typeof(int));
            productTable.Columns.Add("Name", typeof(string));
            productTable.Columns.Add("Type", typeof(string));
            productTable.Rows.Add(1, "Code Paradise Database", "D");
            productTable.Rows.Add(2, "Code Paradise Spreadsheet", "S");
            productTable.Rows.Add(3, "Code Paradise Word Processor", "W");

            DataTable contractTable = new DataTable("Contracts");
            contractTable.Columns.Add("Id", typeof(int));
            contractTable.Columns.Add("Product", typeof(int));
            contractTable.Columns.Add("Revenue", typeof(decimal));
            contractTable.Columns.Add("DateSigned", typeof(DateTime));
            contractTable.Rows.Add(1, 1, 9999, new DateTime(2020,1,1));
            contractTable.Rows.Add(2, 2, 1000, new DateTime(2020, 3, 15));
            contractTable.Rows.Add(3, 3, 24000, new DateTime(2020, 7, 25));

            DataTable revenueRecognitionsTable = new DataTable("RevenueRecognitions");
            revenueRecognitionsTable.Columns.Add("Id", typeof(int));
            revenueRecognitionsTable.Columns.Add("Contract", typeof(int));
            revenueRecognitionsTable.Columns.Add("Amount", typeof(decimal));
            revenueRecognitionsTable.Columns.Add("RecognizedOn", typeof(DateTime));

            ds.Tables.Add(productTable);
            ds.Tables.Add(contractTable);
            ds.Tables.Add(revenueRecognitionsTable);

            

            // calculate recognized revenues
            Contract contract = new Contract(ds);

            // database product
            contract.CalculateRecognitions(1);
            var databaseRevenue = new RevenueRecognition(ds).RecognizedRevenue(1, new DateTime(2020, 1, 25));
            Console.WriteLine($"database revenue before 2020-01-25 = {databaseRevenue}");

            // spreadsheet product
            contract.CalculateRecognitions(2);
            var spreadsheetRevenue = new RevenueRecognition(ds).RecognizedRevenue(2, new DateTime(2020, 6, 1));
            Console.WriteLine($"spreadsheet revenue before 2020-06-01 = {spreadsheetRevenue}");

            // word processor product
            contract.CalculateRecognitions(3);
            var wordProcessorRevenue = new RevenueRecognition(ds).RecognizedRevenue(3, new DateTime(2020, 9, 30));
            Console.WriteLine($"word processor revenue before 2020-09-30 = {wordProcessorRevenue}");
        }
    }
}
