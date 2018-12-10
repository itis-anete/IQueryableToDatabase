using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NUnit.Framework;
using UltimateDatabaseQueryable;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class SelectDbAccessTests
    {
        [Test]
        public void SimpleSelect()
            => DbPreparator.GetRefilledDb(CreateSimpleSelect);

        private void CreateSimpleSelect(SqlConnection sqlConnection)
        {
            var provider = new DbQueryProvider(sqlConnection);
            var paynes = new UltimateQueryable<MajorPayne>(provider)
                .Select(payne => payne.Height)
                .AsEnumerable();
            Assert.AreEqual(paynes.Count(), 4);
        }
    }
}
