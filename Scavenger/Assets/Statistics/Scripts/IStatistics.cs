using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IStatistics
{
    bool Has(string parameter);
    Statistic this[string index] { get; set; }
}
