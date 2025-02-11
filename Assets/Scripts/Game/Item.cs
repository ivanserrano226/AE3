using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private AudioClip _pickupSound;
    public abstract void Use();

    protected virtual void Update()
    {
        //Rotate the item
        transform.Rotate(Vector3.up * Time.deltaTime * 100f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlaySound(_pickupSound);
            // Add powerup to player
            Use();
            // Destroy the item
            Destroy(gameObject);
        }
    }
}
