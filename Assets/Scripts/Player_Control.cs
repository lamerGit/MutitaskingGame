using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player_Control : MonoBehaviour
{
    PlayerInput inputActions;
    Player player;
    Vector3 lookDir;
    Vector3 targetPos;
    Vector2 mousePos;

    bool isLeftClick = false;

    GameObject targetItem;

    private void Awake()
    {
        inputActions = new PlayerInput();

    }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        mousePos = Mouse.current.position.ReadValue();
        if (isLeftClick)
        {
            mouseRay();
            ItemRay();
            player.MovePlayer(targetPos);
            player.TurnPlayer(lookDir);

        }
        player.DistaceAcess(targetPos);

    }


    private void OnEnable()
    {
        inputActions.Enable();
        //inputActions.Player.Look.performed += OnLook;
        inputActions.Player.LeftClick.performed += OnLeftClick;
        inputActions.Player.LeftClick.canceled += OnLeftClick;
        
    }

    private void OnDisable()
    {
        inputActions.Player.LeftClick.canceled -= OnLeftClick;
        inputActions.Player.LeftClick.performed -= OnLeftClick;
        //inputActions.Player.Look.performed -= OnLook;
        inputActions.Disable();
    }

    private void OnLeftClick(InputAction.CallbackContext obj)
    {
        if(obj.performed && !IsPointerOverUIObject())
        {

            mouseRay();
            player.CFXSet(targetPos);
            //player.IsLeftClick = true;
            player.DropItem();
            isLeftClick = true;

            
        }else
        {
            isLeftClick = false;
            //player.IsLeftClick = false;

        }
        
    }

    /// <summary>
    /// ���̸� ���� ���콺�� ���� Ŭ���ϴ��� Ȯ���ϴ� �Լ�
    /// </summary>
    private void mouseRay()
    {
        Vector2 sceenPos = mousePos;
        Ray ray = Camera.main.ScreenPointToRay(sceenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))
        {
            targetPos = hit.point;

            lookDir = hit.point - player.transform.position;
            lookDir.y = 0.0f;
            lookDir = lookDir.normalized;

        }
    }

    /// <summary>
    /// ���̸� ���� �������� �����ϴ� �Լ�
    /// </summary>
    private void ItemRay()
    {
        Vector2 sceenPos = mousePos;
        Ray ray = Camera.main.ScreenPointToRay(sceenPos);
        if (Physics.Raycast(ray, out RaycastHit hitItem, 1000.0f, LayerMask.GetMask("Item")))
        {
            //player.PickUpItem(hitItem.transform.gameObject);
            hitItem.transform.gameObject.GetComponent<Item>().OutlineOn();
            player.TargetItem = hitItem.transform.gameObject;


        }else
        {
            player.TargetItem = null;
        }
    }

    private void OnLook(InputAction.CallbackContext obj)
    {
       

    }

    /// <summary>
    /// ���콺Ŭ���� UI���� object���� Ȯ������ ����
    /// </summary>
    /// <returns>true�̸� UI���� false�� UI����</returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = mousePos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;

    }
}