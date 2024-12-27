using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct EnemyWaveAuthoring
{
    public float SpawnInterval;
    public int EnemyCount;
    public float Delay;
}

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public List<EnemyWaveAuthoring> EnemyWaves = new();
    public float SpawnRange = 5f;

    public GameObject Player;

    class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            if (authoring.EnemyWaves.Count == 0)
            {
                Debug.LogError("Add at least one enemy wave to the spawner");
                return;
            };

            Entity entity = GetEntity(TransformUsageFlags.None);

            BlobBuilder bb = new BlobBuilder(Allocator.Temp);
            ref WavesBlob wavesBlob = ref bb.ConstructRoot<WavesBlob>();
            BlobBuilderArray<EnemyWave> waves = bb.Allocate(ref wavesBlob.Waves, authoring.EnemyWaves.Count);

            for (int i = 0; i < authoring.EnemyWaves.Count; i++)
            {
                EnemyWaveAuthoring wave = authoring.EnemyWaves[i];
                waves[i] = new EnemyWave
                {

                    SpawnInterval = wave.SpawnInterval,
                    EnemyCount = wave.EnemyCount,
                    Delay = wave.Delay,
                };
            }

            var wavesBlobRef = bb.CreateBlobAssetReference<WavesBlob>(Allocator.Persistent);
            AddBlobAsset(ref wavesBlobRef, out var hash);

            EnemyWaveAuthoring firstWave = authoring.EnemyWaves[0];
            AddComponent(entity, new EnemySpawnerComponent()
            {
                EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
                WavesBlob = wavesBlobRef,
                SpawnRange = authoring.SpawnRange,

                Player = GetEntity(authoring.Player, TransformUsageFlags.WorldSpace),

                SpawnTimer = firstWave.SpawnInterval,
                WaveDelayTimer = firstWave.Delay,
                CurrentWaveIndex = 0,
                EnemiesLeftToSpawn = firstWave.EnemyCount
            });

            bb.Dispose();
        }
    }
}

public struct EnemyWave
{
    public int EnemyCount;
    public float SpawnInterval;
    public float Delay;
}

public struct WavesBlob
{
    public BlobArray<EnemyWave> Waves;
}

public struct EnemySpawnerComponent : IComponentData
{
    public Entity EnemyPrefab;
    public BlobAssetReference<WavesBlob> WavesBlob;
    public float SpawnRange;

    public Entity Player;

    public float SpawnTimer;
    public float WaveDelayTimer;
    public int CurrentWaveIndex;
    public int EnemiesLeftToSpawn;
}