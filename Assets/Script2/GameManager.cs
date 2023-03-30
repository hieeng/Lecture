using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    public MainCamera mainCamera;
    public UIManager uIManager;
    public SoundManager soundManager;
    public GameObject timeLine;
    public GameObject enemys;
    public int EnemyCount;
    public int EnemyKillCount;

    private void Awake()
    {
        instance = this;
        EnemyCount = enemys.transform.childCount;
    }
}
