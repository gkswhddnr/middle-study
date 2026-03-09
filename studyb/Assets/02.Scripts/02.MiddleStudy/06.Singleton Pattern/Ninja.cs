using UnityEngine;
//싱글톤을 구현하는 핵심 아이디어 스크립트
public class Ninja : MonoBehaviour
{
    public static Ninja ninjaKing;
    public string ninjaName;
    public bool isKing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isKing) 
        {
            ninjaKing = this;
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("My Name:" + ninjaName + ", 닌자왕은" + ninjaKing);
    }
}
