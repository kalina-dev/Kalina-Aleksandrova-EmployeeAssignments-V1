using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

public class CustomDateTimeConverter : DateTimeConverter
{
    private readonly string[] _dateFormats = new[]
    {
        "yyyy-MM-dd",
        "MM/dd/yyyy",
        "dd/MM/yyyy",
        "yyyy/MM/dd",
        "MM-dd-yyyy",
        "dd-MM-yyyy"
    };

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        // Try parsing the date with different formats
        foreach (var format in _dateFormats)
        {
            if (DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
        }
        throw new Exception($"Unable to parse date: {text}");
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        // Convert DateTime to string in a standard format
        return ((DateTime)value).ToString("yyyy-MM-dd");
    }
}
