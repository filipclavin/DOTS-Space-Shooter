using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemyMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();

        new EnemyMoveJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = SystemAPI.GetComponentRO<LocalTransform>(playerEntity).ValueRO.Position,
        }.ScheduleParallel();
    }
}

[BurstCompile]
[WithAll(typeof(EnemyTag))]
public partial struct EnemyMoveJob : IJobEntity
{
    public float DeltaTime;
    public float3 PlayerPosition;

    [BurstCompile]
    public void Execute(Entity entity, ref LocalTransform transform, in MovementComponent movement)
    {
        var directionToPlayer = math.normalizesafe(PlayerPosition - transform.Position);
        transform.Rotation = math.slerp(transform.Rotation, quaternion.LookRotation(math.forward(), directionToPlayer), movement.TurnSpeed * DeltaTime);
        transform.Position += DeltaTime * movement.MovementSpeed * directionToPlayer;
    }
}
