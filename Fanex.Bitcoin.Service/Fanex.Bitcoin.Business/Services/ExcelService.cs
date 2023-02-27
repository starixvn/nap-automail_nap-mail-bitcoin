using Fanex.Bitcoin.Business.Models;
using System;
using System.Collections.Generic;

namespace Fanex.Bitcoin.Business.Services
{
    public interface ExcelService
    {
        string Export(Tuple<IEnumerable<BitcoinModel>, IEnumerable<BitcoinModel>> data);
    }
}