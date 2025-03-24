using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UiValueEditorGenerator : MonoBehaviour
{
    public List<UiValueInputFieldEditorEdit> UiInputFieldPrefabs;
    public List<UiValueSliderEditorEdit> UiSliderPrefabs;
    public List<UiCheckboxEditorEdit> UiCheckboxPrefabs;
    public List<UiTagInputFieldDisplay> UiTagInputFieldDisplayPrefabs;
    public UiMultipleInputFieldEditorEdit UiMultipleInputFieldEditorEditPrefab;

    public UiValueEditorEdit GenerateSingleUiField(object obj, string fieldName, Transform holder, Action onValueEdit = null, Func<string, bool> condition = null, ValueEditorEditType type = ValueEditorEditType.Default)
    {
        FieldInfo fieldInfo = obj.GetType().GetField(fieldName);
        UiValueEditorEdit outputUi = GenerateSingleUiEdit(fieldInfo, holder, type);

        outputUi.Init(obj, fieldInfo, onValueEdit, condition);
        return outputUi;
    }

    private UiValueEditorEdit GenerateSingleUiEdit(FieldInfo fieldInfo, Transform holder, ValueEditorEditType type)
    {
        SiegeEditorSliderAttribute slider = Attribute.GetCustomAttribute(fieldInfo, typeof(SiegeEditorSliderAttribute)) as SiegeEditorSliderAttribute;
        UiValueEditorEdit uiEdit = null;
        Type fieldType = fieldInfo.FieldType;

        if (fieldType == typeof(string))
        {
            uiEdit = Instantiate(GetTagInputFieldPrefab(type), holder);
        }
        else if (fieldType == typeof(bool))
        {
            uiEdit = Instantiate(GetCheckboxPrefab(type), holder);
        }
        else if (slider != null)
        {
            uiEdit = Instantiate(GetSliderPrefab(type), holder);
        }
        else
        {
            uiEdit = Instantiate(GetInputFieldPrefab(type), holder);
        }

        return uiEdit;
    }

    private UiValueEditorEdit GetInputFieldPrefab(ValueEditorEditType type)
    {
        UiValueEditorEdit result = UiInputFieldPrefabs.Find(x => x.PrefabType == type);
        if(result == null)
        {
            return UiInputFieldPrefabs.Find(x => x.PrefabType == ValueEditorEditType.Default);
        }
        return result;
    }

    private UiValueEditorEdit GetSliderPrefab(ValueEditorEditType type)
    {
        UiValueEditorEdit result = UiSliderPrefabs.Find(x => x.PrefabType == type);
        if (result == null)
        {
            return UiSliderPrefabs.Find(x => x.PrefabType == ValueEditorEditType.Default);
        }
        return result;
    }

    private UiValueEditorEdit GetCheckboxPrefab(ValueEditorEditType type)
    {
        UiValueEditorEdit result = UiCheckboxPrefabs.Find(x => x.PrefabType == type);
        if (result == null)
        {
            return UiCheckboxPrefabs.Find(x => x.PrefabType == ValueEditorEditType.Default);
        }
        return result;
    }

    private UiValueEditorEdit GetTagInputFieldPrefab(ValueEditorEditType type)
    {
        UiValueEditorEdit result = UiTagInputFieldDisplayPrefabs.Find(x => x.PrefabType == type);
        if (result == null)
        {
            return UiTagInputFieldDisplayPrefabs.Find(x => x.PrefabType == ValueEditorEditType.Default);
        }
        return result;
    }

    public UiMultipleInputFieldEditorEdit GenerateMultipleUiField(object obj, string[] fieldNames, string displayedName, Transform holder, Action onValueEdit = null, Func<string, bool> condition = null, ValueEditorEditType type = ValueEditorEditType.Default)
    {
        List<UiValueInputFieldEditorEdit> inputFields = new List<UiValueInputFieldEditorEdit>();

        for (int i = 0; i < fieldNames.Length; i++)
        {
            FieldInfo fieldInfo = obj.GetType().GetField(fieldNames[i]);
            UiValueEditorEdit outputUi = GenerateSingleUiEdit(fieldInfo, holder, type);
            UiValueInputFieldEditorEdit uiInputField = outputUi as UiValueInputFieldEditorEdit;

            if (uiInputField != null)
            {
                uiInputField.Init(obj, fieldInfo, onValueEdit, condition);
                inputFields.Add(uiInputField);
            }
            else
            {
                Destroy(outputUi.gameObject);
            }
        }

        UiMultipleInputFieldEditorEdit uiEdit = Instantiate(UiMultipleInputFieldEditorEditPrefab, holder);
        uiEdit.Init(inputFields, displayedName);

        return uiEdit;
    }

    public UiValueEditorEdit[] GenerateUiEdits(object obj, Transform holder, Action onValueEdit = null, Func<string, bool> condition = null, ValueEditorEditType type = ValueEditorEditType.Default, params string[] groups)
    {
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        UiValueEditorEdit[] output = new UiValueEditorEdit[fields.Length];
        List<FieldInfo> finalFields = new List<FieldInfo>();

        output = FilterFields(fields, out fields, groups);

        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];
            SiegeEditorSliderAttribute slider = Attribute.GetCustomAttribute(field, typeof(SiegeEditorSliderAttribute)) as SiegeEditorSliderAttribute;
            UiValueEditorEdit uiEdit = null;
            Type fieldType = field.FieldType;

            uiEdit = GenerateSingleUiEdit(field, holder, type);
            output[i] = uiEdit;
            uiEdit.Init(obj, field, onValueEdit, condition);
        }

        return output;
    }

    public void UpdateValueEditorEdits(object obj, UiValueEditorEdit[] uiEdits, params string[] groups)
    {
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        FilterFields(fields, out fields, groups);

        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];
            uiEdits[i].UpdateUI(obj, field);
        }
    }

    public void UpdateSingleValueEditorUi(object obj, string name, UiValueEditorEdit uiValueEditorEdit)
    {
        FieldInfo fieldInfo = obj.GetType().GetField(name);
        uiValueEditorEdit.UpdateUI(obj, fieldInfo);
    }

    private UiValueEditorEdit[] FilterFields(FieldInfo[] fields, out FieldInfo[] output, string[] groups)
    {
        List<FieldInfo> finalFields = new List<FieldInfo>();
        List<string> filters = groups.ToList();
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];
            SiegeEditorGroupAttribute group = Attribute.GetCustomAttribute(field, typeof(SiegeEditorGroupAttribute)) as SiegeEditorGroupAttribute;
            bool exclude = Attribute.GetCustomAttribute(field, typeof(SiegeEditorExcludeAttribute)) as SiegeEditorExcludeAttribute != null;

            if (!exclude)
            {
                if (groups == null || groups.Length == 0)
                {
                    finalFields.Add(field);
                }
                else
                {
                    if (group != null)
                    {
                        for (int j = 0; j < group.Groups.Length; j++)
                        {
                            if (filters.Contains(group.Groups[j]))
                            {
                                finalFields.Add(field);
                                break;
                            }
                        }
                    }
                }
            }
        }

        output = finalFields.ToArray();
        return new UiValueEditorEdit[output.Length];
    }
}