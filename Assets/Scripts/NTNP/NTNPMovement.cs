using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NTNPMovement : MonoBehaviour {
    [SerializeField] private float gridSize;
    [SerializeField] private float moveSpeed;

    private Rigidbody rb;
    private PlayerDeath deathScript;

    private int direction;
    private Vector2 move;
    private Vector3 moveStartPoint;
    private Vector3 moveEndPoint;
    private Vector3 lastPosition;
    private int stuckTick;

    // Read by player animation controller
    [HideInInspector] public Directions movingDirection { get; private set; } = Directions.None;
    [HideInInspector]
    public enum Directions {
        None,
        Up,
        Right,
        Down,
        Left
    }
    [HideInInspector] public int turnAmount { get; private set; } = 0;

    private void OnSwipe(InputValue input) {
        move = input.Get<Vector2>();
    }
    private void OnClick(InputValue input) {
        if (input.isPressed) {
            move = new Vector2(0, 1);
        }
    }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        deathScript = GetComponent<PlayerDeath>();
    }

    private void FixedUpdate() {
        if (deathScript.alive) {
            WhileAlive();
        }
    }

    private void WhileAlive() {
        float deadzone = 0.5f;
        Vector3 vel = rb.velocity;
        Vector3 pos = transform.position;

        if (movingDirection == Directions.None) {
            turnAmount = 0;
            if (move.x > deadzone) {
                movingDirection = Directions.Right;
            }
            else if (move.x < -deadzone) {
                movingDirection = Directions.Left;
            }
            else if (move.y > deadzone) {
                movingDirection = Directions.Up;
            }
            else if (move.y < -deadzone) {
                movingDirection = Directions.Down;
            }

            if (movingDirection != Directions.None) {
                int newDirection = (int)movingDirection - 1;
                turnAmount = DifferenceWithLoop(direction, newDirection, 4);
                if (turnAmount == -2) turnAmount = 2;
                direction = newDirection;

                Vector2Int directionVec = DirectionToVector(movingDirection);

                moveStartPoint = SnapToGrid(transform.position);
                moveEndPoint = new Vector3(
                    moveStartPoint.x + (directionVec.x * gridSize),
                    transform.position.y,
                    moveStartPoint.z + (directionVec.y * gridSize)
                );
                move = Vector2.zero;
            }
        }

        if (movingDirection != Directions.None) {
            if (Vector3.Distance(pos, lastPosition) < 0.05f) {
                if (stuckTick == 5) {
                    if (movingDirection == Directions.Up) {
                        movingDirection = Directions.Down;
                    }
                    else if (movingDirection == Directions.Down) {
                        movingDirection = Directions.Up;
                    }
                    else if (movingDirection == Directions.Left) {
                        movingDirection = Directions.Right;
                    }
                    else {
                        movingDirection = Directions.Left;
                    }
                    turnAmount = 2;
                    direction = (int)movingDirection - 1;

                    moveEndPoint = moveStartPoint;
                    moveStartPoint = SnapToGrid(transform.position);

                    stuckTick = 0;
                }
                else {
                    stuckTick++;
                }
            }
            else {
                stuckTick = 0;
            }


            if (movingDirection == Directions.Right) {
                if (pos.x > moveEndPoint.x) {
                    pos.x = moveEndPoint.x;
                    vel.x = 0;
                    movingDirection = Directions.None;
                }
                else {
                    vel.x += moveSpeed;
                }
            }
            else if (movingDirection == Directions.Left) {
                if (pos.x < moveEndPoint.x) {
                    pos.x = moveEndPoint.x;
                    vel.x = 0;
                    movingDirection = Directions.None;
                }
                else {
                    vel.x -= moveSpeed;
                }
            }
            else if (movingDirection == Directions.Up) {
                if (pos.z > moveEndPoint.z) {
                    pos.z = moveEndPoint.z;
                    vel.z = 0;
                    movingDirection = Directions.None;
                }
                else {
                    vel.z += moveSpeed;
                }
            }
            else {
                if (pos.z < moveEndPoint.z) {
                    pos.z = moveEndPoint.z;
                    vel.z = 0;
                    movingDirection = Directions.None;
                }
                else {
                    vel.z -= moveSpeed;
                }
            }
        }

        rb.velocity = vel;
        lastPosition = pos;
    }

    private Vector2Int DirectionToVector(Directions direction) {
        if (direction == Directions.Up) {
            return Vector2Int.up;
        }
        else if (direction == Directions.Down) {
            return Vector2Int.down;
        }
        else if (direction == Directions.Left) {
            return Vector2Int.left;
        }
        return Vector2Int.right;
    }

    private Vector3 SnapToGrid(Vector3 position) {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            position.y,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }

    private int DifferenceWithLoop(int start, int end, int max) {
        // Screw it, I can spare a few CPU cycles. Though now I'm realising why the other way wasn't working
        int num = start;
        int distance = 0;
        while (num != end) {
            num++;
            if (num == max) num = 0;
            distance++;
        }


        num = start;
        int distance2 = 0;
        while (num != end) {
            if (num == 0) num = max - 1;
            else num--;
            distance2++;
        }

        return distance > distance2 ? -distance2 : distance;
    }

    public void OnRotateAnimationFinished() { // Called by the animation controller
        transform.rotation = Quaternion.identity;
        transform.Rotate(0, direction * 90, 0);
    }

    /*
    private int DifferenceWithLoop(int start, int end, int max) {
        //return AbsMin(end - start, (max - start) + end, -start - (max - end));
        return AbsMin(start - end, (start - max) - end, start - (max - end));
    }

    private int AbsMin(params int[] numbers) {
        int min = numbers[0];
        foreach (int num in numbers) {
            if (Mathf.Abs(num) < Mathf.Abs(min)) {
                min = Mathf.Abs(num);
			}
		}

        return min;
	}
    */
}
