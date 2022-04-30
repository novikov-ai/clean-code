using CleanCode.VariableNames3.ArcadeGame;

namespace CleanCode.CtorInterfaceNames.Monsters
{
    public class Wolf : MonsterAbstract
    {
        public override int AttackRadius => 3;
        public override int Speed => 5;

        public Wolf(Field field) : base(field) { }
    }
}