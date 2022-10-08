using Nez.Sprites;

namespace MonogameLearning.Platformer.Objects
{
    public class Animation
    {
        public string Name { get; set; }

        public SpriteAnimator.LoopMode LoopMode { get; set; }

        public Animation(string name, SpriteAnimator.LoopMode loopMode)
        {
            Name = name;
            LoopMode = loopMode;
        }

        public class WalkAnimation : Animation
        {
            public WalkAnimation() : base("Walk", SpriteAnimator.LoopMode.Loop)
            {
            }
        }

        public class RunAnimation : Animation
        {
            public RunAnimation() : base("Run", SpriteAnimator.LoopMode.Loop)
            {
            }
        }
        public class IdleAnimation : Animation
        {
            public IdleAnimation() : base("Idle", SpriteAnimator.LoopMode.Loop)
            {
            }
        }
        public class AttackAnimation : Animation
        {
            public AttackAnimation() : base("Attack", SpriteAnimator.LoopMode.Once)
            {
            }
        }
        public class DeathAnimation : Animation
        {
            public DeathAnimation() : base("Death", SpriteAnimator.LoopMode.ClampForever)
            {
            }
        }
        public class FallingAnimation : Animation
        {
            public FallingAnimation() : base("Falling", SpriteAnimator.LoopMode.Loop)
            {
            }
        }
        public class HurtAnimation : Animation
        {
            public HurtAnimation() : base("Hurt", SpriteAnimator.LoopMode.ClampForever)
            {
            }
        }
        public class JumpingAnimation : Animation
        {
            public JumpingAnimation() : base("Jumping", SpriteAnimator.LoopMode.Loop)
            {
            }
        }

        public class RollAnimation : Animation
        {
            public RollAnimation() : base("Roll", SpriteAnimator.LoopMode.Loop)
            {
            }
        }
    }
}