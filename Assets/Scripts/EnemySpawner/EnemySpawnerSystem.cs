using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public partial struct EnemySpawnerSystem : ISystem
{
    private Entity spawner;
    private Random RandomGen;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemySpawnerComponent>();
        RandomGen = Random.CreateFromIndex(1);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spawner = SystemAPI.GetSingletonEntity<EnemySpawnerComponent>();
        var spawnerComponent = SystemAPI.GetComponentRW<EnemySpawnerComponent>(spawner);
        ref BlobArray<EnemyWave> waves = ref spawnerComponent.ValueRO.WavesBlob.Value.Waves;

        if (spawnerComponent.ValueRO.WaveDelayTimer > 0)
        {
            spawnerComponent.ValueRW.WaveDelayTimer -= SystemAPI.Time.DeltaTime;
            return;
        }

        if (spawnerComponent.ValueRO.SpawnTimer > 0)
        {
            spawnerComponent.ValueRW.SpawnTimer -= SystemAPI.Time.DeltaTime;
            return;
        }

        EntityCommandBuffer ecb = new(Allocator.Temp);
        Entity enemy = ecb.Instantiate(spawnerComponent.ValueRO.EnemyPrefab);

        float3 position = new float3(RandomGen.NextFloat2Direction(), 0) * spawnerComponent.ValueRO.SpawnRange;

        float3 playerPosition = SystemAPI.GetComponentRO<LocalTransform>(spawnerComponent.ValueRO.Player).ValueRO.Position;
        float3 direction = math.normalize(playerPosition - position);
        quaternion rotation = quaternion.LookRotation(math.forward(), direction);

        ecb.SetComponent(
            enemy,
            LocalTransform.FromPositionRotation(position, rotation)
        );

        spawnerComponent.ValueRW.EnemiesLeftToSpawn--;
        if (spawnerComponent.ValueRO.EnemiesLeftToSpawn == 0)
        {
            spawnerComponent.ValueRW.CurrentWaveIndex++;
            if (spawnerComponent.ValueRO.CurrentWaveIndex == waves.Length)
                spawnerComponent.ValueRW.CurrentWaveIndex = 0;

            spawnerComponent.ValueRW.WaveDelayTimer = waves[spawnerComponent.ValueRO.CurrentWaveIndex].Delay;
            spawnerComponent.ValueRW.EnemiesLeftToSpawn = waves[spawnerComponent.ValueRO.CurrentWaveIndex].EnemyCount;
        }

        spawnerComponent.ValueRW.SpawnTimer = waves[spawnerComponent.ValueRO.CurrentWaveIndex].SpawnInterval;

        ecb.Playback(state.EntityManager);
    }
}
