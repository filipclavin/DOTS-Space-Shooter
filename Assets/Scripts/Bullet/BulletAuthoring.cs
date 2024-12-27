using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float speed;
    public float lifeTime;

    class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BulletComponent { Speed = authoring.speed, RemainingLifeTime = authoring.lifeTime });
        }
    }
}

public struct BulletComponent : IComponentData
{
    public float Speed;
    public float RemainingLifeTime;
}