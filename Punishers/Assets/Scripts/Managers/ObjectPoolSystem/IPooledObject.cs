public interface IPooledObject
{
    int PoolId { get; set; }
    void OnObjectSpawn();
}