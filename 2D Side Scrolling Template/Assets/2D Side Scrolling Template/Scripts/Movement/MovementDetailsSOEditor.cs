using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovementDetailsSO))]
public class MovementDetailsSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("");

        MovementDetailsSO movementDetails = (MovementDetailsSO)target;

        EditorGUILayout.LabelField("Character Type");
        movementDetails.CharacterType = (SelectionType)EditorGUILayout.EnumPopup(movementDetails.CharacterType);
        EditorGUILayout.LabelField("");


        if (movementDetails.CharacterType == SelectionType.Player)
        {
            EditorGUILayout.LabelField("Movement Details");
            movementDetails.MovementSpeed = EditorGUILayout.FloatField("Movement Speed", movementDetails.MovementSpeed);

            EditorGUILayout.LabelField("");

            EditorGUILayout.LabelField("Dash Details");
            movementDetails.DashSpeed = EditorGUILayout.FloatField("Dash Speed", movementDetails.DashSpeed);
            movementDetails.DashDistance = EditorGUILayout.FloatField("Dash Distance", movementDetails.DashDistance);
            movementDetails.DashCoolDown = EditorGUILayout.FloatField("Dash Cool Down", movementDetails.DashCoolDown);

            EditorGUILayout.LabelField("");

            EditorGUILayout.LabelField("Dash Details");
            movementDetails.SlideSpeed = EditorGUILayout.FloatField("Dash Speed", movementDetails.SlideSpeed);
            movementDetails.SlideDistance = EditorGUILayout.FloatField("Dash Distance", movementDetails.SlideDistance);
            movementDetails.SlideCoolDown = EditorGUILayout.FloatField("Dash Cool Down", movementDetails.SlideCoolDown);
        }
        else if (movementDetails.CharacterType == SelectionType.Enemy)
        {
            EditorGUILayout.LabelField("Movement Details");
            movementDetails.MinMovementSpeed = EditorGUILayout.FloatField("Min Movement Speed", movementDetails.MinMovementSpeed);
            movementDetails.MaxMovementSpeed = EditorGUILayout.FloatField("Max Movement Speed", movementDetails.MaxMovementSpeed);
        }

        EditorUtility.SetDirty(movementDetails);
    }
}
