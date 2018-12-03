using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace UltimateDatabaseQueryable
{
    class SelectDbEnumerator<T> : ProjectionRow, IEnumerator<T>
    {
        private readonly Func<ProjectionRow, T> projector;
        private readonly DbDataReader reader;

        public T Current { get; private set; }
        object IEnumerator.Current => Current;

        internal SelectDbEnumerator(DbDataReader reader, Func<ProjectionRow, T> projector)
        {
            this.reader = reader;
            this.projector = projector;
        }

        public override object GetValue(int index)
        {
            if (index >= 0)
            {
                if (reader.IsDBNull(index)) return null;
                else return reader.GetValue(index);
            }
            throw new IndexOutOfRangeException();
        }

        public bool MoveNext()
        {
            if (reader.Read())
            {
                Current = projector(this);
                return true;
            }
            return false;
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}