﻿using System.Collections.Generic;
using NodaTime;

namespace HealthParse.Standard.Health.Sheets
{
    public class CyclingWorkoutBuilder : WorkoutBuilder
    {
        public CyclingWorkoutBuilder(IReadOnlyDictionary<string, IEnumerable<Workout>> workouts, DateTimeZone zone)
            : base(workouts, HKConstants.Workouts.Cycling, zone, r => new
            {
                date = r.StartDate.InZone(zone),
                duration = r.Duration,
                durationUnit = r.DurationUnit,
                distance = r.TotalDistance,
                unit = r.TotalDistanceUnit,
            })
        {
        }
    }
}
