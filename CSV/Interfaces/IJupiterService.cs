using CSV.Models;

namespace CSV.Interfaces;

public interface IJupiterService
{
    Task<IList<Terminal>> getDataFromJupiter();
}