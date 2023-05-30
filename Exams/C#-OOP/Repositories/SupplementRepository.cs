using RobotService.Models.Contracts;
using RobotService.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotService.Repositories
{
    public class SupplementRepository : IRepository<ISupplement>
    {
        private List<ISupplement> supplements;
        public SupplementRepository()
        {
            supplements = new List<ISupplement>();
        }
        public void AddNew(ISupplement model)
        {
            supplements.Add(model);
        }

        public ISupplement FindByStandard(int interfaceStandard)
            => supplements.FirstOrDefault(x => x.InterfaceStandard == interfaceStandard);

        public IReadOnlyCollection<ISupplement> Models()
            => supplements.AsReadOnly();

        public bool RemoveByName(string typeName)
        {
            var supplement = supplements.FirstOrDefault(x => x.GetType().Name == typeName);
            if(supplement != null)
            {
                supplements.Remove(supplement);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
