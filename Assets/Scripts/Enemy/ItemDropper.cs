using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private GameObject dropItemPrefab;

    public void DropItem()
    {
        Instantiate(dropItemPrefab, transform.position + Vector3.up * 0.3f, Quaternion.identity);
    }
}
