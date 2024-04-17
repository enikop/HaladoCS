using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MazeApp.Model
{
    public class ScoreLogger
    {
        private static List<Result> ReadRecords(string filePath)
        {
            List<Result> records = new List<Result>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Result[]));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
                stream.Position = 0;
                records = ((Result[]?)jsonSerializer.ReadObject(stream))?.ToList() ?? new List<Result>();
            }
            return records;
        }
        public static Result? ReadBestResult(string filePath, Result previewRes)
        {
            List<Result> records = ReadRecords(filePath);
            Result? existingRecord = records.FirstOrDefault(r => r.IsSameCategory(previewRes));
            if (existingRecord != null)
            {
                return existingRecord;
            }
            return null;
        }

        public static bool LogScore(string filePath, Result newScore)
        {
            List<Result> records = ReadRecords(filePath);
            // Check if there is a better time for the same parameters
            List<Result> categoryResults = records.FindAll(r => newScore.IsSameCategory(r));
            Result? existingRecord = categoryResults.FirstOrDefault(r => r.ElapsedTime < newScore.ElapsedTime);

            // If it's a new record or an improvement, add/update it in the list
            if (existingRecord == null)
            {
                records.Add(newScore);
                records.RemoveAll(r => categoryResults.Contains(r));
                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<Result>));
                    jsonSerializer.WriteObject(stream, records);
                    string json = Encoding.UTF8.GetString(stream.ToArray());
                    File.WriteAllText(filePath, json);
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
