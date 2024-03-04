namespace Game
{
    public static class AttackHandler
    {
        public static void HandleAttack(IEntity entity, IHittable target, int damagePoint)
        {
            if (!target.canBeHit) return;

            if (entity.groupId == TeamGroup.Default.id || target.groupId == TeamGroup.Default.id ||
                entity.groupId != target.groupId)
            {
                target.CmdBeAttack(damagePoint);
            }
        }
    }
}