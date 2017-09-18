using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PowerupInfo)), CanEditMultipleObjects]
public class PowerupEditor : Editor {

    public SerializedProperty
        powerupType_prop,
        duration_prop,
        reloadTimeFactor_prop,
        damageBoostFactor_prop,
        movementSpeedBoostFactor_prop,
        bonusHP_prop,
        regenerationAmountPerSec_prop;


    public void OnEnable() {
        powerupType_prop = serializedObject.FindProperty("powerupType");
        duration_prop = serializedObject.FindProperty("duration");
        reloadTimeFactor_prop = serializedObject.FindProperty("reloadTimeFactor");
        damageBoostFactor_prop = serializedObject.FindProperty("damageBoostFactor");
        movementSpeedBoostFactor_prop = serializedObject.FindProperty("movementSpeedBoostFactor");
        bonusHP_prop = serializedObject.FindProperty("bonusHP");
        regenerationAmountPerSec_prop = serializedObject.FindProperty("regenerationAmountPerSec");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(powerupType_prop);
        PowerupInfo.Type type = (PowerupInfo.Type)powerupType_prop.enumValueIndex;

        switch (type) {
            case PowerupInfo.Type.FasterReload:
                EditorGUILayout.PropertyField(duration_prop);
                EditorGUILayout.PropertyField(reloadTimeFactor_prop);
                break;

            case PowerupInfo.Type.DamageBoost:
                EditorGUILayout.PropertyField(duration_prop);
                EditorGUILayout.PropertyField(damageBoostFactor_prop);
                break;

            case PowerupInfo.Type.MovementSpeedBoost:
                EditorGUILayout.PropertyField(duration_prop);
                EditorGUILayout.PropertyField(movementSpeedBoostFactor_prop);
                break;

            case PowerupInfo.Type.TemporaryBonusHP:
                EditorGUILayout.PropertyField(duration_prop);
                EditorGUILayout.PropertyField(bonusHP_prop);
                break;

            case PowerupInfo.Type.AutoHPRegen:
                EditorGUILayout.PropertyField(duration_prop);
                EditorGUILayout.PropertyField(regenerationAmountPerSec_prop);
                break;

            case PowerupInfo.Type.ScattershotWeapon:
                EditorGUILayout.PropertyField(duration_prop);
                break;

            case PowerupInfo.Type.BarrelBombweapon:
                EditorGUILayout.PropertyField(duration_prop);
                break;

            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
