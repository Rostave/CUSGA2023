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
        private const char Quote = '"';
        private const char Commas = ',';
        private static int _a, _b, _c, _d;  // 四种符文类型计数

        /// <summary>
        /// 从符文列表.xlsx读入符文和子弹信息
        /// </summary>
        public static void ImportSpellData()
        {
            if (!File.Exists(CsvPath))
            {
                Debug.Log($"文件不存在: {CsvPath}");
                return;
            }

            var data = SpellData.Instance;
            var fs = new FileStream(CsvPath, FileMode.Open, FileAccess.Read, FileShare.None);
            var dt = CsvToDataTable(fs);

            data.data.Clear();  // 清空当前数据
            _a = 0;
            _b = 0;
            _c = 0; 
            _d = 0;
            
            foreach (DataRow dr in dt.Rows)
            {
                var first = dr[0].ToString();
                if (!StrIsNum(first)) continue;

                SpellData.SpellDataStruct sd;
                var effectType = dr[1].ToString().Split('_')[0];
                var type = (SpellEffect) int.Parse(effectType);

                switch (type)
                {
                    case SpellEffect.BulletSummon:
                        if (data.bulletSpellData[_a++].isSpellInfoLocked) continue;
                        sd = new SpellData.BulletSpellDataStruct();
                        break;
                    case SpellEffect.ElementAttach:
                        if (data.elementSpellData[_b++].isSpellInfoLocked) continue;
                        sd = new SpellData.ElementSpellDataStruct();
                        break;
                    case SpellEffect.PropMod:
                        if (data.propModSpellData[_c++].isSpellInfoLocked) continue;
                        sd = new SpellData.PropModSpellDataStruct();
                        break;
                    default:
                        if (data.specialSpellData[_d++].isSpellInfoLocked) continue;
                        sd = new SpellData.SpecialSpellDataStruct();
                        break;
                }
                    
                sd.cat = (SpellCat) (int.Parse(first) - 1);
                sd.effect = type;
                sd.name = dr[2].ToString();
                sd.bgDesc = dr[4].ToString();
                sd.powerCost = float.Parse(dr[5].ToString());
                        
                switch (type)
                {
                    case SpellEffect.BulletSummon:
                        GenBulletSpellData(dr, ref sd);
                        break;
                    case SpellEffect.ElementAttach:
                        GenElementSpellData(dr, ref sd);
                        break;
                    case SpellEffect.PropMod: 
                        GenPropModSpellData(dr, ref sd);
                        break;
                   default: 
                        GenPropModSpellData(dr, ref sd);
                        break;
                }
            }

            SpellData.Instance.IntegrateSpells();  // 汇总所有符文
            Debug.Log("<color=#adff2f>载入成功</color>");
        }

        private static void GenBulletSpellData(DataRow dr, ref SpellData.SpellDataStruct sdOri)
        {
            var sd = (SpellData.BulletSpellDataStruct) sdOri;
            sd.isFacingDir = int.Parse(dr[3].ToString()) > 0;
            sd.dmg = float.Parse(dr[6].ToString());
            sd.moveSpd = float.Parse(dr[7].ToString());
            sd.cd = float.Parse(dr[8].ToString());
            sd.effectDesc = dr[11].ToString();
            sd.randomAngle = float.Parse(dr[12].ToString());
            sd.destroyOnPlayerDmgCount = int.Parse(dr[13].ToString());
            sd.destroyOnUsaageCount = int.Parse(dr[14].ToString());
            sd.defaultLifeTime = float.Parse(dr[16].ToString());
            
            if (_a > SpellData.Instance.bulletSpellData.Count) SpellData.Instance.bulletSpellData.Add(sd);
            else SpellData.Instance.bulletSpellData[_a - 1] = sd;
        }
        
        private static void GenElementSpellData(DataRow dr, ref SpellData.SpellDataStruct sdOri)
        {
            var sd = (SpellData.ElementSpellDataStruct) sdOri;
            sd.effectDesc = dr[6].ToString();
            
            if (_b > SpellData.Instance.elementSpellData.Count) SpellData.Instance.elementSpellData.Add(sd);
            else SpellData.Instance.elementSpellData[_b - 1] = sd;
        }
        
        private static void GenPropModSpellData(DataRow dr, ref SpellData.SpellDataStruct sdOri)
        {
            var sd = (SpellData.PropModSpellDataStruct) sdOri;
            sd.effectDesc = dr[6].ToString();
            
            if (_c > SpellData.Instance.propModSpellData.Count) SpellData.Instance.propModSpellData.Add(sd); 
            else SpellData.Instance.propModSpellData[_c - 1] = sd; 
        }

        private static bool StrIsNum(string input) => NumMatchReg.IsMatch(input);
        
        private static DataTable CsvToDataTable(Stream stream)
        {
            var dt = new DataTable();

            using var sr = new StreamReader(stream, Encoding.GetEncoding(936));
            var rowStr = new StringBuilder();
            var special = false;
            var isFirst = true;
            while (true)
            {
                var line = sr.ReadLine();
                if (line == null) break;
                
                rowStr.Append(line);
                
                var remainder = (line.Split(Quote).Length - 1) % 2;
                if (remainder != 0)
                {
                    if (special) special = false;
                    else
                    {
                        rowStr.Append(Environment.NewLine);
                        special = true;
                        continue;
                    }
                }
                else
                {
                    if (special)
                    {
                        rowStr.Append(Environment.NewLine);
                        continue;
                    }
                }
                
                var rowData = SplitCsvRow(rowStr.ToString());
                rowStr.Clear();
                
                if (isFirst)
                {
                    isFirst = false;
                    foreach (var colName in rowData)
                    {
                        dt.Columns.Add(colName);
                    }
                }
                else
                {
                    if (rowData.Length == dt.Columns.Count)
                    {
                        var row = dt.NewRow();
                        row.ItemArray = rowData;
                        dt.Rows.Add(row);
                    }
                    else
                    {
                        throw new Exception("csv文件列格式错误");
                    }
                }
            }
            
            if (special)
            {
                throw new Exception("csv文件格式错误");
            }

            return dt;
        }
        
        private static string[] SplitCsvRow(string rowText)
        {
            var temp = new List<string>();
            if (rowText.IndexOf('"') < 0)
            {
                temp.AddRange(rowText.Split(Commas));
            }
            else
            {
                var spArr = rowText.Split(Commas);
                var cellValueBuilder = new StringBuilder();
                var special = false;
                
                foreach (var spItem in spArr)
                {
                    cellValueBuilder.Append(spItem);
                    var remainder = (spItem.Split(Quote).Length - 1) % 2;
                    if (remainder != 0)
                    {
                        if (special)
                        {
                            special = false;
                        }
                        else
                        {
                            cellValueBuilder.Append(',');
                            special = true;
                            continue;
                        }
                    }
                    else
                    {
                        if (special)
                        {
                            cellValueBuilder.Append(',');
                            continue;
                        }
                    }
                    if (special)
                    {
                        throw new Exception("csv文件行格式错误");
                    }
                    temp.Add(cellValueBuilder.ToString());
                    cellValueBuilder.Clear();
                }
            }
            
            return temp.ToArray();
        }
    }
}