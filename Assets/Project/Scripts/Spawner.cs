using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnedObject;

    List<float> _spawnRandoms = null;
    List<bool> _qbitStateRandoms = null;

    public QASMRandomProvider _randomProvider;

    private float _timeDiscount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _randomProvider = FindObjectOfType<QASMRandomProvider>();
        _randomProvider.GenerateFloatPool(128, (fpool) => {
            _randomProvider.GenerateBoolPool(128, (bpool) => {
                _timeDiscount = 0;
                _qbitStateRandoms = bpool;
                _spawnRandoms = fpool;
                StartCoroutine(SpawnPeriodically());
            });
        });
    }

    private void Update() {
        _timeDiscount += Time.deltaTime / 10;
        if (Qbit.currentQBits > 10) {
            Score.instance.SetLoose();
        }
        if (_spawnRandoms != null && _spawnRandoms.Count < 3 && Qbit.currentQBits == 0) {
            Score.instance.SetWin();
        }
    }


    public IEnumerator SpawnPeriodically() {
        while (_spawnRandoms.Count >= 3) {
            yield return new WaitForSeconds(_spawnRandoms[0] * Mathf.Max(0.5f, 4f - _timeDiscount) + Mathf.Max(0, 1f - _timeDiscount));
            _spawnRandoms.RemoveAt(0);

            GameObject go = Instantiate(spawnedObject);
            go.GetComponent<Qbit>().SetState(_qbitStateRandoms[0], _qbitStateRandoms[1]);
            _qbitStateRandoms.RemoveAt(0);
            _qbitStateRandoms.RemoveAt(0);

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
