using UnityEngine;

public class Test : MonoBehaviour
{
    public BaseMonster[] monsters;

    public void Start()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            Debug.Log(monsters[i].gameObject.name);
        }
    }

}
