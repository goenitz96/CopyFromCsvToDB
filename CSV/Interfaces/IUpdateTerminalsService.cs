using CSV.Models;

namespace CSV.Interfaces;

public interface IUpdateTerminalsService
{
    Task<IList<Terminal>> UpdateTerminals();
}