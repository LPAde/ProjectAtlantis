using PlayerScripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Private Fields
    
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private Player player;

    #endregion

    public RhythmManager RhythmManager => rhythmManager;
    public Player Player => player;

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else 
            Instance = this;
    }
}
