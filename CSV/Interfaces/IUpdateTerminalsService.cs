using CSV.Models;

namespace CSV.Interfaces;

public interface IUpdateTerminalsService
{
    Task<IList<Terminal>> UpdateTerminals();
    Task<IList<TerminalAppend>> UpdateTerminalsAppend();
    Task AllInOneJob();
}