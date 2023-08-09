using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBoost : MonoBehaviour
{
    private enum Direction
    {
        Up, Down, Left, Right
    }

    private static float speed = 1;
    private static float lifeTime = 5;

    private float lifeCounter = 0;

    [SerializeField] private Direction direction;

    private Vector3 startingPos;

    public Vector2 force { get; private set; }

    public bool canUse = true;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        startingPos = transform.position;

        switch (direction)
        {
            case Direction.Left:
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 90);
                    force = Vector2.left;
                    break;
                }
            case Direction.Right:
                {
                    transform.localRotation = Quaternion.Euler(0, 0, -90);
                    force = Vector2.right;
                    break;
                }
            case Direction.Up:
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    force = Vector2.up;
                    break;
                }
            case Direction.Down:
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 180);
                    force = Vector2.down;
                    break;
                }
        }

        ResetBoost();
    }

    void Update()
    {
        if (GameManager.isGameActive)
        {
            if (lifeCounter < lifeTime)
            {
                switch (direction)
                {
                    case Direction.Left:
                        {
                            transform.position += new Vector3(-speed * Time.deltaTime, 0);
                            break;
                        }
                    case Direction.Right:
                        {
                            transform.position += new Vector3(speed * Time.deltaTime, 0);
                            break;
                        }
                    case Direction.Up:
                        {
                            transform.position += new Vector3(0, speed * Time.deltaTime);
                            break;
                        }
                    case Direction.Down:
                        {
                            transform.position += new Vector3(0, -speed * Time.deltaTime);
                            break;
                        }
                }
            }
            else
            {
                ResetBoost();
            }

            lifeCounter += Time.deltaTime;

            if (canUse)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.8f);
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.2f);
            }
        }
    }
    private void ResetBoost()
    {
        transform.position = startingPos;
        lifeCounter = 0;
        canUse = true;
        AudioManager.PlaySound("Boost Spawned");
    }
}
