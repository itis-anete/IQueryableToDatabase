using NUnit.Framework;
using UltimateDatabaseQueryable;
using System.Data.SqlClient;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class SelectTranslatorTests
    {
        [Test]
        public void DummySelect()
            => DbPreparator.GetRefilledDb(CreateDummySelect);

        private void CreateDummySelect(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var paynes = new UltimateQueryable<MajorPayne>(provider)
                .Select(payne => payne);
            Assert.AreEqual(
                paynes.ToString(),
                "select * from (select * from MajorPayne) as T ");
        }
    }
}