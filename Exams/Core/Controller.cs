using RobotService.Core.Contracts;
using RobotService.Models;
using RobotService.Models.Contracts;
using RobotService.Repositories;
using RobotService.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Core
{
    public class Controller : IController
    {
        private SupplementRepository supplements;
        private RobotRepository robots;
        public Controller()
        {
            supplements = new();
            robots = new();
        }
        public string CreateRobot(string model, string typeName)
        {
            if (typeName != "DomesticAssistant" && typeName != "IndustrialAssistant")
            {
                return String.Format(OutputMessages.RobotCannotBeCreated,typeName);
            }
            IRobot robot = null;
            if (typeName == "DomesticAssistant")
            {
                robot = new DomesticAssistant(model);
            }
            if(typeName == "IndustrialAssistant")
            {
                robot = new IndustrialAssistant(model);
            }
            robots.AddNew(robot);
            return String.Format(OutputMessages.RobotCreatedSuccessfully,typeName,model);
        }

        public string CreateSupplement(string typeName)
        {
            if(typeName != "LaserRadar" && typeName != "SpecializedArm")
            {
                return String.Format(OutputMessages.SupplementCannotBeCreated,typeName);
            }
            ISupplement supplement = null;
            if(typeName == "LaserRadar")
            {
                supplement = new LaserRadar();
            }
            if(typeName == "SpecializedArm")
            {
                supplement = new SpecializedArm();
            }
            supplements.AddNew(supplement);
            return String.Format(OutputMessages.SupplementCreatedSuccessfully,typeName);
        }

        public string PerformService(string serviceName, int intefaceStandard, int totalPowerNeeded)
        {
            List<IRobot> robotsToBeServiced = new();
            foreach(var robot in robots.Models())
            {
                if(robot.InterfaceStandards.Contains(intefaceStandard))
                {
                    robotsToBeServiced.Add(robot);
                }
            }
            if(robotsToBeServiced.Count == 0)
            {
                return String.Format(OutputMessages.UnableToPerform,intefaceStandard);
            }
            List<IRobot> ordered = robotsToBeServiced.OrderByDescending(x => x.BatteryLevel).ToList();
            int powerSum = 0;
            foreach(var robot in ordered)
            {
                powerSum += robot.BatteryLevel;
            }
            if(powerSum < totalPowerNeeded)
            {
                return String.Format(OutputMessages.MorePowerNeeded,serviceName,(totalPowerNeeded - powerSum));
            }
            int totalPower = totalPowerNeeded;
            int robotsCounter = 0;
            while (totalPower > 0)
            {
                foreach(var robot in ordered)
                {
                    while(robot.BatteryLevel > 0)
                    {
                        if(robot.BatteryLevel > totalPower)
                        {
                            robot.ExecuteService(totalPower);
                            totalPower = 0;
                            robotsCounter++;
                            break;
                        }
                        else
                        {
                            int powerToBeConsumed = robot.BatteryLevel;
                            robot.ExecuteService(robot.BatteryLevel);
                            totalPower -= powerToBeConsumed;
                            robotsCounter++;
                        }
                    }
                    if(totalPower == 0)
                    {
                        break;
                    }
                }
            }
            return String.Format(OutputMessages.PerformedSuccessfully, serviceName, robotsCounter);

        }

        public string Report()
        {
            StringBuilder sb = new();
            foreach(var robot in robots.Models().OrderByDescending(x => x.BatteryLevel).ThenBy(x => x.BatteryCapacity))
            {
                sb.AppendLine(robot.ToString());
            }
            return sb.ToString().TrimEnd();
        }

        public string RobotRecovery(string model, int minutes)
        {
            List<IRobot> robotsToFeed = new List<IRobot>();
            foreach(var robot in robots.Models())
            {
                if(robot.Model == model)
                {
                    if(robot.BatteryLevel < robot.BatteryCapacity / 2)
                    {
                        robotsToFeed.Add(robot);
                    }
                }
            }
            foreach(var robot in robotsToFeed)
            {
                robot.Eating(minutes);
            }
            return String.Format(OutputMessages.RobotsFed,robotsToFeed.Count);
            
        }

        public string UpgradeRobot(string model, string supplementTypeName)
        {
            ISupplement supplement = supplements.Models().FirstOrDefault(x => x.GetType().Name == supplementTypeName);
            IRobot robott = null;
            List<IRobot> robotss = new();
            foreach(var robot in robots.Models())
            {
                if(robot.Model == model)
                {
                    if (!robot.InterfaceStandards.Contains(supplement.InterfaceStandard))
                    {
                        robotss.Add(robot);
                    }
                }
            }
            robott = robotss.FirstOrDefault();
            if(robotss.Count == 0)
            {
                return String.Format(OutputMessages.AllModelsUpgraded,model);
            }
            robott.InstallSupplement(supplement);
            return String.Format(OutputMessages.UpgradeSuccessful,model,supplementTypeName);
        }
    }
}
