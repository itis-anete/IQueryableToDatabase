using NUnit.Framework;
using UltimateDatabaseQueryable;
using System.Data.SqlClient;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class TreeTranslatorTests
    {
        [Test]
        public void InitialisesWithSelectFrom()
        {
            DbPreparator.GetRefilledDb(CreateEmptyQuery);
        }

        [Test]
        public void SimpleWhereEqualsRequest()
        {
            DbPreparator.GetRefilledDb(CreateSimpleWhereEqualsQuery);
        }

        [Test]
        public void ComplexWhereRequest()
        {
            DbPreparator.GetRefilledDb(CreateComplexWhereRequest);
        }

        [Test]
        public void WhereWithCapturedVariable()
        {
            DbPreparator.GetRefilledDb(CreateWhereWithCapturedVariable);
        }

        [Test]
        public void WhereWithMemberAccess()
        {
            DbPreparator.GetRefilledDb(CreateWhereWithMemberAccess);
        }

        private void CreateEmptyQuery(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var bobas = new UltimateQueryable<Boba>(provider);
            Assert.AreEqual(bobas.ToString(), "select * from Boba");
        }

        private void CreateSimpleWhereEqualsQuery(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var bobas = new UltimateQueryable<Boba>(provider)
                .Where(boba => boba.Feet == false);
            Assert.AreEqual(
                bobas.ToString(), 
                "select * from (select * from Boba) as T where (Feet = 0)");
        }

        private void CreateComplexWhereRequest(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var paynes = new UltimateQueryable<MajorPayne>(provider)
                .Where(payne => payne.Height >= 170 && payne.Song == "AAAA");
            Assert.AreEqual(
                paynes.ToString(),
                "select * from (select * from MajorPayne) as T where ((Height >= 170) and (Song = 'AAAA'))");
        }

        private void CreateWhereWithCapturedVariable(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var song = "BBBB";
            var paynes = new UltimateQueryable<MajorPayne>(provider)
                .Where(payne => payne.Song == song);
            Assert.AreEqual(
                paynes.ToString(),
                "select * from (select * from MajorPayne) as T where (Song = 'BBBB')");
        }

        private void CreateWhereWithMemberAccess(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var paynes = new UltimateQueryable<MajorPayne>(provider)
                .Where(payne => payne.Song.Length >= 4);
            Assert.AreEqual(
                paynes.ToString(),
                "");
        }
    }
}