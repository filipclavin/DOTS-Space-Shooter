using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float Speed = 5f;
    public float TurnSpeed = 5f;
    public GameObject BulletPrefab;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerTag());
            AddComponent(entity, new MovementComponent()
            {
                MovementSpeed = authoring.Speed,
                TurnSpeed = authoring.TurnSpeed
            });
            AddComponent(entity, new ShootComponent { BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic) });
        }
    }
}

public struct PlayerTag : IComponentData {}

public struct ShootComponent : IComponentData
{
    public Entity BulletPrefab;
}