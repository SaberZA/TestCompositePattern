namespace TestCompositePattern
{
    public interface IParty
    {
        int Gold { get; set; }
        string Name { get; set; }

        void Add(IParty party);

    }
}