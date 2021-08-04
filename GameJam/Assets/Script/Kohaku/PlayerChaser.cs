using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseSystem.Property;

public class PlayerChaser : MonoBehaviour
{
    [SerializeField] int speed;

    GameObject tower;
    GameObject target;
    public bool OnChasePlayer;

    public void SetTowerGameObject(GameObject obj)
    {
        tower = obj;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && OnChasePlayer == false)
        {
            target = collision.gameObject;
            OnChasePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && OnChasePlayer == true && target == collision)
        {
            OnChasePlayer = false;
        }
    }

    private void Update()
    {
        if(OnChasePlayer)
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        else
        transform.position = Vector2.MoveTowards(transform.position, tower.transform.position, speed * Time.deltaTime);

    }
}
