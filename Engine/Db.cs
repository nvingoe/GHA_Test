using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Fdb.Rx.Platform;

namespace Fdb.Rx.AreasOfInterest
{
    internal static class Db
    {
        internal static SqlMapper.ICustomQueryParameter ToTable(this IEnumerable<Drug> drugs)
        {
            var dt = new DataTable();
            dt.Columns.Add("Code");
            drugs.Select(drug=>drug.ProductCode).Distinct().ForEach(drug => dt.Rows.Add(drug));
            return dt.AsTableValuedParameter();
        }

        internal static SqlMapper.ICustomQueryParameter ToTable(this IEnumerable<ClinicalEntry> clinicalEntries)
        {
            var dt = new DataTable();
            dt.Columns.Add("ClinicalCode");
            clinicalEntries.MostRecent(entry => entry.ClinicalCode, entry => entry.Started, entry => entry.Value)
                .ForEach(clinicalEntry => dt.Rows.Add(clinicalEntry.ClinicalCode));
            return dt.AsTableValuedParameter();
        }
    }

    internal static class Sorting
    {
        public static IEnumerable<TSource> MostRecent<TSource, TKey, TSort, TSort2>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TSort> sortSelector, Func<TSource, TSort2> sortSelector2)
        {
            return source.GroupBy(keySelector, (key, entries) => entries.OrderByDescending(sortSelector).ThenByDescending(sortSelector2).First());
        }
    }
}