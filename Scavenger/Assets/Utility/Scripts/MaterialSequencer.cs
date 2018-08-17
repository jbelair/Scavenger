using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSequencer : MonoBehaviour
{
    public bool instance = true;
    public Material material;

    [System.Serializable]
    public class FloatContainer
    {
        public string name;
        public FloatSequencer sequence;
    }
    public FloatContainer[] floats;

    [System.Serializable]
    public class ColourContainer
    {
        public string name;
        public ColourSequencer sequence;
    }
    public ColourContainer[] colours;

    [System.Serializable]
    public class VectorContainer
    {
        public string name;
        public Vector3Sequencer sequence;
    }
    public VectorContainer[] vectors;

	// Use this for initialization
	void Start ()
    {
        if (instance)
        {
            GetComponent<MeshRenderer>().sharedMaterial = material = new Material(material);
        }

		foreach(FloatContainer container in floats)
        {
            material.SetFloat(container.name, container.sequence.Start());
        }

        foreach (ColourContainer container in colours)
        {
            material.SetColor(container.name, container.sequence.Start());
        }

        foreach (VectorContainer container in vectors)
        {
            material.SetVector(container.name, container.sequence.Start());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (FloatContainer container in floats)
        {
            material.SetFloat(container.name, container.sequence.Update());
        }

        foreach (ColourContainer container in colours)
        {
            material.SetColor(container.name, container.sequence.Update());
        }

        foreach (VectorContainer container in vectors)
        {
            material.SetVector(container.name, container.sequence.Update());
        }
    }
}
