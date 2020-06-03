using System;
using System.Data;
using System.Linq;
using Fdb.Rx.Persistence.Dapper;

namespace Fdb.Rx.AreasOfInterest
{
    public interface IManageAreasOfInterest
    {
        void SaveProduct(Product product);
        void DeleteProduct(Product product);
        void SaveConditionTerm(ConditionTerm term);
        void DeleteConditionTerm(ConditionTermId id);
        void SaveConditionFilter(Guid id, string name, ConditionTerm term);
        void DeleteConditionFilter(Guid id);
        void Save(Guid id, string name, ConditionTerm term);
        void Delete(Guid id);
    }

    public class Management : IManageAreasOfInterest
    {
        private IDb Db { get; }

        public Management(IDb db)
        {
            Db = db;
        }

        public void SaveProduct(Product product)
        {
            var productId = int.Parse(product.References.Single(pr => pr.Terminology == DrugTerminology.Multilex).Code);
            var group = product.Group != Product.Grouping.Empty ? (long?)product.Group.Id : null;
            var groupName = product.Group != Product.Grouping.Empty ? product.Group.Name : null;
            Db.Execute("SaveProduct", new { id = productId, group, groupName, references = product.References.ToTable()});
        }

        public void DeleteProduct(Product product)
        {
            var productId = int.Parse(product.References.Single(pr => pr.Terminology == DrugTerminology.Multilex).Code);
            Db.Execute("DeleteProduct", new { Id = productId });
        }

        public void SaveConditionTerm(ConditionTerm term)
        {
            Db.Execute("SaveConditionTerm", new { TermId = term.Id, Conditions = term.Conditions.ToTable() });
        }


        public void SaveConditionFilter(Guid id, string name, ConditionTerm term)
        {
            Db.Execute("SaveConditionGroup", new { Id = id, Name = name, TermId = term.Id, Conditions = term.Conditions.ToTable(), IsAreaOfInterest = false, IsFilter = true });
        }

        public void DeleteConditionFilter(Guid id)
        {
            Db.Execute("DeleteConditionGroup", new { Id = id });
        }

        public void Save(Guid id, string name, ConditionTerm term)
        {
            Db.Execute("SaveConditionGroup", new { Id = id, Name = name, TermId = term.Id, Conditions = term.Conditions.ToTable(), IsAreaOfInterest = true, IsFilter = false });
        }

        public void Delete(Guid id)
        {
            Db.Execute("DeleteConditionGroup", new { Id = id });
        }

        public void DeleteConditionTerm(ConditionTermId id)
        {
            Db.Execute("DeleteConditionTerm", new { Term = id });
        }

      
    }
}
