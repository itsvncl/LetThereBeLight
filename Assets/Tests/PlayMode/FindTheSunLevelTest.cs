using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FindTheSunLevelTest
{
    [Test]
    public void AzimuthToVector3Test() {
        Vector3 v1 = new Vector3(-0.99f, -4.87f, -0.55f);
        Vector3 v2 = new Vector3(-1.52f, -3.60f, -0.83f);
        Vector3 v3 = new Vector3(-1.05f, -1.44f, -0.91f);
        Vector3 v4 = new Vector3(-0.97f, -6.02f, -0.06f);

        Vector3 c1 = SunGame.AzimuthToVector3(0.2f, 0.11f, 5.0f);
        Vector3 c2 = SunGame.AzimuthToVector3(0.4f, 0.21f, 4.0f);
        Vector3 c3 = SunGame.AzimuthToVector3(0.63f, 0.47f, 2.0f);
        Vector3 c4 = SunGame.AzimuthToVector3(0.16f, 0.01f, 6.1f);

        c1 = new Vector3((float)Math.Round(c1.x, 2), (float)Math.Round(c1.y, 2), (float)Math.Round(c1.z, 2));
        c2 = new Vector3((float)Math.Round(c2.x, 2), (float)Math.Round(c2.y, 2), (float)Math.Round(c2.z, 2));
        c3 = new Vector3((float)Math.Round(c3.x, 2), (float)Math.Round(c3.y, 2), (float)Math.Round(c3.z, 2));
        c4 = new Vector3((float)Math.Round(c4.x, 2), (float)Math.Round(c4.y, 2), (float)Math.Round(c4.z, 2));

        Assert.AreEqual(v1, c1);
        Assert.AreEqual(v2, c2);
        Assert.AreEqual(v3, c3);
        Assert.AreEqual(v4, c4);
    }
}
