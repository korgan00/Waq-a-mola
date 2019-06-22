using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qbit : MonoBehaviour
{

    public Material x0;
    public Material x1;
    public Material z0;
    public Material z1;

    public static int currentQBits = 0;

    private QbitState _state = QbitState.X0;

    private bool _hasCollapsed = false;
    Animator _animator;
    MeshRenderer _meshRenderer;

    private void Start() {
        _animator = GetComponent<Animator>();
        _meshRenderer = GetComponent<MeshRenderer>();
        currentQBits++;
    }

    public void SetState(bool b, bool v) {
        if (b) {
            _state = v ? QbitState.X1 : QbitState.X0;
        } else {
            _state = v ? QbitState.Z1 : QbitState.Z0;
        }
    }

    void OnMouseDown() {
        if (!_hasCollapsed && currentQBits <= 10) {
            _hasCollapsed = true;
            currentQBits--;
            
            switch (_state) {
                case QbitState.X0: _meshRenderer.material = x0; break;
                case QbitState.X1: _meshRenderer.material = x1; break;
                case QbitState.Z0: _meshRenderer.material = z0; break;
                case QbitState.Z1: _meshRenderer.material = z1; break;
            }

            Score.instance.AddScore(_state);

            _animator.SetTrigger("Collapse");

            Invoke("SelfDestroy", 4f);    
        }
    }

    void SelfDestroy() {
        Destroy(gameObject);
    }
}
