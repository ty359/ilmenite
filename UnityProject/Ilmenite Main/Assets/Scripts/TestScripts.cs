using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestScripts : MonoBehaviour {

    void Test01()
    {
        ION root = new Dictionary<string, ION>
        {
            {"name", "orz" },
            {"type", "ION" },
            {"size", 5 },
            {"append",
                new Dictionary<string, ION>
                {
                    { "a", (short)10},
                    { "b", (long)5},
                    { "c", (byte)12},
                    { "d", 0},
                    { "sum", (int)27}
                }
            },
            {"height",
                new List<ION>
                {
                    3.2f,
                    4.3,
                    4.5,
                }
            }
        };
        Debug.Log(root);
        MemoryStream memoryStream = new MemoryStream();
        root.Write(memoryStream);
        Debug.Log(memoryStream.ToArray().Length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        root = ION.Read(memoryStream);
        Debug.Log(root);
    }

    void Test02() {
        ION a = 10;
        ION b = 3.2f;
        ION c = new List<ION> { a, 20, 30.2 };
        ION d = new Dictionary<string, ION>
        {
            { "length", 9 },
            { "weight", 3.2 },
            { "appends", c}
        };
        Debug.Log(a);
        Debug.Log(b);
        Debug.Log(c);
        Debug.Log(d);
    }

    void Start()
    {
        Test01();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
