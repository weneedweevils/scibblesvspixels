using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarBehaviour : MonoBehaviour
{
    public Slider healthBar;
    public Color low;
    public Color high;
    public Vector3 offset;
    public float xScale;
    public float yScale;

    public void SetHealth(float health, float maxHealth)
    {
        healthBar.gameObject.SetActive(health < maxHealth && health > 0);
        healthBar.value = health;
        healthBar.maxValue = maxHealth;

        // Dynamically changes the color of the healthbar based on remaining health
        healthBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, healthBar.normalizedValue);
    }

    // Start is called before the first frame update
    public void Start()
    {
        // Sets health bar size
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(xScale, yScale, 1);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public void Disable()
    {
        healthBar.gameObject.SetActive(false);
    }
}
