﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;

namespace Flights.Domain.Query
{
    public interface ITimeTableStatusQuery
    {
        IEnumerable<TimeTableStatus> GetTimeTableStatusesByWebSiteId(int id);

        bool IsTimeTableFresh(TimeTableStatus timeTableStatus);
    }
}
