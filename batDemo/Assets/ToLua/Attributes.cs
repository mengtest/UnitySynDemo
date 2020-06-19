using System;

[AttributeUsage(AttributeTargets.Class|AttributeTargets.Enum, AllowMultiple = false)]
//自动注册class到lua
public class AutoRegistLuaAttribute : Attribute
{

}
