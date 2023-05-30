using RobotService.Models.Contracts;
using RobotService.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Models
{
    public abstract class Robot : IRobot
    {
        
        private string model;
        private int batteryCapacity;
        private int batteryLevel;
        private int conversionCapacityIndex;
        private List<int> interfaceStandarts;
        public Robot(string model, int batteryCapacity, int conversionCapacityIndex)
        {
            Model = model;
            BatteryCapacity = batteryCapacity;
            batteryLevel = batteryCapacity;
            ConvertionCapacityIndex = conversionCapacityIndex;
            interfaceStandarts = new List<int>();
            
        }
        public string Model
        {
            get { return model; }
            private set 
            {
                if(String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.ModelNullOrWhitespace);
                }
                model = value;
            }
        }

        public int BatteryCapacity
        {
            get { return batteryCapacity; }
            private set 
            {
                if(batteryCapacity < 0)
                {
                    throw new ArgumentException(ExceptionMessages.BatteryCapacityBelowZero);
                }
                batteryCapacity = value;
            }
        }

        public int BatteryLevel
        {
            get { return batteryLevel; }
        }

        public int ConvertionCapacityIndex
        {
            get { return conversionCapacityIndex; }
            private set 
            {
                conversionCapacityIndex = value;
            }
        }

        public IReadOnlyCollection<int> InterfaceStandards => interfaceStandarts.AsReadOnly();

        public void Eating(int minutes)
        {
            int acumudatedEnergyFromEating = ConvertionCapacityIndex * minutes;
            if(batteryLevel + acumudatedEnergyFromEating > BatteryCapacity)
            {
                batteryLevel = BatteryCapacity;
            }
            else 
            {
                batteryLevel += acumudatedEnergyFromEating;
            }
        }

        public bool ExecuteService(int consumedEnergy)
        {
            if(BatteryLevel >= consumedEnergy)
            {
                batteryLevel -= consumedEnergy;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InstallSupplement(ISupplement supplement)
        {
            if(supplement.GetType().Name == "LaserRadar")
            {
                interfaceStandarts.Add(20082);
            }
            else
            {
                interfaceStandarts.Add(10045);
            }
            this.BatteryCapacity -= supplement.BatteryUsage;
            this.batteryLevel = BatteryCapacity;
        }
        public override string ToString()
        {
            List<int> installedSupplements = new List<int>();
            foreach (var supplement in interfaceStandarts)
            {
                if(!installedSupplements.Contains(supplement))
                {
                    installedSupplements.Add(supplement);
                }
            }
            StringBuilder sb = new();
            sb.AppendLine($"{this.Model.GetType().Name} {Model}:");
            sb.AppendLine($"--Maximum battery capacity: {BatteryCapacity}");
            sb.AppendLine($"--Current battery level: {BatteryLevel}");
            if(interfaceStandarts.Count > 0)
            {
                sb.AppendLine($"--Supplements installed: {String.Join(" ", installedSupplements)}");
            }
            else
            {
                sb.AppendLine("--Supplements installed: none");
            }
            return sb.ToString().Trim();
        }
    }
}
