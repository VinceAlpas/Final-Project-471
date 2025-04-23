using UnityEngine;

[CreateAssetMenu(fileName = "NewCustomerOrder", menuName = "Customer/Order")]
public class CustomerOrderSO : ScriptableObject
{
    public string orderName;
    public Sprite orderIcon; // For thought bubble
    public ItemSO requiredItem; // The actual inventory item needed
}
