using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace UltimateDatabaseQueryable
{
    public class ObjectReader<T> : IEnumerable<T>
        where T : class, new()
    {
        private DbEnumerator<T> dbEnumerator;

        public ObjectReader(DbDataReader dbDataReader)
        {
            dbEnumerator = new DbEnumerator<T>(dbDataReader);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = dbEnumerator;
            if (enumerator == null)
            {
                throw new InvalidOperationException("DbDataReader cannot be enumerated more than once.");
            }
            dbEnumerator = null;
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}