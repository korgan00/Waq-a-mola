using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerVertexMove : MonoBehaviour {

    private Mesh _mesh;
    private Vector3[] _verticesSrc;
    private Vector3[] _verticesTarget;
    private Vector3[] _verticesLastPosition;
    private Vector3[] _currentVertices;

    private QASMRandomProvider _randomProvider;

    private float _currentTransition = 0;

    void Start() {
        _mesh = GetComponent<MeshFilter>().mesh;

        _verticesSrc = _mesh.vertices;
        _currentVertices = _mesh.vertices;
        _verticesTarget = new Vector3[_currentVertices.Length];
        _verticesLastPosition = new Vector3[_currentVertices.Length];

        _randomProvider = FindObjectOfType<QASMRandomProvider>();

        StartCoroutine(ChangeVerticesPosition());
    }

    public IEnumerator ChangeVerticesPosition() {
        bool requested = false;
        List<float> offsets = null;
        _randomProvider.GenerateFloatPool(_verticesTarget.Length * 6, (pool) => {
            offsets = pool;
        });

        while (true) {
            yield return new WaitWhile(() => offsets == null || offsets.Count < _verticesTarget.Length);

            _currentTransition = 0;
            for (int i = 0; i < _verticesTarget.Length; i++) {
                _verticesLastPosition[i] = _currentVertices[i];
                _verticesTarget[i] = _verticesSrc[i] + Vector3.up * offsets[0];
                offsets.RemoveAt(0);
            }

            if (!requested && offsets.Count < _verticesTarget.Length * 3) {
                requested = true;
                _randomProvider.GenerateFloatPool(_verticesTarget.Length * 6, (pool) => {
                    offsets.AddRange(pool);
                    requested = false;
                });
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void Update() {
        _currentTransition += Time.deltaTime;
        for (int i = 0; i < _currentVertices.Length; i++) {
            _currentVertices[i] = Vector3.Lerp(_verticesLastPosition[i], _verticesTarget[i], _currentTransition);
        }
        
        _mesh.vertices = _currentVertices;
        _mesh.RecalculateBounds();
    }
}
