﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthParse.Standard.Health.Sheets
{
    public class StepBuilder : ISheetBuilder<StepBuilder.StepItem>
    {
        private readonly IEnumerable<Record> _records;

        public StepBuilder(IReadOnlyDictionary<string, IEnumerable<Record>> records)
        {
            _records = records.ContainsKey(HKConstants.Records.StepCount)
                ? records[HKConstants.Records.StepCount]
                : Enumerable.Empty<Record>();
        }

        IEnumerable<object> ISheetBuilder.BuildRawSheet()
        {
            return GetStepsByDay();
        }

        IEnumerable<StepItem> ISheetBuilder<StepItem>.BuildSummary()
        {
            return GetStepsByDay()
                .GroupBy(s => new { s.Date.Year, s.Date.Month })
                .Select(x => new StepItem(x.Key.Year, x.Key.Month, x.Sum(r => r.Steps)));
        }

        IEnumerable<StepItem> ISheetBuilder<StepItem>.BuildSummaryForDateRange(IRange<DateTime> dateRange)
        {
            return GetStepsByDay().Where(x => dateRange.Includes(x.Date, Clusivity.Inclusive));
        }

        private IEnumerable<StepItem> GetStepsByDay()
        {
            return StepHelper.PrioritizeSteps(_records)
                .GroupBy(s => s.StartDate.Date)
                .Select(x => new StepItem(x.Key, (int)x.Sum(r => r.Value.SafeParse(0))))
                .OrderByDescending(s => s.Date);
        }

        public class StepItem : DatedItem
        {
            public StepItem(DateTime date, int steps) : base(date)
            {
                Steps = steps;
            }

            public StepItem(int year, int month, int steps) : base(year, month)
            {
                Steps = steps;
            }

            public int Steps { get; internal set; }

        }
    }
}
