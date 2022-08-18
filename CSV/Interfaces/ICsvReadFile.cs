using CSV.Models;

namespace CSV.Interfaces;

public interface ICsvReadFile
{
    Task<IList<Hourly>> csvReadFileAndCopyToDB();

    Task<IList<Hourly>> saveTheData();
    Task insertIntoHourlyBilling();
    void downloadFtpCsvFile(string userName, string password, string host, string fileName, string ftpPath);
    void deleteFile();
}