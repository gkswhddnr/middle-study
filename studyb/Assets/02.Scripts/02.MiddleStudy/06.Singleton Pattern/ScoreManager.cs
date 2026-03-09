using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ScoreManager>();

            if (instance == null)
            {
                GameObject container = new GameObject(); // 일반적으로 게임오브젝트는 인스턴시에이트로 찍어내지만 빈 게임오브젝트는 new로 찍어낼 수 있다.

                 instance = container.AddComponent<ScoreManager>();
            }


        }
        return instance;
    }
    private static ScoreManager instance;
    private int score = 0;

    public int GetScore()
    {
        return score;
    }
    public void AddScore(int newScore)
    {
        score = score + newScore;
    }



    void Start()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
