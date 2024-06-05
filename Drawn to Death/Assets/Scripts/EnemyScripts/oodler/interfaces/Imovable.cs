using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Imovable
{
    Rigidbody2D Rigidbody { get; set; }
    SpriteRenderer BossSprite { get; set; }
    float MovementSpeed { get; set; }
    void MoveEnemy(Vector2 velocity);

}
