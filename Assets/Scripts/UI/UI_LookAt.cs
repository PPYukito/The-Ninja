using System.Runtime.CompilerServices;
using UnityEngine;

public class UI_LookAt : MonoBehaviour
{
    [SerializeField] private GameObject trackObject;

    private void Update()
    {
        transform.LookAt(trackObject.transform, Vector3.up);
    }
}
