using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Nez;
using Nez.AI.BehaviorTrees;
using Nez.Textures;

namespace MonogameLearning.Platformer.Objects
{
    public class Enemy : BaseObject, IUpdatable, ITriggerListener
    {
        BehaviorTree<Enemy> _tree;
        private int _hitPoints;
        private Vector2 _maxLeftPosition;
        private Vector2 _maxRightPosition;
		private SoundEffect _hurtEffect;
		private SoundEffect _deathEffect;

        public Enemy(Vector2 maxLeftPosition, Vector2 maxRightPosition)
        {
            _maxLeftPosition = maxLeftPosition;
            _maxRightPosition = maxRightPosition;
        }
        protected override void Setup()
        {
            _hitPoints = 3;
            var sprites = Sprite.SpritesFromAtlas(Entity.Scene.Content.LoadTexture("enemy"), 32, 32);

            _animator.AddAnimation(new Animation.RunAnimation().Name, new[]
			{
				sprites[8],
				sprites[9],
				sprites[10],
				sprites[11],
				sprites[12],
				sprites[13],
                sprites[14]
			});

            _animator.AddAnimation(new Animation.DeathAnimation().Name, new[]
			{
				sprites[40],
				sprites[41],
				sprites[42],
				sprites[43],
                sprites[44],
			});

            _animator.AddAnimation(new Animation.HurtAnimation().Name, new[]
			{
				sprites[64],
				sprites[64 + 1]
			});


			_hurtEffect = Entity.Scene.Content.LoadSoundEffect("Sounds/Effects/hurt");
			_deathEffect = Entity.Scene.Content.LoadSoundEffect("Sounds/Effects/death");

            SetUpBehavior();
        }

        private void SetUpBehavior()
        {
            var builder = BehaviorTreeBuilder<Enemy>.Begin(this);

            builder
                .Sequence()
                .Action(x => x.GoToPosition(_maxRightPosition))
                .Action(x => x.GoToPosition(_maxLeftPosition))
                .EndComposite();

            _tree = builder.Build();
        }

        public void OnTriggerEnter(Collider other, Collider local)
        {
            var player = other.GetComponent<Player>();
            if (player is not null)
            {
                if (player.IsAttacking && _hitPoints >0)
                {
                    _hitPoints--;
                    if (_hitPoints<=0)
                    {
                        PlayAnimation(new Animation.DeathAnimation());
                        _deathEffect.Play();
                        return;
                    }
                    _mover.Move(new Vector2(player.IsFlipped ? -30 : 30, 0), _boxCollider, _collisionState);
                    PlayAnimation(new Animation.HurtAnimation());
                    _hurtEffect.Play();
                }
            }
        }

        public void OnTriggerExit(Collider other, Collider local)
        {
        }

        public void Update()
        {
            if (_animator.IsAnimationActive(new Animation.DeathAnimation().Name) && !_animator.IsRunning)
            {
                Entity.Destroy();
            }
            _triggerHelper.Update();
            if (_tree != null && _hitPoints >0)
				_tree.Tick();
        }

        TaskStatus GoToPosition(Vector2 position)
		{
			var moveRight = position.X - Entity.Position.X >= 0;
            var animation = new Animation.RunAnimation();
			if ((Entity.Position.X <= position.X && moveRight) || (Entity.Position.X >= position.X && !moveRight))
			{
                _mover.Move(new Vector2((moveRight ? 500 : -500)* Time.DeltaTime, 010), _boxCollider, _collisionState);
                _animator.FlipX = !moveRight;
                if (!_animator.IsAnimationActive(animation.Name) || !_animator.IsRunning)
                {
                    PlayAnimation(animation);
                }
                if ((Entity.Position.X >= position.X && moveRight) || (Entity.Position.X < position.X && !moveRight))
                {
                    return TaskStatus.Success;
                } 

				return TaskStatus.Running;
			}

			return TaskStatus.Success;
		}
    }
}