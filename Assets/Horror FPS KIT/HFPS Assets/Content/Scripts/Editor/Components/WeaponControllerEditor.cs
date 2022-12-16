using UnityEditor;
using HFPS.Player;

namespace HFPS.Editors
{
    [CustomEditor(typeof(WeaponController)), CanEditMultipleObjects]
    public class WeaponControllerEditor : Editor
    {
        SerializedProperty m_shotgunSettings;
        SerializedProperty m_bulletModelSettings;

        WeaponController controller;

        void OnEnable()
        {
            controller = target as WeaponController;
            m_shotgunSettings = serializedObject.FindProperty("shotgunSettings");
            m_bulletModelSettings = serializedObject.FindProperty("bulletModelSettings");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            DrawPropertiesExcluding(serializedObject, "shotgunSettings", "bulletModelSettings");

            if (controller.bulletType == WeaponController.BulletType.Bullet)
            {
                EditorGUILayout.PropertyField(m_bulletModelSettings, true);
            }
            if (controller.weaponType == WeaponController.WeaponType.Shotgun)
            {
                EditorGUILayout.PropertyField(m_shotgunSettings, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}