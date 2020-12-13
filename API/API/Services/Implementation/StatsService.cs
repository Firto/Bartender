using API.AddtionalClases.ValidatingService;
using API.DataBase.Entities;
using API.Helpers;
using API.Managers.CClientErrorManager;
using API.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace API.Services.Implementation
{
    public class StatsService : IStatsService
    {
        public IGenericRepository<Stat> _statsGR;
        public StatsService(IGenericRepository<Stat> statsGR) {
            _statsGR = statsGR;
        }
        public static void OnInit(IValidatingService validating)
        {
            validating.AddValidateFunc("name", (string prop, PropValidateContext context) =>
            {
                if (prop == null) return;

                if (prop.Length > 40)
                    context.Valid.Add($"Too long name, maximum 40 characters!");

                if (prop.Length < 1)
                    context.Valid.Add($"Too short name!");

                if (!Regex.Match(prop, "^[a-zA-Z_0-9]*$").Success)
                    context.Valid.Add($"Name musn't have specials chars!");
            });

            validating.AddValidateFunc("stats", (string stats, PropValidateContext context) =>
            {
                if (stats == null) return;
                if (!Int32.TryParse(stats, out var m) || m < 1)
                    context.Valid.Add($"Incorrect stats!");
            });
        }
        public void AddStat(string name, int count)
        {
            var m = _statsGR.Find(name);
            if (m == null)
                _statsGR.Create(new Stat { Name = name, Points = count });
            else if (m.Points < count)
            {
                m.Points = count;
                _statsGR.Update(m);
            }
        }

        public IEnumerable<Stat> GetStats()
        {
            return _statsGR.GetDbSet().OrderByDescending(item => item.Points).ToArray();
        }
    }
}
