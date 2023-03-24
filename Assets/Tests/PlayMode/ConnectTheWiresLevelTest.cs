using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ConnectTheWiresLevelTest {
    [UnityTest]
    public IEnumerator GoodConnection()
    {
        GameObject gameGo = new();
        WireGame game = gameGo.AddComponent<WireGame>();
        game.SetWireCount(1);

        GameObject paddingObj = new();
        GameObject w1Go = new();
        GameObject w2Go = new();

        Wire w1 = w1Go.AddComponent<Wire>();
        Wire w2 = w2Go.AddComponent<Wire>();

        paddingObj.AddComponent<SpriteRenderer>();

        w1._color = WireColor.RED;
        w2._color = WireColor.RED;

        TouchData tData = new(Vector3.zero, Vector3.zero, Vector3.zero, 0, w1Go, 1);

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(WireGame.ConnectedWireCount == 0);

        w1.exposedWire = paddingObj;
        w1.finalWire = paddingObj;
        w1.tempWire = paddingObj;

        w2.exposedWire = paddingObj;
        w2.finalWire = paddingObj;
        w2.tempWire = paddingObj;

        w2.OnTouchEnd(tData);

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(WireGame.ConnectedWireCount == 1);
    }
    [UnityTest]
    public IEnumerator ConnectingAlreadyConnected() {
        GameObject gameGo = new();
        WireGame game = gameGo.AddComponent<WireGame>();
        game.SetWireCount(1);

        GameObject paddingObj = new();
        GameObject w1Go = new();
        GameObject w2Go = new();

        Wire w1 = w1Go.AddComponent<Wire>();
        Wire w2 = w2Go.AddComponent<Wire>();

        paddingObj.AddComponent<SpriteRenderer>();

        w1._color = WireColor.RED;
        w2._color = WireColor.RED;

        TouchData tData = new(Vector3.zero, Vector3.zero, Vector3.zero, 0, w1Go, 1);

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(WireGame.ConnectedWireCount == 0);

        w1.exposedWire = paddingObj;
        w1.finalWire = paddingObj;
        w1.tempWire = paddingObj;

        w2.exposedWire = paddingObj;
        w2.finalWire = paddingObj;
        w2.tempWire = paddingObj;

        w2.OnTouchEnd(tData);

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(WireGame.ConnectedWireCount == 1);
        w2.OnTouchEnd(tData);

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(WireGame.ConnectedWireCount == 1);
    }

    [UnityTest]
    public IEnumerator MissmatchingColor() {
        GameObject gameGo = new();
        WireGame game = gameGo.AddComponent<WireGame>();
        game.SetWireCount(1);

        GameObject paddingObj = new();
        GameObject w1Go = new();
        GameObject w2Go = new();

        Wire w1 = w1Go.AddComponent<Wire>();
        Wire w2 = w2Go.AddComponent<Wire>();

        paddingObj.AddComponent<SpriteRenderer>();

        w1._color = WireColor.RED;
        w2._color = WireColor.GRAY;

        TouchData tData = new(Vector3.zero, Vector3.zero, Vector3.zero, 0, w1Go, 1);

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(WireGame.ConnectedWireCount == 0);

        w1.exposedWire = paddingObj;
        w1.finalWire = paddingObj;
        w1.tempWire = paddingObj;

        w2.exposedWire = paddingObj;
        w2.finalWire = paddingObj;
        w2.tempWire = paddingObj;

        w2.OnTouchEnd(tData);

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(WireGame.ConnectedWireCount == 0);
    }

    [UnityTest]
    public IEnumerator SelfConnection() {
        GameObject gameGo = new();
        WireGame game = gameGo.AddComponent<WireGame>();
        game.SetWireCount(1);

        GameObject paddingObj = new();
        GameObject w1Go = new();

        Wire w1 = w1Go.AddComponent<Wire>();

        paddingObj.AddComponent<SpriteRenderer>();

        w1._color = WireColor.RED;

        TouchData tData = new(Vector3.zero, Vector3.zero, Vector3.zero, 0, w1Go, 1);

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(WireGame.ConnectedWireCount == 0);

        w1.exposedWire = paddingObj;
        w1.finalWire = paddingObj;
        w1.tempWire = paddingObj;

        w1.OnTouchEnd(tData);

        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(WireGame.ConnectedWireCount == 0);
    }
}
