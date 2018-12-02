using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using UltimateDatabaseQueryable;

namespace Tests
{
    public static class DbPreparator
    {
        public static void GetRefilledDb(Action<SqlConnection> tester)
        {
            EmptyDatabase();
            InsertPaynes();
            InsertBobas();
            using (SqlConnection sqlConnection = new SqlConnection(AppSettings.LocalConnectionString))
            {
                tester(sqlConnection);
            }
        }

        private static void EmptyDatabase()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                dbContext.Database.ExecuteSqlCommand("truncate table MajorPayne");
                dbContext.Database.ExecuteSqlCommand("truncate table Boba");
            }
        }

        private static void InsertPaynes()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var paynes = new[]
                {
                    new MajorPayne(160, new Boots(Color.Black, 6.0), "AAAA"),
                    new MajorPayne(170, new Boots(Color.Brown, 7.0), "BBBB"),
                    new MajorPayne(180, new Boots(Color.Red, 8.0), "CCCC"),
                    new MajorPayne(190, new Boots(Color.White, 9.0), "DDDD")
                };
                dbContext.MajorPayne.AddRange(paynes);
                dbContext.SaveChanges();
            }
        }

        private static void InsertBobas()
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                var bobas = new[]
                {
                    new Boba(new Boots(Color.Black, 12.0), true),
                    new Boba(null, true),
                    new Boba(new Boots(Color.Red, 16.0), true),
                    new Boba(null, false)
                };
                dbContext.Boba.AddRange(bobas);
                dbContext.SaveChanges();
            }
        }
    }
}