using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private int _x0 = 0;
    private int _x1 = 0;
    private int _z0 = 0;
    private int _z1 = 0;

    public Text x0Txt;
    public Text x1Txt;
    public Text z0Txt;
    public Text z1Txt;

    public Text winText;
    public Text looseText;

    public static Score _instance;
    public static Score instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Score>();
            }
            return _instance;
        }
    }

    private void Awake() {
        _instance = this;
        SetupScore();
    }

    public void AddScore(QbitState state) {
        switch (state) {
            case QbitState.X0: _x0++; break;
            case QbitState.X1: _x1++; break;
            case QbitState.Z0: _z0++; break;
            case QbitState.Z1: _z1++; break;
        }
        SetupScore();
    }

    private void SetupScore() {
        x0Txt.text = $"X0: {_x0}";
        x1Txt.text = $"X1: {_x1}";
        z0Txt.text = $"Z0: {_z0}";
        z1Txt.text = $"Z1: {_z1}";
    }

    public void SetWin() {
        winText.gameObject.SetActive(true);
    }

    public void SetLoose() {
        looseText.gameObject.SetActive(true);
    }

}
