using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerShootingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputComponent>();
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var input = SystemAPI.GetSingleton<InputComponent>();
        if (input.MouseTriggered)
        {
            var player = SystemAPI.GetSingletonEntity<PlayerTag>();
            var localTransform = SystemAPI.GetComponentRO<LocalTransform>(player);
            var shootComp = SystemAPI.GetComponentRO<ShootComponent>(player);

            var bullet = state.EntityManager.Instantiate(shootComp.ValueRO.BulletPrefab);
            var bulletLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(bullet);
            bulletLocalTransform.ValueRW.Position = localTransform.ValueRO.Position;
            bulletLocalTransform.ValueRW.Rotation = localTransform.ValueRO.Rotation;
        }
    }
}