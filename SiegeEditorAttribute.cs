using System;

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorRangeAttribute : Attribute //implemented
{
    public float RangeMin { get; private set; }
    public float RangeMax { get; private set; }
    public bool IsDisplayedAsString { get; private set; }

    public SiegeEditorRangeAttribute(float min, float max, bool isDisplayedAsString = false)
    {
        RangeMin = min;
        RangeMax = max;
        IsDisplayedAsString = isDisplayedAsString;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorSliderAttribute : Attribute //implemented
{
    public int RangeMin { get; private set; }
    public int RangeMax { get; private set; }
    public int Granulation { get; private set; }
    public bool AddPercentSymbol { get; private set; }

    public SiegeEditorSliderAttribute(int min = 0, int max = 100, int granulation = 1, bool addPercentSymbol = false)
    {
        RangeMin = min;
        RangeMax = max;
        Granulation = granulation;
        AddPercentSymbol = addPercentSymbol;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorOnlyPositiveNumbersAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorOnlyNegativeNumbersAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorExcludeAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorTagNameFieldAttribute : Attribute
{
    public string Name { get { return tag_.Translate(); } private set { tag_ = value; } }
    private string tag_;

    public SiegeEditorTagNameFieldAttribute(string tag)
    {
        tag_ = tag;
    }
}

public class SiegeEditorGroupAttribute : Attribute
{
    public string[] Groups { get; private set; }

    public SiegeEditorGroupAttribute(params string[] groupNames)
    {
        Groups = groupNames;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorCheckboxAttribute : Attribute
{
    public SiegeEditorCheckboxAttribute()
    {

    }
}


[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorEnumExcludeAttribute : Attribute
{
    public byte[] ExcludedValues { get; private set; }

    public SiegeEditorEnumExcludeAttribute(params byte[] excludedEnums)
    {
        ExcludedValues = excludedEnums;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorDropdownAttribute : Attribute
{
    public SiegeEditorDropdownAttribute()
    {

    }
}

// works only with UiTagInputFieldDisplay
[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorCustomHeightAttribute : Attribute
{
    public float Height { get; private set; }

    public SiegeEditorCustomHeightAttribute(float height)
    {
        Height = height;
    }
}

// works only with UiTagInputFieldDisplay
[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorCustomWidthAttribute : Attribute
{
    public float Width { get; private set; }

    public SiegeEditorCustomWidthAttribute(float width)
    {
        Width = width;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorCustomTextTooltipAttribute : Attribute
{
    public string Content { get; private set; }
    public bool IsTag { get; private set; }

    public SiegeEditorCustomTextTooltipAttribute(string content, bool isTag = false)
    {
        Content = content;
        IsTag = isTag;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class SiegeEditorOnlyUnityEditor : Attribute
{
    public SiegeEditorOnlyUnityEditor() { }
}