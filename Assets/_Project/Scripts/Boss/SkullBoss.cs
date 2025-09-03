using Nato;
using UnityEngine;

public class SkullBoss : BossBase
{
    public override void Start()
    {
        base.Start();
    }

    public override void StartBoss()
    {
        base.StartBoss();
        SetPatterns();
        DoPatterns();
    }

    public override void SetPatterns()
    {
        base.SetPatterns();
        patterns.Add(FollowPattern);
        patterns.Add(JumpAttackPattern);
        patterns.Add(ShootAttackPattern);
    }

    private void FollowPattern()
    {

    }

    private void JumpAttackPattern()
    {

    }

    private void ShootAttackPattern()
    {
        
    }
}
