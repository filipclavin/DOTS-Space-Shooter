
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerMovementSystem : ISystem
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
        var player = SystemAPI.GetSingletonEntity<PlayerTag>();
        var movement = SystemAPI.GetComponentRW<MovementComponent>(player);
        var input = SystemAPI.GetSingleton<InputComponent>();
        var localTransform = SystemAPI.GetComponentRW<LocalTransform>(player);

        float3 playerPosition = localTransform.ValueRO.Position;
        float3 direction = math.normalize(new float3(input.MousePos, 0) - playerPosition);
        quaternion rotation = quaternion.LookRotation(math.forward(), direction);
        localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation, rotation, movement.ValueRO.TurnSpeed * SystemAPI.Time.DeltaTime);

        localTransform.ValueRW.Position += movement.ValueRO.MovementSpeed * SystemAPI.Time.DeltaTime * localTransform.ValueRO.Up();
    }
}