using NUnit.Framework;
using System.Data.SqlClient;
using UltimateDatabaseQueryable;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class WhereDbAccessTests
    {
        [Test]
        public void GetAllTable()
            => DbPreparator.GetRefilledDb(CreateGetAllTable);

        private void CreateGetAllTable(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var paynes = new UltimateQueryable<MajorPayne>(provider)
                .AsEnumerable();
            Assert.AreEqual(paynes.Count(), 4);
            
        }

        [Test]
        public void WhereWithSimplePredicate()
            => DbPreparator.GetRefilledDb(CreateWhereWithSimplePredicate);

        private void CreateWhereWithSimplePredicate(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var paynes = new UltimateQueryable<MajorPayne>(provider)
                .Where(payne => payne.Height > 170)
                .AsEnumerable();

            Assert.AreEqual(paynes.Count(), 2);
        }
    }
}