using Enemies;
using Gameplay.Rhythm;
using Gameplay.Spawning;
using PlayerScripts;
using UI;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Private Fields
    
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCam;
    [SerializeField] private DialogBox dialogBox;
    
    [Header("Base Managers")]
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private HudManager hudManager;
    [SerializeField] private AudioManager audioManager;

    [Header("Arena Related Managers")] 
    [SerializeField] private ArenaManager arenaManager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private WaveManager waveManager;

    #endregion

    #region Properties

     public Player Player => player;
     public Camera MainCam => mainCam;
     public DialogBox DialogBox => dialogBox;
     public RhythmManager RhythmManager => rhythmManager;
     public EnemyManager EnemyManager => enemyManager;
     public HudManager HudManager => hudManager;
     public AudioManager AudioManager => audioManager;
     public ArenaManager ArenaManager => arenaManager;
     public EnemySpawner EnemySpawner => enemySpawner;
     public WaveManager WaveManager => waveManager;

    #endregion
    
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else 
            Instance = this;
    }
}