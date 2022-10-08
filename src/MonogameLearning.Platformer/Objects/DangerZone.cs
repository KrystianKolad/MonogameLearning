using Microsoft.Xna.Framework;
using Nez;

namespace MonogameLearning.Platformer.Objects
{
    public class DangerZone : Component, ITriggerListener
    {
        Vector2 _respawnPoint;

        public DangerZone(Vector2 respawnPoint)
        {
            _respawnPoint = respawnPoint;
        }

        public void OnTriggerEnter(Collider other, Collider local)
        {
            if (other.GetComponent<Player>() is null)
            {
                return;
            }
            // everything that touches this zone gets relocated to the spawn point
            other.Entity.Position = _respawnPoint;
        }

        public void OnTriggerExit(Collider other, Collider local)
        {
        }
    }
}