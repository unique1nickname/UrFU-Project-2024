using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BindingControl))]
public class BindingControlUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BindingControl bc = (BindingControl)target;
        // if (GUILayout.Button("Make new page")) bc.MakeNewPage();
        // if (GUILayout.Button("Delate page")) bc.DelatePage();

        bc.newItemName = EditorGUILayout.TextField("Imput item's name", bc.newItemName);
        // if (GUILayout.Button("Add an item")) bc.AddNewItem();
        // if (GUILayout.Button("Clear all slots")) bc.ClearAllSlots();

        if (bc.Grids.Length > 0)
        {
            bc.currentGrid = EditorGUILayout.IntSlider(
                "Selected Grid Index",
                bc.currentGrid,
                0,
                bc.Grids.Length - 1
            );
        }
        else EditorGUILayout.LabelField("Grids array is empty.");
    }
}
