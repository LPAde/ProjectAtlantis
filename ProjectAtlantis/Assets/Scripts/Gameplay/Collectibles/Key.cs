namespace Gameplay.Collectibles
{
    public class Key : BaseItem
    {
        protected override void Update()
        {
        }

        protected override void OnCollecting()
        {
            base.OnCollecting();

            int currentlyCollectedKeys = SaveSystem.GetInt("UsedKeys");
            currentlyCollectedKeys++;
            SaveSystem.SetInt("UsedKeys", currentlyCollectedKeys);
            GameManager.Instance.WaveManager.DestroyWall(currentlyCollectedKeys);
        }
    }
}