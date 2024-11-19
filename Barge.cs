using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Berth
    {
        public bool IsFree { get; set; } = true;
        public bool NeedTowing { get; set; } = false;

        private int busyTime = 0;

        public int GetBusyTime()
        {
            return busyTime;
        }

        public void DecBusyTime()
        {
            busyTime--;
            NeedTowing = busyTime <= 0;
        }

        public void SetBusyTime(int busyTime)
        {
            IsFree = false;
            this.busyTime = busyTime;
        }
    }
}
