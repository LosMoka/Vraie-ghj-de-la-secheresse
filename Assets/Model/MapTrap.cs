namespace Model
{
    public class MapTrap
    {
        public MapTrap(int id, int cost)
        {
            Id = id;
            Cost = cost;
        }

        public int Id { get; }
        public int Cost { get; }
    }
}