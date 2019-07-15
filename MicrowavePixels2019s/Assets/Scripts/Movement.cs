using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float speed;
    public float MaxPlayerSize;
    public float MinPlayerSize;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 Direction = new Vector2(x, y).normalized;
        Move(Direction);
    }

    void Move(Vector2 direction)
    {
        //make sure it does not go pass the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        max.x = max.x - MaxPlayerSize;
        min.x = min.x + MaxPlayerSize;


        max.y = max.y - MinPlayerSize;
        min.y = min.y + MinPlayerSize;
        //get current position
        Vector2 pos = transform.position;
        //make new position
        pos += direction * speed * Time.deltaTime;
        //again make sure new poss does not pass the screen
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        //applay new position
        transform.position = pos;



    }
}
