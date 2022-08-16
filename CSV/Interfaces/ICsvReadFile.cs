using CSV.Models;

namespace CSV.Interfaces;

public interface ICsvReadFile
{
    Task<IList<Hourly>> csvReadFileAndCopyToDB();

    Task<IList<Hourly>> saveTheData();
    void downloadFtpCsvFile(string userName, string password, string host, string fileName, string ftpPath, string downloadPath);
    void deleteFile(string fileName, string downloadPath);
}