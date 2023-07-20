using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public EnemyProducer enemyProducer;
    public GameObject playerPrefab;

    public TMP_Text winText;
    public TMP_Text countText;
    public int enemyDeaths;

    void Start()
    {
        enemyDeaths = 0;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.onPlayerDeath += onPlayerDeath;
        countText.SetText(GetKillCountText());
    }

    void onPlayerDeath(Player player)
    {
        enemyProducer.SpawnEnemies(false);
        Destroy(player.gameObject);

        Invoke("restartGame", 3);
    }

    void StopEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        var enemyProducer = GameObject.Find("EnemyProducer");
        Destroy(enemyProducer);
    }

    public void OnEnemyDeath()
    {
        enemyDeaths++;
        countText.SetText(GetKillCountText());
        if (enemyDeaths >= 5)
        {
            winText.text = "Wow! Hopi Wins!";
            StopEnemies();
        }
    }

    String GetKillCountText()
    {
        return "Kill count: " + enemyDeaths;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void restartGame()
    {
        enemyDeaths = 0;
        winText.text = "";
        countText.text = "";

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        var playerObject = Instantiate(playerPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
        var cameraRig = Camera.main.GetComponent<CameraRig>();
        cameraRig.target = playerObject;
        enemyProducer.SpawnEnemies(true);
        playerObject.GetComponent<Player>().onPlayerDeath += onPlayerDeath;
    }
}
