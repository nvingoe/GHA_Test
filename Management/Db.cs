using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Fdb.Rx.AreasOfInterest
{
    internal static class AreasOfInterestDb
    {
        internal static SqlMapper.ICustomQueryParameter ToTable(this IEnumerable<Product.Reference> productReferences)
        {
            var dt = new DataTable();
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("Terminology");
            productReferences.ForEach(r => dt.Rows.Add(r.Code, r.Name, (byte) r.Terminology));
            return dt.AsTableValuedParameter();
        }

        internal static SqlMapper.ICustomQueryParameter ToTable(this IEnumerable<ClinicalCode> clinicalCodes)
        {
            var dt = new DataTable();
            dt.Columns.Add("ClinicalCode");
            dt.Columns.Add("Terminology");
            clinicalCodes.ForEach(clinicalCode => dt.Rows.Add(clinicalCode.Code, (byte)clinicalCode.Terminology));
            return dt.AsTableValuedParameter();
        }
    }
}
