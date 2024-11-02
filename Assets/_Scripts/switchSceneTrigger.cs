//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class switchSceneTrigger : MonoBehaviour
//{
//    LevelManager levelManager;

//    [SerializeField]
//    private string sceneName;

//    private void Start()
//    {
//        levelManager = FindAnyObjectByType<LevelManager>();
//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.tag == "Player")
//        {
//            levelManager.SwitchScene(sceneName);
//        }
//    }
//}
