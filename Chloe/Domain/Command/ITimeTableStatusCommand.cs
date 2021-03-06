﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDto = Flights.Dto;

namespace Flights.Domain.Command
{
    public interface ITimeTableStatusCommand
    {
        FlightsDto.TimeTableStatus Merge(FlightsDto.TimeTableStatus timeTableStatus);
    }
}
