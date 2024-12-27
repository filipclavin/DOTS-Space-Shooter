using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BulletSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach ((var localTransform, var bullet, var entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BulletComponent>>().WithEntityAccess())
        {
            bullet.ValueRW.RemainingLifeTime -= SystemAPI.Time.DeltaTime;

            if (bullet.ValueRO.RemainingLifeTime <= 0)
            {
                ecb.DestroyEntity(entity);
            }

            localTransform.ValueRW.Position += bullet.ValueRO.Speed * SystemAPI.Time.DeltaTime * localTransform.ValueRO.Up();
        }
    }
}