using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private Animator anim;
    private Inventory inventory;
    private Functionality currentFunction;
    private WaitForSeconds takeCooldown;
    private bool isWorking = false;
    private bool isProcessing = false;
    private bool canPut = true;
    private void Awake()
    {
        canPut = true;
        anim = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        takeCooldown = new WaitForSeconds(0.5f);
    }
    private void Update()
{
    // Button A (joystick button 0) - equivalent to left mouse button
    if (Input.GetKeyDown(KeyCode.JoystickButton0))
    {
        DoAction();
    }
    else if (Input.GetKey(KeyCode.JoystickButton0))
    {
        isWorking = true;
        if (!isProcessing)
        {
            StartProcessAction();
        }
        else
        {
            DoProcessAction();
        }
    }
    else if (Input.GetKeyUp(KeyCode.JoystickButton0))
    {
        isWorking = false;
        if (isProcessing)
        {
            currentFunction?.ResetTimer();
            isProcessing = false;
        }
    }

    // Right Trigger for TakeAction (axis value > 0.5)
    if (Input.GetAxis("Fire1") > 0.5f) // Or replace with custom axis
    {
        DoTakeAction();
    }
}

    private void DoAction()
    {
        anim.SetTrigger("Take");
    }
    private void StartProcessAction()
    {
        Ray ray = new Ray(transform.position + Vector3.up / 2, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1))
        {
            if (hit.collider.TryGetComponent<Functionality>(out Functionality itemProcess))
            {
                isProcessing = true;
                currentFunction = itemProcess;
            }
        }
    }
    private void DoProcessAction()
    {
        if (!isProcessing) return;
        if (!isWorking) return;
        ItemType item = currentFunction.Process();
        if (item != ItemType.NONE)
        {
            currentFunction.ClearObject();
            inventory.TakeItem(item);
            isWorking = false;
        }
    }
    public void DoTakeAction()
    {
        Ray ray = new Ray(transform.position + Vector3.up / 2, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1))
        {
            if (hit.collider.TryGetComponent<IPutItemFull>(out IPutItemFull itemPutBox))
            {
                if (canPut)
                {
                    bool status = itemPutBox.PutItem(inventory.GetItem());
                    if (status == true)
                    {
                        inventory.PutItem();
                        inventory.ClearHand();
                    }
                }
            }
            if (hit.collider.TryGetComponent<ItemBox>(out ItemBox itemBox))
            {
                inventory.TakeItem(itemBox.GetItem());
                StartCoroutine(canPutCoolDown());
            }
            
        }
        

    }
    private void OnTriggerStay(Collider other)
    {
        if (inventory.CurrentType != ItemType.HAMBURGER) return;
        if (other.gameObject.CompareTag("SellArea"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                CustomerManager.Instance.SellToCustomer();
                inventory.ClearHand();
            }
        }
    }
    private IEnumerator canPutCoolDown()
    {
        canPut = false;
        yield return takeCooldown;
        canPut = true;
    }
} 
