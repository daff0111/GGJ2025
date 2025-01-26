using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DarkShard : MonoBehaviour
{
    NPCController nPCController;
    public Bubblemon darkShardBubblemon;
    public AudioClip battleAudio;

    // Start is called before the first frame update
    void Start()
    {
        nPCController = GetComponent<NPCController>();
        nPCController.OnInteractEnded += OnDarkShardAwakened;
    }

    void OnDarkShardAwakened()
    {
        darkShardBubblemon.Init();
        GameController.Instance.StartBattle(darkShardBubblemon);
        GameController.Instance.PlayMusic(battleAudio);
        GameController.Instance.OnBattleEnded += OnBattleEnded;
    }

    void OnBattleEnded()
    {
        SceneManager.LoadScene("Scenes/MyScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
