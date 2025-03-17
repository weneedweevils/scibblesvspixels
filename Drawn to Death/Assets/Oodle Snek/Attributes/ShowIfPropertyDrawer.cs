#if UNITY_EDITOR 
using UnityEngine;
using UnityEditor;

namespace CustomAttributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
            bool enabled = GetShowAttributeResult(condHAtt, property);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            GUI.enabled = wasEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute condHAtt = (ShowIfAttribute)attribute;
            bool enabled = GetShowAttributeResult(condHAtt, property);

            if (enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        private bool GetShowAttributeResult(ShowIfAttribute condHAtt, SerializedProperty property)
        {
            bool enabled = true;

            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
            string conditionPath = propertyPath.Replace(property.name, condHAtt.source); //changes the path to the conditionalsource property path
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

            object value = condHAtt.value;
            Relation evaluation = condHAtt.evaluationMethod;

            switch (sourcePropertyValue.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    {
                        switch (evaluation)
                        {
                            case Relation.Equal:
                                enabled = sourcePropertyValue.boolValue.Equals(value);
                                break;
                            case Relation.NotEqual:
                                enabled = !sourcePropertyValue.boolValue.Equals(value);
                                break;
                            default:
                                Debug.LogWarningFormat("Warning: {0} is not a supported Relation type for the {1} Property Type",
                                    evaluation.ToString(),
                                    sourcePropertyValue.propertyType.ToString()
                                );
                                enabled = true;
                                break;
                        }
                        break;
                    }
                case SerializedPropertyType.Enum:
                    {
                        switch (evaluation)
                        {
                            case Relation.Equal:
                                enabled = sourcePropertyValue.enumValueIndex == ((int)value);
                                break;
                            case Relation.NotEqual:
                                enabled = sourcePropertyValue.enumValueIndex != ((int)value);
                                break;
                            case Relation.GT:
                                enabled = sourcePropertyValue.enumValueIndex > ((int)value);
                                break;
                            case Relation.GTE:
                                enabled = sourcePropertyValue.enumValueIndex >= ((int)value);
                                break;
                            case Relation.LT:
                                enabled = sourcePropertyValue.enumValueIndex < ((int)value);
                                break;
                            case Relation.LTE:
                                enabled = sourcePropertyValue.enumValueIndex <= ((int)value);
                                break;
                            default:
                                Debug.LogWarningFormat("Warning: {0} is not a supported Relation type for the {1} Property Type",
                                    evaluation.ToString(),
                                    sourcePropertyValue.propertyType.ToString()
                                );
                                enabled = true;
                                break;
                        }
                        break;
                    }
                case SerializedPropertyType.Integer:
                    {
                        switch (evaluation)
                        {
                            case Relation.Equal:
                                enabled = sourcePropertyValue.intValue == ((int)value);
                                break;
                            case Relation.NotEqual:
                                enabled = sourcePropertyValue.intValue != ((int)value);
                                break;
                            case Relation.GT:
                                enabled = sourcePropertyValue.intValue > ((int)value);
                                break;
                            case Relation.GTE:
                                enabled = sourcePropertyValue.intValue >= ((int)value);
                                break;
                            case Relation.LT:
                                enabled = sourcePropertyValue.intValue < ((int)value);
                                break;
                            case Relation.LTE:
                                enabled = sourcePropertyValue.intValue <= ((int)value);
                                break;
                            default:
                                Debug.LogWarningFormat("Warning: {0} is not a supported Relation type for the {1} Property Type",
                                    evaluation.ToString(),
                                    sourcePropertyValue.propertyType.ToString()
                                );
                                enabled = true;
                                break;
                        }
                        break;
                    }
                case SerializedPropertyType.Float:
                    {
                        switch (evaluation)
                        {
                            case Relation.Equal:
                                enabled = sourcePropertyValue.floatValue == ((int)value);
                                break;
                            case Relation.NotEqual:
                                enabled = sourcePropertyValue.floatValue != ((int)value);
                                break;
                            case Relation.GT:
                                enabled = sourcePropertyValue.floatValue > ((int)value);
                                break;
                            case Relation.GTE:
                                enabled = sourcePropertyValue.floatValue >= ((int)value);
                                break;
                            case Relation.LT:
                                enabled = sourcePropertyValue.floatValue < ((int)value);
                                break;
                            case Relation.LTE:
                                enabled = sourcePropertyValue.floatValue <= ((int)value);
                                break;
                            default:
                                Debug.LogWarningFormat("Warning: {0} is not a supported Relation type for the {1} Property Type",
                                    evaluation.ToString(),
                                    sourcePropertyValue.propertyType.ToString()
                                );
                                enabled = true;
                                break;
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogWarningFormat("Warning: {0} is not a supported value type for the ConditionalHideAttribute", sourcePropertyValue.propertyType.ToString());
                        enabled = true;
                        break;
                    }
            }

            return enabled;
        }
    }
}
#endif