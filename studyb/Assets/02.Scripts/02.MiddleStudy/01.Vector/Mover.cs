using UnityEngine;

// Translate 할떄는 기본적으로 자기 자신 local을 기준으로 한다.
public class Mover : MonoBehaviour
{
    public Vector3 move = new Vector3(-5, -5, -5);


    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(move * Time.deltaTime); // Space.World는 Global , Space.Self는 Local, Space = 기준 좌표계
    }
}
