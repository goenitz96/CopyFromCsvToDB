using CSV.Models;

namespace CSV.Interfaces;

public interface ICsvReadFile
{
    Task<IList<Hourly>> csvReadFileAndCopyToDB();

    Task<IList<Hourly>> saveTheData();
    //Task<IList<Hourly>> getAllData();
}