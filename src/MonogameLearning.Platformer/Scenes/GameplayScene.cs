using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            var tiledEntity = CreateEntity("test-map");
            tiledEntity.AddComponent(new TiledMapRenderer(map));

            var player = CreateEntity("player",new Vector2(640, 360));
            player.AddComponent(new Player());
            player.AddComponent(new BoxCollider(-8, -16, 32, 32));
			player.AddComponent(new TiledMapMover(map.GetLayer<TmxLayer>("Ground")));

            var topLeft = Vector2.Zero;
            var bottomRight = new Vector2(
                map.TileWidth * map.Width, 
                map.TileWidth * map.Height);

            Camera.Entity.AddComponent(new FollowCamera(player));
        }
    }
}