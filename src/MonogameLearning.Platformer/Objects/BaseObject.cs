using Nez;
using Nez.Sprites;
using Nez.Tiled;

namespace MonogameLearning.Platformer.Objects
{
    public abstract class BaseObject : Component
    {
        protected SpriteAnimator _animator;
		protected TiledMapMover _mover;
		protected BoxCollider _boxCollider;
		protected TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();
		protected ColliderTriggerHelper _triggerHelper;

        public override void OnAddedToEntity()
        {
            _boxCollider = Entity.GetComponent<BoxCollider>();
            _mover = Entity.GetComponent<TiledMapMover>();
            _animator = Entity.AddComponent(new SpriteAnimator());
            _triggerHelper = new ColliderTriggerHelper(Entity);

            Setup();
        }

        protected abstract void Setup();

        protected void PlayAnimation(Animation animation)
        {
            _animator.Play(animation.Name, animation.LoopMode);
        }

    }
}