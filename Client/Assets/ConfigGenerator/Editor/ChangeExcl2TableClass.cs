using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;
using System;
public class ChangeExcl2TableClass
{
    public static string savePath = Application.dataPath + "/ConfigGenerator/Scripts";
    public static string excelPath = Application.dataPath + "/ConfigGenerator/Configs";
    public static string defultPath = Application.dataPath + "/ConfigGenerator/Resources/Configs";
    #region 生成数据结构代码
    [MenuItem("ChangeExcl2TableClass/从excel生成数据结构代码")]
    static void Change()
    {
        string name = "CfgSvc";
        string cs_name= name+".cs";
        string des = "配置数据服务";

        string targetPath = savePath + "/"+ cs_name;
        DirectoryInfo folder = new DirectoryInfo(excelPath);

        using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream))
            {
                StringBuilder builder = new StringBuilder();
                List<string> classNames = new List<string>();
              
                builder.AppendLine("/****************************************************");
                builder.AppendLine("文件："+ cs_name);
                builder.AppendLine("作者：xietingrong");
                builder.AppendLine("邮箱: 592183492@qq.com");
                builder.AppendLine("日期："+ string.Format("{0:D2}:{1:D2}:{2:D2} " + "{3:D4}/{4:D2}/{5:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                builder.AppendLine("功能：" + des);
                builder.AppendLine("****************************************************/");
                builder.AppendLine("using UnityEngine;");
                builder.AppendLine("using System.Collections.Generic;\n");
                builder.AppendLine("public class "+ name+ "{");
                builder.AppendLine("\tprivate static  "+name+ " instance = null;");
                builder.AppendLine("\tpublic static " + name + " Instance " + "{");
                builder.AppendLine("\t\tget {");
                builder.AppendLine("\t\t\tif (instance == null) {");
                builder.AppendLine("\t\t\t\tinstance = new " + name + "();");
                builder.AppendLine("\t\t\t}");
                builder.AppendLine("\t\t\treturn instance;");
                builder.AppendLine("\t\t}");
                builder.AppendLine("\t}");
                textWriter.Write(builder);
                foreach (var file in folder.GetFiles("*.xlsx"))
                {
                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;
                    string className = textInfo.ToTitleCase(Path.GetFileNameWithoutExtension(file.FullName))+ "Cfg";
                    WriteTableData(file.FullName, textWriter, className);
                    classNames.Add(className);
                }
                textWriter.WriteLine("\tpublic void Init()");
                textWriter.WriteLine("\t{");
                for (int i = 0; i < classNames.Count; i++)
                {
                    textWriter.WriteLine("\t\t_" + classNames[i] + " = ReadTable.Read<Dictionary<int, " + classNames[i] + "> >(\"" + classNames[i] + "\");");
                }
                textWriter.WriteLine("\t}");
                textWriter.WriteLine("}");
                foreach (var file in folder.GetFiles("*.xlsx"))
                {
                    ExcelUtility(file.FullName, textWriter, Path.GetFileNameWithoutExtension(file.FullName));
                }
            }
        }

        EditorUtility.DisplayDialog("成功", "数据结构生成成功", "确定");
        AssetDatabase.Refresh();
    }
    public static void ExcelUtility(string excelFile, TextWriter textWriter, string className)
    {
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);

        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
		DataSet mResultSet = null;
		try {
			mResultSet = mExcelReader.AsDataSet();
		} catch (System.Exception ) {
             Debug.Log("单元格格式错误，无法获取正确数据[" + excelFile + "]");
		}
        
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;
        //注释
        List<string> comments = new List<string>();
        //字段类型
        List<string> filedType = new List<string>();
        //字段名字
        List<string> filedName = new List<string>();
        int colCount = mSheet.Columns.Count;
        string text = null;
        for (int i = 0; i < colCount; i++)
        {
            text = mSheet.Rows[0][i].ToString().Trim();
            if (string.IsNullOrEmpty(text)) continue;
            switch (text)
            {
                case "bool": filedType.Add("bool"); break;
                case "int": filedType.Add("int"); break;
                case "float": filedType.Add("float"); break;
                case "long": filedType.Add("long"); break;
                case "string": filedType.Add("string"); break;
                case "vector2": filedType.Add("Vector2"); break;
                case "vector3": filedType.Add("Vector3"); break;
                case "list<int>": filedType.Add("int[]"); break;
                case "list<float>": filedType.Add("float[]"); break;
                case "list<string>": filedType.Add("string[]"); break;
                case "list<vector2>": filedType.Add("vector2[]"); break;
                case "list<vector3>": filedType.Add("vector3[]"); break;
                default:  Debug.Log(className + " 暂不支持这个属性[ " + text + "]"); continue;
            }
            comments.Add(mSheet.Rows[1][i].ToString());
            filedName.Add(mSheet.Rows[2][i].ToString());
        }

        StringBuilder builder = new StringBuilder();
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        className = textInfo.ToTitleCase(className)+ "Cfg";
        builder.AppendLine("#region  " + className + "类");
        builder.AppendLine("public class " + className+ "{");
  
        for (int i = 0; i < filedType.Count; i++)
        {
            builder.AppendLine("\t/// <summary>");
            builder.AppendLine("\t///" + comments[i]);
            builder.AppendLine("\t/// </summary>");
            builder.AppendLine("\tpublic readonly " + filedType[i] + " " + filedName[i] + ";");
        }

        builder.AppendLine("}");
        builder.AppendLine("#endregion");
        textWriter.Write(builder);

    }
    #endregion
    #region
    static void WriteTableData(string excelFile, TextWriter textWriter, string className)
    {
        //CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        //TextInfo textInfo = cultureInfo.TextInfo;
        //className = textInfo.ToTitleCase(className);

        //     FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        //    IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        //        DataSet mResultSet = mExcelReader.AsDataSet();

        //判断Excel文件中是否存在数据表
        //if (mResultSet.Tables.Count < 1)
        //    return;

        //默认读取第一个数据表
        //        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        //if (mSheet.Rows.Count < 1)
        //    return;
        //是否含有唯一主键
        bool uniquenessMajorKey = true;
        //        int lastKey = -1;
        //for (int i = 0; i < mSheet.Rows.Count; i++)
        //{
        //    int key = 0;
        //    try
        //    {
        //        key = int.Parse(mSheet.Rows[i][0].ToString());
        //    }
        //    catch
        //    {
        //        continue;
        //    }

        //    if (key.Equals(lastKey))
        //    {
        //        uniquenessMajorKey = false;
        //        break;
        //    }
        //    lastKey = key;
        //}

        if (uniquenessMajorKey)
        {
            StringBuilder builder = new StringBuilder();
            string dic = "Dictionary<int, " + className + "> ";
           
            builder.AppendLine("\t#region  " + className + "配置");
            builder.AppendLine("\tprivate " + dic + "_" + className + " = null;\n");
            builder.AppendLine("\tpublic " + dic + "" +className + "Dic" +"{");
            builder.AppendLine("\t\tget{");
            builder.AppendLine("\t\t\tif (_" + className + " == null){");
            builder.AppendLine("\t\t\t\t_" + className + " = ReadTable.Read<" + dic + ">(\"" + className + "\");");
            builder.AppendLine("\t\t\t}");
            builder.AppendLine("\t\t\treturn _" + className + ";");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t}");

            builder.AppendLine("\tpublic " + className + " Get" + className + "Dic"  + "(int id){");
            builder.AppendLine("\t\t" + className + " trc = null;");
            builder.AppendLine("\t\tif(" + className + "Dic"+ ".TryGetValue(id, out trc))  {");
            builder.AppendLine("\t\t\treturn trc;");
            builder.AppendLine("\t\t}");
            builder.AppendLine("\t\treturn null;");
            builder.AppendLine("\t}");
            builder.AppendLine("\t#endregion");
            textWriter.Write(builder);
            return;
        }
        else
        {

        }
    }
    #endregion

    #region 从excel生成文本
    [MenuItem("ChangeExcl2TableClass/从excel生成文本")]
    static void GenerateText()
    {       
        string textSavePath = EditorUtility.OpenFolderPanel("转换excel文件为文本", defultPath, null);
        if (string.IsNullOrEmpty(textSavePath)) return;

        DirectoryInfo textFolder = new DirectoryInfo(textSavePath);
        if (!textFolder.Exists) textFolder.Create();

        DirectoryInfo folder = new DirectoryInfo(excelPath);
        string objectName = null;
        foreach (var file in folder.GetFiles("*.xlsx"))
        {
            objectName = file.Name;
            objectName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(objectName);
            objectName = objectName.Replace(".xlsx", "Cfg.txt");
            string textPath = textSavePath + "/" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(objectName);
            using (FileStream fileStream = new FileStream(textPath, FileMode.Create, FileAccess.Write))
            {
                using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.Unicode))
                {
                    GenerateText(file.FullName, textWriter);
                }
            }
        }

        foreach (var tempFolder in folder.GetDirectories())
        {
            char[] splitStrArr = { '\\' };
            string[] pathArr = tempFolder.FullName.Split(splitStrArr, System.StringSplitOptions.RemoveEmptyEntries);
            string textSecondSavePath = textSavePath + "/" + pathArr[pathArr.Length - 1];
            string excelSecondPath = excelPath + "/" + pathArr[pathArr.Length - 1];

            DirectoryInfo textSecondFolder = new DirectoryInfo(textSecondSavePath);
            if (!textSecondFolder.Exists) textSecondFolder.Create();

            DirectoryInfo excelSecondFolder = new DirectoryInfo(excelSecondPath);
            string objectSecondName = null;
            if (excelSecondFolder.GetFiles("*.xlsx").Length > 0)
            {
                foreach (var file in excelSecondFolder.GetFiles("*.xlsx"))
                {
                    objectSecondName = file.Name;
                    objectSecondName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(objectSecondName);
                    objectSecondName = objectSecondName.Replace(".xlsx", "Cfg.txt");

                    string textSecondPath = textSecondSavePath + "/" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(objectSecondName);

                    using (FileStream fileStream = new FileStream(textSecondPath, FileMode.Create, FileAccess.Write))
                    {
                        using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.Unicode))
                        {
                            GenerateText(file.FullName, textWriter);
                        }
                    }
                }
            }
            foreach (var file in excelSecondFolder.GetFiles("*.txt"))
            {
                objectSecondName = file.Name;
                objectSecondName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToLower(objectSecondName);
                string textSecondPath = textSecondSavePath + "/" + objectSecondName;
                file.CopyTo(textSecondPath, true);
            }
        }
        EditorUtility.DisplayDialog("成功", "数据文本生成成功", "确定");
        AssetDatabase.Refresh();
    }

    static void GenerateText(string excelFile, TextWriter textWriter)
    {
        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        if (mStream == null)
        {
             Debug.Log("表格文件有问题：【" + excelFile + "】无法正确打开文件");
            return;
        }
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        if (mExcelReader == null)
        {
             Debug.Log("表格文件有问题：【" + excelFile + "】无法正确打开文件");
            return;
        }
        DataSet mResultSet = null;
        try
        {
            mResultSet = mExcelReader.AsDataSet(true);
        }
        catch (System.Exception e)
        {
             Debug.Log("某个单元格格式有问题！表名【" + excelFile + "】"); 
            return;
        }
      
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        int colCount = mSheet.Columns.Count;
        int row = mSheet.Rows.Count;

        StringBuilder builder = new StringBuilder();
        string tableContent = null;
        for (int i = 2; i < row; i++)
        {
            for (int j = 0; j < colCount; j++)
            {

                if (string.IsNullOrEmpty(mSheet.Rows[0][j].ToString().Trim()))
                {
                    continue;
                }
                tableContent = mSheet.Rows[i][j].ToString();
                tableContent = tableContent.Replace((char)13, ' ');
                tableContent = tableContent.Replace((char)10, ' ');
                tableContent = tableContent.Trim();
                tableContent += "\t";
                builder.Append(tableContent);
            }
            builder.AppendLine();
        }
        textWriter.Write(builder);
    }
    #endregion

    #region 转换选中文本
    [MenuItem("ChangeExcl2TableClass/转换选中表格为文本")]
    public static void Change2Text()
    {
        UnityEngine.Object[] selectedObjects =  UnityEditor.Selection.objects;
        if (selectedObjects == null)
        {
             Debug.Log("没有选中任何物体！");
            return;
        }
        string textSavePath = defultPath;
        if (string.IsNullOrEmpty(textSavePath))
            return;
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(selectedObjects[i]);
         
            if (path.Contains("/ConfigGenerator/Configs") && path.EndsWith(".xlsx"))
            {
                string objectName = selectedObjects[i].name;
                string toUper = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(objectName) + "Cfg.txt";
                string textPath = textSavePath + path.Replace("Assets/Resources/Configs", "");
                textPath = textPath.Replace(objectName +".xlsx", toUper);
                using (FileStream fileStream = new FileStream(textPath, FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.Unicode))
                    {
                        GenerateText(path, textWriter);
                    }
                }
            }
            else
            {
                 Debug.Log("只对ConfigGenerator/Configs文件夹下的.xlsx文件生效");
            }
        }
        EditorUtility.DisplayDialog("转换成功", "转换成功", "确定");
        AssetDatabase.Refresh();
    }
    #endregion

    [MenuItem("ChangeExcl2TableClass/清理生成文本")]
    public static void ClearAllGenerateText()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(defultPath);
        if (!dirInfo.Exists)
            return;
        dirInfo.Delete(true);
        Directory.CreateDirectory(defultPath);
        EditorUtility.DisplayDialog("清理成功", "清理成功", "确定");
        AssetDatabase.Refresh();
    }



}
