using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RopeGenerator))]
public class RopeGeneratorEditor : Editor
{
    SerializedProperty ropeSizeProperty;
    SerializedProperty partSizeProperty;
    SerializedProperty partDistanceProperty;

    public void OnEnable()
    {
        ropeSizeProperty = serializedObject.FindProperty("size");
        partSizeProperty = serializedObject.FindProperty("partSize");
        partDistanceProperty = serializedObject.FindProperty("partDistance");
    }

    public override void OnInspectorGUI()
    {
        float previousRopeSize = ropeSizeProperty.floatValue;
        float previousPartSize = partSizeProperty.floatValue;
        float previousPartDistance = partDistanceProperty.floatValue;

        base.OnInspectorGUI();

        serializedObject.Update();


        if (previousRopeSize != ropeSizeProperty.floatValue || previousPartSize != partSizeProperty.floatValue
            || previousPartDistance != partDistanceProperty.floatValue)
        {
            DoUpdate();
        }

        serializedObject.ApplyModifiedProperties();
    }

    //Update rope when a param of rope is modified in editor
    void DoUpdate()
    {
        RopeGenerator ropeGenerator = (RopeGenerator)target;

        if (ropeGenerator.gameObject.scene.IsValid())
        {
            ropeGenerator.GenerateRope();

            TriggerRope triggerRope = ropeGenerator.gameObject.GetComponent<TriggerRope>();

            if (triggerRope != null)
            {
                triggerRope.UpdateTriggerZoneSize();
            }
        }
    }
}
