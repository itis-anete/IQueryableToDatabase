using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace UltimateDatabaseQueryable
{
    public class ProjectionReader<T> : IEnumerable<T>
    {
        private SelectDbEnumerator<T> enumerator;

        public ProjectionReader(DbDataReader reader, Func<ProjectionRow, T> projector)
        {
            enumerator = new SelectDbEnumerator<T>(reader, projector);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var  e = enumerator;
            if (e == null)
            {
                throw new InvalidOperationException(
                    "DbDataReader cannot be enumerated more than once.");
            }
            enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}