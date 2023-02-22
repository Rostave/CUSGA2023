using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using R0.SpellRel;
using UnityEngine;


namespace R0.ScriptableObjConfig
{
    public static class ExcelImporter
    {
        private static readonly string CsvPath = $"{Application.dataPath}/Resources/Data/符文列表.csv";
        private static readonly Regex NumMatchReg = new Regex("^[1-9]\\d*$");
        // private static DataRow _titles, _segments;

        /// <summary>
        /// 从符文列表.xlsx读入符文和子弹信息
        /// </summary>
        public static void ImportData()
        {
            if (!File.Exists(CsvPath))
            {
                Debug.Log($"文件不存在: {CsvPath}");
                return;
            }
            
            var fs = new FileStream(CsvPath, FileMode.Open, FileAccess.Read, FileShare.None);
            // var sr = new StreamReader(fs, Encoding.GetEncoding(936));
            var dt = CsvToDataTable(fs);
            foreach (DataRow dr in dt.Rows)
            {
                var first = dr[0].ToString();
                if (StrIsNum(first))  // 符文
                {
                    SpellData.SpellDataStruct sd;
                    var effectType = dr[1].ToString().Split("_")[0];
                    var type = (SpellEffect) int.Parse(effectType);
                    switch (type)
                    {
                        case SpellEffect.SummonBullet:
                            sd = new SpellData.BulletSpellDataStruct();
                            break;
                        case SpellEffect.Element:
                            sd = new SpellData.ElementSpellDataStruct();
                            return;
                        default:
                            sd = new SpellData.SpellDataStruct();
                            return;
                    }
                    
                    sd.cat = (SpellCat) int.Parse(first);
                    sd.effect = type;
                    sd.name = dr[2].ToString();
                    sd.description = dr[4].ToString();
                    sd.powerCost = float.Parse(dr[5].ToString());
                        
                    switch (type)
                    {
                        case SpellEffect.SummonBullet:
                            var sd2 = (SpellData.BulletSpellDataStruct) sd;
                            sd2.dmg = float.Parse(dr[6].ToString());
                            sd2.moveSpd = float.Parse(dr[7].ToString());
                            sd2.cd = float.Parse(dr[8].ToString());
                            sd2.atkDesc = dr[11].ToString();
                            sd2.randomAngle = float.Parse(dr[12].ToString());
                            sd2.destroyOnPlayerDmgCount = int.Parse(dr[13].ToString());
                            sd2.destroyOnUsaageCount = int.Parse(dr[14].ToString());
                            sd = sd2;
                            break;
                        case SpellEffect.Element:
                            break;
                    }

                    Debug.Log(sd.cat);
                    SpellData.Instance.spellData.Add(sd);

                }
            }  
            
        }
        
        private static bool StrIsNum(string input) => NumMatchReg.IsMatch(input);
        
                /// <summary>
        /// 将Csv导入到datatable,第一行为列名
        /// </summary>
        /// <param name="stream">Csv的流数据</param>
        /// <returns>返回datatable</returns>
        public static DataTable CsvToDataTable(Stream stream)
        {
            DataTable dt = new DataTable();
 
            using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(936)))
            {
                string rowStr = null;
                bool special = false;
                bool isFirst = true;
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    rowStr += line;
                    //获取该行双引号的个数，如果是双数或为0，则为完整一行
                    //如果该行双引号个数为单数，则是非完整行，需要下次出现单数才是完整行
                    int remainder = (line.Split(new char[] { '"' }, StringSplitOptions.None).Length - 1) % 2;
                    if (remainder != 0)
                    {
                        if (special)
                        {
                            special = false;
                        }
                        else
                        {
                            rowStr += Environment.NewLine;
                            special = true;
                            continue;
                        }
                    }
                    else
                    {
                        if (special)
                        {
                            rowStr += Environment.NewLine;
                            continue;
                        }
                    }
 
                    //解析完整行的csv数据
                    string[] rowData = SplitCsvRow(rowStr);
                    rowStr = null;
                    if (isFirst)
                    {
                        isFirst = false;
                        foreach (string colName in rowData)
                        {
                            dt.Columns.Add(colName);
                        }
                    }
                    else
                    {
                        if (rowData.Length == dt.Columns.Count)
                        {
                            DataRow row = dt.NewRow();
                            row.ItemArray = rowData;
                            dt.Rows.Add(row);
                        }
                        else
                        {
                            throw new Exception("csv文件列格式错误，");
                        }
                    }
 
                }
                if (special)
                {
                    throw new Exception("csv文件格式错误");
                }
            }
 
            return dt;
        }
 
        /// <summary>
        /// 解析完整行的csv数据
        /// </summary>
        /// <param name="rowText"></param>
        /// <returns></returns>
        private static string[] SplitCsvRow(string rowText)
        {
            List<string> temp = new List<string>();
            if (rowText.IndexOf('"') < 0)
            {
                //无特殊字符，直接按逗号拆分
                temp.AddRange(rowText.Split(new char[] { ',' }));
            }
            else
            {
                var spArr = rowText.Split(new char[] { ',' });
                string cellValue = null;
                bool special = false;
                //有特殊字符
                foreach (var spItem in spArr)
                {
                    cellValue += spItem;
                    int remainder = (spItem.Split(new char[] { '"' }, StringSplitOptions.None).Length - 1) % 2;
                    if (remainder != 0)
                    {
                        if (special)
                        {
                            special = false;
                        }
                        else
                        {
                            cellValue += ',';
                            special = true;
                            continue;
                        }
                    }
                    else
                    {
                        if (special)
                        {
                            cellValue += ',';
                            continue;
                        }
                    }
                    if (special)
                    {
                        throw new Exception("csv文件行格式错误");
                    }
                    temp.Add(cellValue);
                    cellValue = null;
                }
            }
            return temp.ToArray();
        }

    }
}