using CleanCode.VariableNames3.ArcadeGame;

namespace CleanCode.CtorInterfaceNames.Monsters
{
    public class Bear : MonsterAbstract
    {
        public override int AttackRadius => 5;
        public override int Speed => 3;

        public Bear(Field field) : base(field) { }
    }
}