using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName ="角色控制",menuName ="角色/控制")]
public class PlayerInput : ScriptableObject,Controls.IPlayerActions
{
    public UnityAction<float> onMove;
    public UnityAction onDisMove;

    public void OnFire(InputAction.CallbackContext context)
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onMove?.Invoke(context.ReadValue<float>());
        }
        else if (context.phase == InputActionPhase.Canceled)
            onDisMove?.Invoke();

    }

    #region control变量
    Controls control;
    public void EnablePlayerControl()
    {
        control.Player.Enable();
    }
    public void DisablePlayerControl()
    {
        control.Player.Disable();
    }
    #endregion

    private void OnEnable()
    {
        control = new Controls();
        control.Player.SetCallbacks(this);
        EnablePlayerControl();
    }
    private void OnDisable()
    {
        DisablePlayerControl();
    }

    public void OnSs(InputAction.CallbackContext context)//按W测试
    {
        Debug.Log(context);
    }
}
