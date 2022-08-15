using CsvHelper;

namespace CSV.Helpers;

public class Defaulter<T> : CsvHelper.TypeConversion.ITypeConverter
{
    Exception conversionError;
    string offendingValue;

    public Exception GetLastError()
    {
        return conversionError;
    }

    public string GetOffendingValue()
    {
        return offendingValue;
    }

    object CsvHelper.TypeConversion.ITypeConverter.ConvertFromString(string text, IReaderRow row, CsvHelper.Configuration.MemberMapData memberMapData)
    {
        conversionError = null;
        offendingValue = null;
        try
        {
            return (T)Convert.ChangeType(text, typeof(T));
        }
        catch (Exception localConversionError)
        {
            conversionError = localConversionError;
        }
        return default(T);
    }
    string CsvHelper.TypeConversion.ITypeConverter.ConvertToString(object value, IWriterRow row, CsvHelper.Configuration.MemberMapData memberMapData)
    {
        return Convert.ToString(value);
    }
}