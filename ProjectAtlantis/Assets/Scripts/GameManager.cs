using Enemies;
using PlayerScripts;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Private Fields
    
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCam;
    

    #endregion

    public RhythmManager RhythmManager => rhythmManager;
    public EnemyManager EnemyManager => enemyManager;
    public Player Player => player;
    public Camera MainCam => mainCam;

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else 
            Instance = this;
    }
}
