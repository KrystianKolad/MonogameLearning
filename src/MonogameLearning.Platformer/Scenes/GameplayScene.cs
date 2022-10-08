using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MonogameLearning.Platformer.Components;
using MonogameLearning.Platformer.Objects;
using Nez;
using Nez.Tiled;
using Nez.UI;

namespace MonogameLearning.Platformer.Scenes
{
    public class GameplayScene : BaseScene
    {
        public override Table Table { get; set; }
        public override void Initialize()
        {
            SetupScene();
            SetDesignResolution(1280, 720, SceneResolutionPolicy.ShowAllPixelPerfect);
			Screen.SetSize(1280, 720);

            var map = Content.LoadTiledMap("Content/TestMap.tmx");
            var mapEntity = CreateEntity("test-map");
            mapEntity.AddComponent(new TiledMapRenderer(map));
            
            var playerPosition = map.GetObjectGroup("Player").Objects.Single();
            var player = CreateEntity("player",new Vector2(playerPosition.X, playerPosition.Y));
            player.AddComponent(new Player());
            var playerCollider = player.AddComponent(new BoxCollider(-16, -4, 32, 32){ IsTrigger = true});
			Flags.SetFlagExclusive(ref playerCollider.CollidesWithLayers, 0);
            Flags.SetFlagExclusive(ref playerCollider.PhysicsLayer, 1);

			player.AddComponent(new TiledMapMover(map.GetLayer<TmxLayer>("Ground")));

            var topLeft = Vector2.Zero;
            var bottomRight = new Vector2(
                map.TileWidth * map.Width, 
                map.TileWidth * map.Height);

            mapEntity.AddComponent(new CameraBounds(topLeft, bottomRight));
            Camera.Entity.AddComponent(new FollowCamera(player));
            
            SetupEnemies(map);
            SetUpDangerZone(map);

            MediaPlayer.Play(this.Content.Load<Song>("Sounds/Soundtrack/gameplayTheme"));
            MediaPlayer.IsRepeating = true;
        }

        private void SetupEnemies(TmxMap map)
        {
            var enemies = new List<Entity>();
            foreach (var enemyObject in map.GetObjectGroup("Enemies").Objects.Where(x=>x.Type.Equals("Enemy")))
            {
                var enemy = CreateEntity($"enemy", new Vector2(enemyObject.X, enemyObject.Y));
                var enemyMaxLeftPosition = map.GetObjectGroup("Enemies").Objects.First(x=>x.Name.Equals($"{enemyObject.Name}-MaxLeft-Position"));
                var enemyMaxRightPosition = map.GetObjectGroup("Enemies").Objects.First(x=>x.Name.Equals($"{enemyObject.Name}-MaxRight-Position"));
                enemy.AddComponent(new Enemy(new Vector2(enemyMaxLeftPosition.X, enemyMaxLeftPosition.Y), new Vector2(enemyMaxRightPosition.X, enemyMaxRightPosition.Y)));
                enemy.AddComponent(new BoxCollider(-10, -16, 20, 32){ IsTrigger = true});
			    enemy.AddComponent(new TiledMapMover(map.GetLayer<TmxLayer>("Ground")));
                enemies.Add(enemy);
            }
        }

        private void SetUpDangerZone(TmxMap map)
        {
            foreach (var item in map.GetObjectGroup("Danger-zone").Objects)
            {
                var zone = CreateEntity("danger-zone", new Vector2(item.X, item.Y));
			
				// add a Trigger-Collider to the zone
				var collider = zone.AddComponent(new BoxCollider(-16, -16, 32, 32){ IsTrigger = true});

				var playerPosition = map.GetObjectGroup("Player").Objects.Single();
				zone.AddComponent(new DangerZone(new Vector2(playerPosition.X, playerPosition.Y)));
            }
        }
    }
}