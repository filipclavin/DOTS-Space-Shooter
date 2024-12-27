using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float BaseSpeed = 5f;
    public float TurnSpeed = 5f;

    class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new EnemyTag());
            AddComponent(entity, new MovementComponent()
            {
                MovementSpeed = authoring.BaseSpeed,
                TurnSpeed = authoring.TurnSpeed
            });
        }
    }
}

public struct EnemyTag : IComponentData { }

public struct MovementComponent : IComponentData
{
    public float MovementSpeed;
    public float TurnSpeed;
}