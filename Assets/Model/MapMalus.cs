namespace Model
{
    public class MapMalus
    {
        public MapMalus(int id, int cost)
        {
            Id = id;
            Cost = cost;
        }

        public int Id { get; }
        public int Cost { get; }
    }
}