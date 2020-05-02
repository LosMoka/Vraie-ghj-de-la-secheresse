namespace Model
{
    public class MapElement
    {
        public MapElement(int id, int cost)
        {
            Id = id;
            Cost = cost;
        }
        public int Id { get; }
        public int Cost { get; }
    }
}