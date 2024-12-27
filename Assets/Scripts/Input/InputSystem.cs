using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private Controls controls;

    override protected void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton<InputComponent>(out _))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }
        controls = new Controls();
        controls.Enable();
    }

    override protected void OnUpdate()
    {
        Vector2 mousePos = controls.Main.Move.ReadValue<Vector2>();
        float3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

        SystemAPI.SetSingleton(new InputComponent
        {
            MousePos = new float2(mouseWorldPos.x, mouseWorldPos.y),
            MouseTriggered = controls.Main.Shoot.WasPressedThisFrame()
        });
    }
}
