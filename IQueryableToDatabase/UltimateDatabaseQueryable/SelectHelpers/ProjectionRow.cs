using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateDatabaseQueryable
{
    public abstract class ProjectionRow
    {
        public abstract object GetValue(int index);
    }
}