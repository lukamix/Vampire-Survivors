using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : DucSingleton<GameManager>
{
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private EnemiesManager enemiesManager;
    [SerializeField] private PlayerManager playerManager;
    public DamageNumber damageNumber;

    [HideInInspector] public int gameTime = 0;
    private readonly int MAX_GAME_TIME = 300;
    private readonly int SPAWN_BOSS_TIME_OFFSET = 60;
    private bool isWaiting = false;
    private bool isBossTime = false;
    private void Start()
    {
        SceneManager.LoadScene("GameCanvas", LoadSceneMode.Additive);
        mapGenerator.GenerateMap();
        playerManager.PlayerManagerStart();
        enemiesManager.player = playerManager.character;
        enemiesManager.EnemiesManagerStart(mapGenerator);
        Observer.Instance.AddObserver(ObserverKey.EnemyDead,OnEnemyDead);
        Observer.Instance.AddObserver(ObserverKey.BossTime, DoOnBossTime);
        Observer.Instance.AddObserver(ObserverKey.GameOver, GameOver);
        DucSoundManager.Instance.PlayMusic("bgm_will_beginning");
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Observer.Instance.RemoveObserver(ObserverKey.EnemyDead, OnEnemyDead);
        Observer.Instance.RemoveObserver(ObserverKey.BossTime, DoOnBossTime);
        Observer.Instance.RemoveObserver(ObserverKey.GameOver, GameOver);
    }
    private void Update()
    {
        CheckEscapeButton();
        if (!isWaiting &&!isBossTime)
        {
            StartCoroutine(CountDown());
        }
    }
    private void OnEnemyDead(object data)
    {
        CanvasManager.Instance.enemyCounter.IncreaseEnemyDead();
    }
    private void CheckEscapeButton()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            PopUpSettings ps = PanelManager.Show<PopUpSettings>();
            ps.ActiveQuitButtonForIngame(true);
        }
    }
    IEnumerator CountDown()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1);
        gameTime++;
        CanvasManager.Instance.timeCounter.SetTimeText(gameTime);
        if(gameTime == MAX_GAME_TIME)
        {
            Victory();
        }
        if (gameTime % SPAWN_BOSS_TIME_OFFSET == 0)
        {
            isBossTime = true;
            Observer.Instance.Notify(ObserverKey.BossTime,true);
        }

        isWaiting = false;
    }
    private void DoOnBossTime(object data)
    {
        bool _isBossTime = (bool)data;
        if (!_isBossTime)
        {
            DucSoundManager.Instance.PlayMusic("bgm_will_beginning");
            CanvasManager.Instance.timeCounter.SetEnemyTime();
            isBossTime = false;
        }
        else
        {
            DucSoundManager.Instance.PlayMusic("bgm_elrond_boss0");
            CanvasManager.Instance.timeCounter.SetBossTime();
        }
    }
    private void GameOver(object data)
    {
        DucSoundManager.Instance.StopMusic();
        DucSoundManager.Instance.PlaySoundFx("sfx_gameOver");
        PanelManager.Show<PopUpGameOver>();
        Pause();
    }
    private void Victory()
    {
        StartCoroutine(IEViectory());
    }
    IEnumerator IEViectory()
    {
        enemiesManager.ClearAllEnemy();
        yield return new WaitForSeconds(3);
        DucSoundManager.Instance.StopMusic();
        DucSoundManager.Instance.PlaySoundFx("sfx_victory2");
        PanelManager.Show<PopUpVictory>();
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Continue()
    {
        Time.timeScale = 1;
    }
}
