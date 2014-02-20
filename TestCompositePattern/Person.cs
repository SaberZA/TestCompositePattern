using System.Collections.Generic;

namespace TestCompositePattern
{
    public class Person : IParty
    {
        public string Name { get; set; }
        public void Add(IParty party)
        {
            Parties.Add(party);
        }

        public List<IParty> Parties { get; set; }

        public int Gold { get; set; }
    }
}