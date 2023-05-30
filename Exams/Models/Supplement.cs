using RobotService.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Models
{
    public abstract class Supplement : ISupplement
    {
        private int interfaceStandart;
        private int battery;
        public Supplement(int interfaceStandard, int batteryUsage)
        {
            InterfaceStandard = interfaceStandard;
            BatteryUsage = batteryUsage;
        }
        public int InterfaceStandard
        {
            get { return interfaceStandart; }
            private set
            {
                interfaceStandart = value;
            }
        }   

        public int BatteryUsage
        {
            get { return battery; }
            private set { battery = value; }
        }
    }
}
