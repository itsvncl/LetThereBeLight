using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowLoader : MonoBehaviour
{
    public static FlowLoader FL;

    [SerializeField] private Sprite _flow;
    [SerializeField] private Sprite _origin;

    [SerializeField] private Color _red;
    [SerializeField] private Color _blue;
    [SerializeField] private Color _green;
    [SerializeField] private Color _pink;
    [SerializeField] private Color _yellow;
    [SerializeField] private Color _orange;
    [SerializeField] private Color _lightblue;

    void Awake() {
        FL = this;
    }

    public Sprite FlowSprite { get { return _flow; }}
    public Sprite OriginSprite { get { return _origin; } }

    public Color Red { get { return _red; } }
    public Color Blue { get { return _blue; } }
    public Color Green { get { return _green; } }
    public Color Pink { get { return _pink; } }
    public Color Yellow { get { return _yellow; } }
    public Color Orange { get { return _orange; } }
    public Color LightBlue { get { return _lightblue; } }
}
 