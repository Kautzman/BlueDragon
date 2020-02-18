namespace BlueDragon
{
    class Result
    {
        public int procCount { get; set; }
        public float tickCount { get; set; }
        public float lostTicks { get; set; }
        public float effectiveMP5 { get; set; }
        public float effective3pcMP5 { get; set; }
        public float procRate { get; set; }

        public Result(int thisProcCount, float thisTickCount, float thisLostTicks, float thisEffectiveMP5, float thisEffective3pcMP5, float thisProcRate)
        {
            procCount = thisProcCount;
            tickCount = thisTickCount;
            lostTicks = thisLostTicks;
            effectiveMP5 = thisEffectiveMP5;
            effective3pcMP5 = thisEffective3pcMP5;
            procRate = thisProcRate;
        }
    }
}
