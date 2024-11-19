using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class TugBoat
    {
        public bool IsFree { get; set; } = true;
        public bool IsTowingToBarge { get; set; } = false;
        private int busyTime = 0;

        public int GetBusyTime()
        {
            return busyTime;
        }

        public void DecBusyTime()
        {
            busyTime--;
            IsFree = busyTime <= 0;
        }

        public void SetBusyTime(int busyTime, bool isTowingToBarge)
        {
            IsTowingToBarge = isTowingToBarge;
            IsFree = false;
            this.busyTime = busyTime;
        }
    }
}
