using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool invincable = false;

    public float Speed = 1;

    public float speedAddition = 0;
    public float speedAdditionPerTick = 0.05f;

    float LeftX = -0.5f;
    float RightX = 0.5f;

    public bool right = false;

    public GameController gameController;

    void Update()
    {
        if (gameController.isClimbing)
        {
            speedAddition += speedAdditionPerTick;
        }

        foreach (Animator animator in gameController.CharacterAnimators)
        {
            if (animator.gameObject.activeSelf)
            {
                animator.SetBool("Right", right);
            }
            
        }

        if (right)
        {
            transform.position = new Vector3(RightX, transform.position.y, 0);
        }
        else
        {
            transform.position = new Vector3(LeftX, transform.position.y, 0);
        }

        if (gameController.isClimbing)
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * (Speed + speedAddition);   
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void ButtonClick()
    {
        gameController.PlaySound("Switch", 1);
        right = !right;
    }
}
