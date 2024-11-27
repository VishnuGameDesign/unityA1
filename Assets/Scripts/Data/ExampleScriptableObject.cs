using UnityEngine;

[CreateAssetMenu(fileName = "ExampleScriptableObject", menuName = "Scriptable Objects/ExampleScriptableObject")]
public class ExampleScriptableObject : ScriptableObject
{
    [field: SerializeField] public float ExampleProperty { get; private set; }
    [field: SerializeField] public Texture2D SomeTexture { get; private set; }
    [field: SerializeField] public GameObject SomePrefab { get; private set; }
}