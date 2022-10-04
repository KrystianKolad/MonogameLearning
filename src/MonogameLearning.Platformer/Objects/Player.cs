using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tiled;

namespace MonogameLearning.Platformer.Objects
{
    public class Player : Component, IUpdatable
	{
		public float MoveSpeed = 150;
		public float Gravity = 1000;
		public float JumpHeight = 16 * 5;

        SpriteAnimator _animator;
		TiledMapMover _mover;
		BoxCollider _boxCollider;
		TiledMapMover.CollisionState _collisionState = new TiledMapMover.CollisionState();
		Vector2 _velocity;
		ColliderTriggerHelper _triggerHelper;

		VirtualButton _jumpInput;
		VirtualIntegerAxis _xAxisInput;

		public override void OnAddedToEntity()
		{
			var texture = Entity.Scene.Content.LoadTexture("player");
			var sprites = Sprite.SpritesFromAtlas(texture, 32, 32);

			_boxCollider = Entity.GetComponent<BoxCollider>();
			_mover = Entity.GetComponent<TiledMapMover>();
            _animator = Entity.AddComponent(new SpriteAnimator(sprites[0]));

            _animator.AddAnimation("Walk", new[]
			{
				sprites[0]
			});

			// the TiledMapMover does not call ITriggerListener Methods on collision.
			// To achieve ITriggerListener calling, this ColliderTriggerHelper can be used.
			// See the Update() function below, to see how this helper can be used.
			_triggerHelper = new ColliderTriggerHelper(Entity);

			SetupInput();
		}

		public override void OnRemovedFromEntity()
		{
			// deregister virtual input
			_jumpInput.Deregister();
			_xAxisInput.Deregister();
		}

		void SetupInput()
		{
			// setup input for jumping. we will allow z on the keyboard or a on the gamepad
			_jumpInput = new VirtualButton();
			_jumpInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Up));
			_jumpInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.W));
			_jumpInput.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

			// horizontal input from dpad, left stick or keyboard left/right
			_xAxisInput = new VirtualIntegerAxis();
			_xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
			_xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
			_xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
			_xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));
		}

		void IUpdatable.Update()
		{
			// handle movement and animations
			var moveDir = new Vector2(_xAxisInput.Value, 0);

			if (moveDir.X < 0)
			{
				_velocity.X = -MoveSpeed;
			}
			else if (moveDir.X > 0)
			{
				_velocity.X = MoveSpeed;
			}
			else
			{
				_velocity.X = 0;
			}

			if (_collisionState.Below && _jumpInput.IsPressed)
			{
				_velocity.Y = -Mathf.Sqrt(2f * JumpHeight * Gravity);
			}

			if (_collisionState.Above && _velocity.Y < 0)
			{
				_velocity.Y = 0;
			}

			// apply gravity
			_velocity.Y += Gravity * Time.DeltaTime;

			// move
			_mover.Move(_velocity * Time.DeltaTime, _boxCollider, _collisionState);
			
			// Update the TriggerHelper. This will check if our collider intersects with a
			// trigger-collider and call ITriggerListener if necessary.
			_triggerHelper.Update();

			if (_collisionState.Below)
				_velocity.Y = 0;

            _animator.Play("Walk");
		}
    }
}