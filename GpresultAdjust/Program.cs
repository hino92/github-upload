using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GpresultAdjust
{

    public enum LineCategory
    {
        UNKNOWN,
        EMPTY,                        // 空白行
        DASH_LINE,
        ADMINISTRATIVE_TEMPLATE,
        GPO_LOCAL_GROUP_POLICY,
        FOLDER_ID,
        VALUE,
        STATE,
        
    }



    class Program
    {
 
        public static string str_EMPTY = string.Empty;
        public static string str_DASH_LINE = "---";
        public static string str_ADMINISTRATIVE_TEMPLATE = "Administrative Templates";
        public static string str_GPO_LOCAL_GROUP_POLICY = "GPO: Local Group Policy";
        public static string str_FOLDER_ID = "Folder Id:";
        public static string str_VALUE = "Value:";
        public static string str_STATE = "State:";

        public static List<AdministrativeTemplateData> m_adminTempDataList = new List<AdministrativeTemplateData>();

        static void Main(string[] args)
        {
            //gpresult /Z の出力を整えるプログラム

            if (args.Length != 1)
            {
                System.Console.WriteLine("> GpresultAdjust gpresult_output_text_file_name");
                Environment.Exit(1);
            }

            try
            {

                using (StreamReader sr = new StreamReader(args[0]))
                {

                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {

                        LineCategory category = ClassifyLine(line);


                        // Administrative Templates 行まで読み飛ばす
                        if (category != LineCategory.ADMINISTRATIVE_TEMPLATE)
                        {
                            continue;
                        }
                        // Admnistrative Templates を読む。
                        // Administrative Templatesの範囲が終わっても適当に読んでしまう。
                        // 入力ファイルで調整せよ。
                        ReadAdministrativeTemplate(sr);

                        
                    }

                    // Administrative Template 内の定義をソートする（これがこのツールの目的）
                    m_adminTempDataList.Sort();
                    
                    foreach (AdministrativeTemplateData adt in m_adminTempDataList)
                    {
                        System.Console.WriteLine("********************");
                        System.Console.Write(adt.ToText());
                        System.Console.WriteLine("********************");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.ToString());
            }
        }


        private static LineCategory ClassifyLine(string line )
        {
            //System.Console.WriteLine("ClassifyLine()");
            string targetLine = line.Trim();

            if ( targetLine.Equals( Program.str_EMPTY) )
            {
                System.Console.WriteLine(LineCategory.EMPTY.ToString());
                return LineCategory.EMPTY;
            }
            if (targetLine.StartsWith(Program.str_DASH_LINE))          //ここは StartsWith() を使う
            {
                System.Console.WriteLine(LineCategory.DASH_LINE.ToString());
                return LineCategory.DASH_LINE;
            }
            if (targetLine.Equals(Program.str_ADMINISTRATIVE_TEMPLATE))
            {
                System.Console.WriteLine(LineCategory.ADMINISTRATIVE_TEMPLATE.ToString());
                return LineCategory.ADMINISTRATIVE_TEMPLATE;
            }
            if (targetLine.Equals(Program.str_GPO_LOCAL_GROUP_POLICY))
            {
                System.Console.WriteLine(LineCategory.GPO_LOCAL_GROUP_POLICY.ToString());
                return LineCategory.GPO_LOCAL_GROUP_POLICY;
            }
            if (targetLine.Equals(Program.str_FOLDER_ID))
            {
                System.Console.WriteLine(LineCategory.FOLDER_ID.ToString());
                return LineCategory.FOLDER_ID;
            }
            if (targetLine.Equals(Program.str_VALUE))
            {
                System.Console.WriteLine(LineCategory.VALUE.ToString());
                return LineCategory.VALUE;
            }
            if (targetLine.Equals(Program.str_STATE))
            {
                System.Console.WriteLine(LineCategory.STATE.ToString());
                return LineCategory.STATE;
            }
            System.Console.WriteLine(LineCategory.UNKNOWN.ToString());
            return LineCategory.UNKNOWN;
        }

        private static void ReadAdministrativeTemplate(StreamReader sr)
        {
            System.Console.WriteLine("ReadAdministrativeTemplate()");
            string line = null;
            while ((line = sr.ReadLine())!=null) 
            {
                LineCategory category = ClassifyLine(line);

                if (category == LineCategory.GPO_LOCAL_GROUP_POLICY)
                {

                    AdministrativeTemplateData adt = new AdministrativeTemplateData();
                    System.Console.WriteLine("new adt");
                    m_adminTempDataList.Add(adt);

                    adt.m_GPO = line.Trim();

                    string line2 = sr.ReadLine();    // Folder Id:行を読む
                    adt.m_Folder_id = line2.Trim();

                    string line3 = sr.ReadLine();    // Value: 行を読む。この行がない場合があってズレるが、とりあえずこのまま。（category を見て設定するとよい。）
                    adt.m_Value = line3.Trim();

                    string line4 = sr.ReadLine();    // State: 行を読む。
                    adt.m_State = line4.Trim();

                }
                continue;
            }
        }
    }
}
