namespace YoloCrawler.Entities
{
    using Fighting;

    public class YoloTeam : ICanAttack, ICanBeAttacked
    {
        public string Name { get; private set; }
        public Position Position { get; private set; }

        public int Hitpoints { get; private set; }

        public YoloTeam(Position startingPosition)
        {
            Position = startingPosition;
            Hitpoints = 10;
            Name = "#YoloTeam";
        }

        public void Move(Offset offset)
        {
            Position += offset;
        }

        public void EnterRoom(Position newRoomStartingPosition)
        {
            Position = newRoomStartingPosition;
        }

        public void Attack(ICanBeAttacked target)
        {
            target.Take(new Damage
            {
                Hitpoints = 1
            });
        }

        public void Take(Damage dmg)
        {
            Hitpoints -= dmg.Hitpoints;
        }
    }
}