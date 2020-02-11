using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Collections;
using UnityEngine;

public class ReadTable
{
    private static object ParseStruct(Type t, string data)
    {
        if (t.Equals(typeof(string))) return data = data.Replace("#", "\n"); ;  
        if (data.Trim() == "" || data.Trim() == null) 
            return null;
        if (t.IsPrimitive)
        {
            if (t.Equals(typeof(int))) 
            {
                int val = 0;
                try
                {
                    val = int.Parse(data);
                }
                catch (Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }              
            else if (t.Equals(typeof(uint))) 
            {
                uint val = 0;
                try
                {
                    val = uint.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }                
            else if (t.Equals(typeof(float))) 
            {
                
                float val = 0f;
                try
                {
                    val = float.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(double))) 
            {
                double val = 0;
                try
                {
                    val = double.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(long))) 
            {
                 long val = 0;
                try
                {
                    val = long.Parse(data); 
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(ulong))) 
            {
                ulong val = 0;
                try
                {
                    val = ulong.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(bool))) 
            {
                bool val = false;
                try
                {
                    val = bool.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(byte))) 
            {
                byte val = 0;
                try
                {
                    val = byte.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(sbyte))) 
            {
                sbyte val = 0;
                try
                {
                    val = sbyte.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(short))) 
            {
                 short val = 0;
                try
                {
                    val = short.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(ushort))) 
            {
                ushort val = 0;
                try
                {
                    val = ushort.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else if (t.Equals(typeof(char))) 
            {
                char val = '0';
                try
                {
                    val = char.Parse(data);
                }
                catch(Exception)
                {
                     Debug.LogError("error .." + t.Name + "  value: " + data + " table: " + currentTable);
                }
                return val;
            }
            else return null;
        }
        if (t.IsArray)
        {
            Type et = t.GetElementType();
            data = data.Replace("\"(", "");
            data = data.Replace(")\"", "");
            data = data.Replace("),(", "|");
            string[] elmentData = data.Split('|');
            Array ary = Array.CreateInstance(et, (int)elmentData.Length);
            for (int i = 0; i < ary.Length; i++)
            {
                ary.SetValue(ParseStruct(et, elmentData[i]), i);
            }
            return ary;
        }

        if (t.IsClass || t.IsValueType)
        {
            object o = FormatterServices.GetUninitializedObject(t);
            BindingFlags FieldBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField;
            string[] dataunit = data.Split(',');
            int data_index = 0;
            foreach (System.Reflection.FieldInfo p in t.GetFields(FieldBindingFlags))
            {
                if (data_index >= dataunit.Length) break;
                p.SetValue(o, ParseStruct(p.FieldType, dataunit[data_index++]));
            }
            return o;
        }
        return null;

    }

    private static object ReadLine(Type t, string[] data)
    {
        object o = FormatterServices.GetUninitializedObject(t);

        BindingFlags FieldBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.SetField;
        int index = 0;
        foreach (var p in t.GetFields(FieldBindingFlags))
        {
            if (index >= data.Length)
            {
                Debug.LogError(currentTable + "   " + p.FieldType.Name);
            }
            p.SetValue(o, ParseStruct(p.FieldType, data[index++]));
        }
        return o;
    }

    private static string currentTable = null;
    public static T Read<T>(string tableName)
    { 
        if (string.IsNullOrEmpty(tableName))
            return default(T);
        currentTable = tableName;
        Type t = typeof(T);
        if (!t.IsGenericType || !t.GetGenericTypeDefinition().Equals(typeof(Dictionary<object, object>).GetGenericTypeDefinition()))
        {
             Debug.LogError("Erro Struct ,please use Dictionary .example Dictionary<int,Mystruct> type object");
            throw new FormatException();
        }
        string[] lineArray = null;
        try
        {
            //TextAsset textAsset = Resources.Load<TextAsset>("Configs/" + tableName);
            //if (textAsset != null)
            // {
            //   lineArray = textAsset.text.Split("\n"[0]);  
            //}
            //  else
           // {
            //    Debug.LogError("没有这个配置表：" + tableName);
             //   return default(T);
           // }
            using (StreamReader sr = new StreamReader("Assets/Resources/Configs/" + tableName + ".txt")) 
            {
                if (sr.Peek() == -1)
                {
                    Debug.LogError("没有这个配置表：" + tableName);
                    return default(T);
                }
                string text;
                if ((text = sr.ReadToEnd()) != null)
                {
                    lineArray = text.Split("\n"[0]);
                }
                else
                {
                    Debug.LogError("没有这个配置表：" + tableName);
                    return default(T);
                }
            }
        }
        catch (Exception)
        {
             Debug.LogError("没有这个配置表：" + tableName);
            throw;
        }
        if (lineArray == null)
        {
             Debug.LogError("配置数据为空：" + tableName);
            return default(T);
        }
        Type[] typeArguments = t.GetGenericArguments();
        int members = lineArray.Length - 1;
        IDictionary dic = System.Activator.CreateInstance(t) as IDictionary;
        for (int i = 1; i <= members; i++)
        {
            string[] parameterArray = lineArray[i].Split("\t"[0]);
            if (parameterArray[0] == "")
                continue;
            object key;
            if (typeArguments[0].IsPrimitive)
            {
                key = System.Activator.CreateInstance(typeArguments[0]);
            }
            key = ParseStruct(typeArguments[0], parameterArray[0]);

            object key_value;
            if (typeArguments[1].IsPrimitive)
            {
                key_value = System.Activator.CreateInstance(typeArguments[1]);
                key_value = ReadLine(typeArguments[1], parameterArray);
                if (dic.Contains(key))
                {
                     Debug.LogError("Key IS  Rpeat  , is Erro DataTable ...... Please Check  " + currentTable + "   " + key);
                }
                dic[key] = key_value;
            }
            else if (typeArguments[1].IsGenericType)//list 
            {
                IList list = System.Activator.CreateInstance(typeArguments[1]) as IList;
                Type[] listUnitTypes = typeArguments[1].GetGenericArguments();
                key_value = ReadLine(listUnitTypes[0], parameterArray);
                if (!dic.Contains(key))
                {
                    list.Add(key_value);
                    dic[key] = list;
                }
                else
                {
                    IList list2 = dic[key] as IList;
                    list2.Add(key_value);
                }
            }
            else// class
            {
                key_value = ReadLine(typeArguments[1], parameterArray);
                if (dic.Contains(key))
                {
                     Debug.LogError("Key IS  Rpeat  , is Erro DataTable ...... Please Check  " + currentTable + "   " + key);
                }
                dic[key] = key_value;
            }
        }
        return (T)dic;
    }
}
