using BuscadorPartitura.Core.Interfaces;
using BuscadorPartitura.Core.Model;
using BuscadorPartitura.Infra.Helpers;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using BuscadorPartitura.Core.Helpers;

namespace BuscadorPartitura.Core.Services
{
    public class SQLiteDatabaseService : IDatabase
    {
        public void SaveMetric(MetricStatus metrics)
        {
            using (var sqlconn = new SQLiteConnection(EnvironmentHelper.GetValue("databaseConnection")))
            {
                sqlconn.Open();
                try
                {
                    var sqlcom = new SQLiteCommand($"UPDATE MachinesConsume " +
                                                      $"SET CpuUsage = {metrics.CpuUsage}, " +
                                                          $"MemoryUsage = {metrics.MemoryUsage} " +
                                                     $"WHERE MachineName = '{metrics.MachineName}'", sqlconn);
                    sqlcom.ExecuteNonQuery();
                }
                finally
                {
                    sqlconn.Close();
                    sqlconn.Dispose();
                }
            }
        }

        public string GetBestQueue()
        {
            using (var sqlconn = new SQLiteConnection(EnvironmentHelper.GetValue("databaseConnection")))
            {
                sqlconn.Open();

                try
                {
                    //SELECT * FROM MachinesConsume WHERE 
                    var sqlcom = new SQLiteCommand($"Select MIN(CpuUsage) CpuUsage, MachineName from MachinesConsume", sqlconn);                    

                    var reader = sqlcom.ExecuteReader();
                    string machineName = Environment.MachineName;
                    while (reader.Read())                    
                        machineName = reader.GetString(1);

                    return MqHelper.OrchestratorQueueName(machineName);
                }
                finally
                {
                    sqlconn.Close();
                    sqlconn.Dispose();
                }
            }            
        }

        public IEnumerable<ScheduledSearch> GetSheetsToSearch()
        {
            return new List<ScheduledSearch>();
        }
    }
}
