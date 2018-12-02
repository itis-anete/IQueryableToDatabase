using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace UltimateDatabaseQueryable
{
    public class DbEnumerator<T> : IEnumerator<T>
        where T : class, new()
    {
        private readonly DbDataReader dbDataReader;
        private readonly PropertyInfo[] propertyInfos;
        private int[] propertyLookup;

        public DbEnumerator(DbDataReader dbDataReader)
        {
            this.dbDataReader = dbDataReader;
            propertyInfos = typeof(T).GetProperties();
        }

        public T Current { get; private set; }
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (dbDataReader.Read())
            {
                if (propertyLookup == null)
                {
                    InitFieldLookup();
                }
                var instance = new T();
                for (var i = 0; i < propertyInfos.Length; i++)
                {
                    var index = propertyLookup[i];
                    if (index >= 0)
                    {
                        var propInfo = propertyInfos[i];
                        if (dbDataReader.IsDBNull(index))
                        {
                            propInfo.SetValue(instance, null);
                        }
                        else
                        {
                            propInfo.SetValue(instance, dbDataReader.GetValue(index));
                        }
                    }
                }
                Current = instance;
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            dbDataReader.Dispose();
        }

        public void Reset()
        {
        }

        private void InitFieldLookup()
        {
            var map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            for (var i = 0; i < dbDataReader.FieldCount; i++)
            {
                map.Add(dbDataReader.GetName(i), i);
            }
            propertyLookup = new int[propertyInfos.Length];
            for (var i = 0; i < propertyInfos.Length; i++)
            {
                if (map.TryGetValue(propertyInfos[i].Name, out var index))
                {
                    propertyLookup[i] = index;
                }
                else
                {
                    propertyLookup[i] = -1;
                }
            }
        }
    }
}