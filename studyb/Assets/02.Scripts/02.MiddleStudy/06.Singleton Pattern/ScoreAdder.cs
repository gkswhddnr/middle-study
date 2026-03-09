using UnityEngine;

public class ScoreAdder : MonoBehaviour
{

    private void Awake()
    {
        Debug.Log("Start Score" + ScoreManager.GetInstance().GetScore());
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ScoreManager.GetInstance().AddScore(5);
            Debug.Log(ScoreManager.GetInstance().GetScore());
        }
    }
}
