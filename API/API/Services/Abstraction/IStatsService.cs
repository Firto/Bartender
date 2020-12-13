using API.DataBase.Entities;
using API.Hostings.ServiceInitialization.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Abstraction
{
    public interface IStatsService: IOnInitService
    {
        void AddStat(string name, int count);
        IEnumerable<Stat> GetStats();
    }
}
