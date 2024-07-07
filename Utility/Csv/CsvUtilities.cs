using System.IO;
using System.Text;

namespace SwiftAPI.Utility.Csv
{
    public static class CsvUtilities
    {
        public static string ToCsvString(this string[,] strings)
        {
            StringBuilder builder = new();
            for (int r = 0; r < strings.GetLength(0); r++)
                builder.AppendLine(string.Join(",", strings.GetRow(r)));
            return builder.ToString();
        }

        public static bool WriteCsvFile(string path, string name, string csvString)
        {
            if (Directory.Exists(path))
            {
                File.WriteAllText(path + name + ".csv", csvString);
                return true;
            }
            return false;
        }

        public static bool WriteCsvFile(string path, string name, string[,] csv) => WriteCsvFile(path, name, csv.ToCsvString());

        public static string ReadCsvFile(string path) => File.Exists(path) ? File.ReadAllText(path) : null;

        public static bool TryReadCsvFile(string path, out string csv)
        {
            csv = ReadCsvFile(path);
            return csv != null;
        }

        public static string[,] GetArrayString(string csv)
        {
            if (csv == null)
                return null;

            string[] rows = csv.Split('\n');

            if (rows.Length == 0)
                return null;

            int column = rows[0].Split(',').Length;
            string[,] result = new string[rows.Length, column];

            for (int r = 0; r < rows.Length; r++)
            {
                string[] temp = rows[r].Split(',');
                int row = 0;
                for (int c = 0; c < column; c++)
                    result[r, c] = temp[row++];
            }

            return result;
        }

        public static bool TryGetArrayString(string csv, out string[,] result)
        {
            result = GetArrayString(csv);
            return result != null;
        }

        public static bool TryCsvToArray(string path, out string[,] result)
        {
            if (TryReadCsvFile(path, out string csv) && TryGetArrayString(csv, out result))
                return true;
            result = null;
            return false;
        }
    }
}
