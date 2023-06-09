﻿namespace AndonMonitoring.Model
{
    public class MonthStat
    {
        public int Id { get; set; }
        public DateTime Month { get; set; }
        public int AndonId { get; set; }
        public Andon Andon { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }
        public int OverallMinutes { get; set; }
        public int StateCount { get; set; }
    }
}
