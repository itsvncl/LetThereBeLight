using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, IClickable
{
    public PipeType _type;
    public PipeOrientation _orientation;
    public PlumberGame game;
    public SpriteRenderer sr;
    
    private bool inFlow = false;
    
    private bool locked = false;

    public PipeType type { get { return _type; } }
    public PipeOrientation orientation { get { return _orientation; } }

    void Awake() {
        _orientation = PipeOrientationMethods.GetOrientations()[Random.Range(0,4)];
        orientate();
    }

    public void rotate() {
        if(_orientation == PipeOrientation.Left) {
            _orientation = 0;
        }
        else {
            _orientation++;
        }
    }
    public void OnClick(TouchData touchData) {
        if (locked) return;

        rotate();
        game.RevealRoute();

        if (game.IsWin()) {
            TouchSystem.Instance.Disable();
            LevelManager.Instance.LevelComplete();
        }
        
        orientate();
    }

    private void orientate() {
        float rotation = 0;

        switch (orientation) {
            case PipeOrientation.Left:
                rotation = 270;
                break;
            case PipeOrientation.Right:
                rotation = 90;
                break;
            case PipeOrientation.Up:
                rotation = 0;
                break;
            case PipeOrientation.Down:
                rotation = 180;
                break;
        }

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -rotation));

        Color c = sr.color;
        c.a = inFlow ? 1.0f : 0.5f;
        sr.color = c;
    }

    public void SetOrientation(PipeOrientation o) {
        _orientation = o;
        orientate();
    }

    public void SetHighlighted(bool v) {
        inFlow = v;
        orientate();
    }

    public bool IsHighlighted { get { return inFlow; } }

    public void LockOrientation() {
        locked = true;
    }
    public void UnlockOrientation() {
        locked = false;
    }
}
