using System;
using Enemies;
using Gameplay.Combat.Spells;
using Gameplay.Rhythm;
using Gameplay.Spawning;
using PlayerScripts;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Private Fields

    [SerializeField] private GameObject gameplayWindow;
    [SerializeField] private GameObject pauseWindow;
    
    [Header("Gameplay Related Stuff")]
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCam;
    [SerializeField] private DialogBox dialogBox;
    [SerializeField] private AudioSource ambienceSound;
    
    [Header("Base Managers")]
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private HudManager hudManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SpellManager spellManager;

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
     public SpellManager SpellManager => spellManager;
     public ArenaManager ArenaManager => arenaManager;
     public EnemySpawner EnemySpawner => enemySpawner;
     public WaveManager WaveManager => waveManager;
     
     public bool IsPaused { get; private set; }
     public AudioSource AmbienceSound => ambienceSound;

    #endregion

    public Action Save;
    public Action Load;
 
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else 
            Instance = this;
    }

    private void Start()
    {
        Load.Invoke();
    } 
    
    public void ToggleWindows()
    {
        gameplayWindow.SetActive(!gameplayWindow.activeSelf);
        pauseWindow.SetActive(!pauseWindow.activeSelf);

        IsPaused = !IsPaused;
        
        if(!IsPaused)
            AudioManager.RestartSong();
    }

    public void SaveAndQuit()
    {
        Save.Invoke();
        SceneManager.LoadScene(0);
    }
}