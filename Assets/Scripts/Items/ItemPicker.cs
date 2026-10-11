using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private string dropItemTag;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(dropItemTag))
        {
            DropItem dropItem = other.GetComponent<DropItem>();
            player.Heal(dropItem.GetHealPoint);
            Destroy(other.gameObject);
        }
    }
}
