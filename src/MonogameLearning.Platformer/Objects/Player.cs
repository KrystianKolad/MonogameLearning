using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Textures;

namespace MonogameLearning.Platformer.Objects
{
    public class Player : BaseObject, IUpdatable, ITriggerListener
	{
		public float MoveSpeed = 150;
		public float Gravity = 1000;
		public float JumpHeight = 16 * 5;
		Vector2 _velocity;
		private SoundEffect _slashEffect;
		private SoundEffect _jumpEffect;
		private SoundEffect _hurtEffect;
		private SoundEffect _deathEffect;

		VirtualButton _attackButton;
		VirtualButton _jumpInput;
		VirtualIntegerAxis _xAxisInput;
		public bool IsDead => _hitPoints <= 0;
		private int _hitPoints;
		private Vector2 _startPosition;
		public bool IsAttacking { get; set; }
		public bool IsFlipped => _animator.FlipX;

		protected override void Setup()
		{
			_hitPoints = 3;
			_startPosition = Entity.Position;
			_animator.AddAnimation(new Animation.IdleAnimation().Name, GetSprites("Player/player1", 0, 6));

			_animator.AddAnimation(new Animation.RunAnimation().Name, GetSprites("Player/player1", 16, 8));

			_animator.AddAnimation(new Animation.AttackAnimation().Name, GetSprites("Player/player1", 8, 6));

			_animator.AddAnimation(new Animation.DeathAnimation().Name, GetSprites("Player/player1", 48, 12));

			_animator.AddAnimation(new Animation.FallingAnimation().Name, GetSprites("Player/player1", 31, 4));

			_animator.AddAnimation(new Animation.HurtAnimation().Name, GetSprites("Player/player1", 40, 4));

			_animator.AddAnimation(new Animation.JumpingAnimation().Name, GetSprites("Player/player1", 26, 4));

			_slashEffect = Entity.Scene.Content.LoadSoundEffect("Sounds/Effects/slash");
			_jumpEffect = Entity.Scene.Content.LoadSoundEffect("Sounds/Effects/jump");
			_hurtEffect = Entity.Scene.Content.LoadSoundEffect("Sounds/Effects/hurt");
			_deathEffect = Entity.Scene.Content.LoadSoundEffect("Sounds/Effects/death");

			SetupInput();

			Sprite[] GetSprites(string spriteName, int startIndex, int numberOfFrames)
			{
				var sprites = Sprite.SpritesFromAtlas(Entity.Scene.Content.LoadTexture(spriteName), 56, 56);
				var result = new List<Sprite>();

				for (int i = 0; i < numberOfFrames; i++)
				{
					result.Add(sprites[startIndex + i]);
				}

				return result.ToArray();
			}
		}

		public override void OnRemovedFromEntity()
		{
			// deregister virtual input
			_jumpInput.Deregister();
			_xAxisInput.Deregister();
		}

		public void OnTriggerEnter(Collider other, Collider local)
		{
			if (!IsAttacking && other.GetComponent<Enemy>() is not null)
			{
				_hitPoints--;
				if (IsDead)
				{
					PlayAnimation(new Animation.DeathAnimation());
					_deathEffect.Play();
					return;
				}
				PlayAnimation(new Animation.HurtAnimation());
				_hurtEffect.Play();
				_mover.Move(new Vector2(IsFlipped ? 50 : -50, 0), _boxCollider, _collisionState);
			}
		}

		public void OnTriggerExit(Collider other, Collider local)
		{ }

		void SetupInput()
		{
			// setup input for jumping. we will allow z on the keyboard or a on the gamepad
			_jumpInput = new VirtualButton();
			_jumpInput.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Up));

			_attackButton = new VirtualButton();
			_attackButton.Nodes.Add(new VirtualButton.KeyboardKey(Keys.A));

			// horizontal input from dpad, left stick or keyboard left/right
			_xAxisInput = new VirtualIntegerAxis();
			_xAxisInput.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
			_xAxisInput.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
			_xAxisInput.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
		}

		void IUpdatable.Update()
		{
			if (_animator.IsAnimationActive(new Animation.HurtAnimation().Name) &&
				_animator.IsRunning)
			{
				return;
			}
			if (_animator.IsAnimationActive(new Animation.DeathAnimation().Name) &&
				_animator.IsRunning)
			{
				return;
			}
			else if (_animator.IsAnimationActive(new Animation.DeathAnimation().Name) &&
				!_animator.IsRunning)
			{
				_hitPoints = 3;
				Entity.Position = _startPosition;
				if (_animator.FlipX)
				{
					_animator.FlipX = false;
				}
			}

			if (_animator.IsAnimationActive(new Animation.AttackAnimation().Name) &&
				_animator.IsRunning)
			{
				return;
			}

			if (_animator.IsAnimationActive(new Animation.AttackAnimation().Name) &&
				!_animator.IsRunning)
			{
				IsAttacking = false;
				_boxCollider.SetSize(_boxCollider.Width - 20, _boxCollider.Height);
			}
			// handle movement and animations
			var moveDir = new Vector2(_xAxisInput.Value, 0);
			Animation animation = null;

			if (moveDir.X < 0)
			{
				if (_collisionState.Below)
					animation = new Animation.RunAnimation();
				_animator.FlipX = true;
				_velocity.X = -MoveSpeed;
			}
			else if (moveDir.X > 0)
			{
				if (_collisionState.Below)
					animation = new Animation.RunAnimation();
				_animator.FlipX = false;
				_velocity.X = MoveSpeed;
			}
			else
			{
				if (_collisionState.Below)
					animation = new Animation.IdleAnimation();
				_velocity.X = 0;
			}

			if (_collisionState.Below && _jumpInput.IsPressed)
			{
				animation = new Animation.JumpingAnimation();
				_jumpEffect.Play();
				_velocity.Y = -Mathf.Sqrt(2f * JumpHeight * Gravity);
			}

			if (!_collisionState.Below && _velocity.Y > 0)
				animation = new Animation.FallingAnimation();

			if (_collisionState.Above && _velocity.Y < 0)
			{
				_velocity.Y = 0;
			}

			if (_attackButton.IsPressed && _collisionState.Below)
			{
				IsAttacking = true;
				animation = new Animation.AttackAnimation();
				_slashEffect.Play();
				_boxCollider.SetSize(_boxCollider.Width + 20, _boxCollider.Height);
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

			if (animation != null && 
				!_animator.IsAnimationActive(animation.Name) &&
				!IsOneTimeAnimationPlaying())
				PlayAnimation(animation);

			bool IsOneTimeAnimationPlaying()
			{
				var oneTimeAnimations = new Animation[]
				{
					new Animation.DeathAnimation(),
					new Animation.HurtAnimation(),
				};
				return oneTimeAnimations.Any(x=>x.Name.Equals(_animator.CurrentAnimationName) && _animator.IsRunning);
			}
		}
	}
}