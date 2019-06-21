using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnedObject;

    List<float> _spawnRandoms = null;

    public QASMRandomProvider _randomProvider;

    // Start is called before the first frame update
    void Start()
    {
        _randomProvider = FindObjectOfType<QASMRandomProvider>();
        _randomProvider.GenerateFloatPool(100, (pool) => {
            _spawnRandoms = pool;
            StartCoroutine(SpawnPeriodically());
        });
    }


    public IEnumerator SpawnPeriodically() {
        while (_spawnRandoms.Count >= 2) {
            yield return new WaitForSeconds(_spawnRandoms[0] * 4f + 1);
            _spawnRandoms.RemoveAt(0);
            GameObject go = Instantiate(spawnedObject);
            go.transform.localPosition = SamplePositionXZ(-1f, Vector2.one * -4, Vector2.one * 4);
            
        }
    }

    public Vector3 SamplePositionXZ(float height, Vector2 min, Vector2 max) {
        Vector2 diff = max - min;

        float x = _spawnRandoms[0] * diff.x + min.x;
        float y = _spawnRandoms[1] * diff.y + min.y;

        _spawnRandoms.RemoveRange(0, 2);

        return new Vector3(x, height, y);
    }
}
